using UnityEngine;
using UnityEngine.Playables;


public class MoodChangeMixerBehaviour : PlayableBehaviour
{
    
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount ();

        MoodChangeBehaviour lastInput = null;
                
        Color lightColor1 = Color.black;
        Color lightColor2 = Color.black;
        Color skyboxTint1 = Color.black;
        Color skyboxTint2 = Color.black;
        
        Color distanceFogColor1 = Color.black;
        Color distanceFogColor2 = Color.black;
        float distanceFogStart1 = 0f;
        float distanceFogStart2 = 0f;
        float distanceFogEnd1 = 3.7f;
        float distanceFogEnd2 = 3.7f;
        
        Texture2D fogRamp1 = null;
        Texture2D fogRamp2 = null;
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
                    lightColor1 = input.LightColor;
                    lightColor2 = input.LightColor;
                    skyboxTint1 = input.SkyboxTint;
                    skyboxTint2 = input.SkyboxTint;
                    fogRamp1 = input.GradientFog;
                    fogRamp2 = input.GradientFog;
                    fogDensity1 = input.FogDensity;
                    fogDensity2 = input.FogDensity;
                    distanceFogStart1 = input.DistanceFogStart;
                    distanceFogStart2 = input.DistanceFogStart;
                    distanceFogEnd1 = input.DistanceFogEnd;
                    distanceFogEnd2 = input.DistanceFogEnd;
                    distanceFogColor1 = input.DistanceFogColor;
                    distanceFogColor2 = input.DistanceFogColor;


                }
                else  // Set the final value and the blend amount
                {
                    lightColor2 = input.LightColor;
                    skyboxTint2 = input.SkyboxTint;
                    distanceFogColor2 = input.DistanceFogColor;
                    distanceFogStart2 = input.DistanceFogStart;
                    distanceFogEnd2 = input.DistanceFogEnd;
                    fogRamp2 = input.GradientFog;
                    fogDensity2 = input.FogDensity;

                    blend = inputWeight;
                }
                lastInput = input;
            }
        }
        
        if (lastInput != null)
        {
            // Skybox tint
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(skyboxTint1, skyboxTint2, blend));
            
            // Tagged light color
            var lightColor = Color.Lerp(lightColor1, lightColor2, blend);
            if (lastInput.ControlledLight != null) {lastInput.ControlledLight.color = lightColor;}
            
            // Unity fog
            RenderSettings.fogColor = Color.Lerp(distanceFogColor1, distanceFogColor2, blend);
            RenderSettings.fogStartDistance = Mathf.Lerp(distanceFogStart1, distanceFogStart2, blend);
            RenderSettings.fogEndDistance = Mathf.Lerp(distanceFogEnd1, distanceFogEnd2, blend);

            // Shader Global fog
            var firewatchFogintensity = Mathf.Lerp(fogDensity1, fogDensity2, blend);
            Shader.SetGlobalFloat("_FWFogDensity", firewatchFogintensity);
            Shader.SetGlobalTexture("_FWColorRamp1", fogRamp1);
            Shader.SetGlobalTexture("_FWColorRamp2", fogRamp2);
            Shader.SetGlobalFloat("_FWRampBlend", blend);
            
        }
        
    }
}
