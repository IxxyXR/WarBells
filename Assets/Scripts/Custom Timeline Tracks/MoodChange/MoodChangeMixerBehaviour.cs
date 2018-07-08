using UnityEngine;
using UnityEngine.Playables;
using UnityStandardAssets.ImageEffects;
// ReSharper disable CheckNamespace


public class MoodChangeMixerBehaviour : PlayableBehaviour
{
    
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount ();

        MoodChangeBehaviour lastInput = null;
        
        Texture2D fogRamp1 = null;
        Texture2D fogRamp2 = null;
        
        Color distanceFogColor1 = Color.black;
        Color distanceFogColor2 = Color.black;
        
        Color lightColor1 = Color.black;
        Color lightColor2 = Color.black;

        float fogDensity1 = 1f;
        float fogDensity2 = 1f;

        float blend = 1f;
                
        for (int i = 0; i < inputCount; i++)
        {
            
            float inputWeight = playable.GetInputWeight(i);
            
            if (inputWeight > 0)  // Assumes we'll only have 1 or 2 inputs with a weight above 0
            {
                ScriptPlayable<MoodChangeBehaviour> inputPlayable = (ScriptPlayable<MoodChangeBehaviour>)playable.GetInput(i);
                MoodChangeBehaviour input = inputPlayable.GetBehaviour();
                
                if (fogRamp1 == null)  // Set start and end values to be the same
                {
                    fogRamp1 = input.GradientFog;
                    fogRamp2 = input.GradientFog;
                    distanceFogColor1 = input.DistanceFogColor;
                    distanceFogColor2 = input.DistanceFogColor;
                    lightColor1 = input.LightColor;
                    lightColor2 = input.LightColor;
                    fogDensity1 = input.FogDensity;
                    fogDensity2 = input.FogDensity;

                }
                else  // Set the final value and the blend amount
                {
                    fogRamp2 = input.GradientFog;
                    distanceFogColor2 = input.DistanceFogColor;
                    lightColor2 = input.LightColor;
                    fogDensity1 = input.FogDensity;
                    fogDensity2 = input.FogDensity;

                    blend = inputWeight;
                }
                lastInput = input;
            }
            
        }
        
        if (lastInput != null)
        {
            RenderSettings.fogColor = Color.Lerp(distanceFogColor1, distanceFogColor2, blend);
            if (lastInput.ControlledLight != null)
            {
                lastInput.ControlledLight.color = Color.Lerp(lightColor1, lightColor2, blend);                
            }
            else
            {
                //Debug.LogError("Please tag at least one light as FX Light");
            }

            var firewatchFog = lastInput.ControlledCamera?.GetComponent<FirewatchBlendFog>();
            if (firewatchFog != null)
            {
                firewatchFog.colorRamp1 = fogRamp1;
                firewatchFog.colorRamp2 = fogRamp2;
                firewatchFog.blendAmount = blend;
                firewatchFog.fogIntensity = Mathf.Lerp(fogDensity1, fogDensity2, blend);
            }            
        }
        
    }
}
