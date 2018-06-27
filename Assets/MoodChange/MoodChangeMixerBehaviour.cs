using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class MoodChangeMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount ();
        Color distanceFogColor = Color.black;
        Color lightColor = Color.black;
        
        MoodChangeBehaviour lastInput = null;
        
        Texture2D fogRamp1 = null;
        Texture2D fogRamp2 = null;
        
        Color DistanceFogColor1 = Color.black;
        Color DistanceFogColor2 = Color.black;
        
        Color LightColor1 = Color.black;
        Color LightColor2 = Color.black;
        
        float blend = 1f;
                
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<MoodChangeBehaviour> inputPlayable = (ScriptPlayable<MoodChangeBehaviour>)playable.GetInput(i);
            MoodChangeBehaviour input = inputPlayable.GetBehaviour();
            
            if (inputWeight > 0)
            {
                if (fogRamp1 == null)
                {
                    fogRamp1 = input.GradientFog;
                    fogRamp2 = input.GradientFog;
                    DistanceFogColor1 = input.DistanceFogColor;
                    DistanceFogColor2 = input.DistanceFogColor;
                    LightColor1 = input.LightColor;
                    LightColor2 = input.LightColor;
                }
                else
                {
                    fogRamp2 = input.GradientFog;
                    DistanceFogColor2 = input.DistanceFogColor;
                    LightColor2 = input.LightColor;
                    blend = inputWeight;
                }
            }
            lastInput = input;
        }
        
        RenderSettings.fogColor = Color.Lerp(DistanceFogColor1, DistanceFogColor2, blend);
        
        var light = lastInput.Light.GetComponent<Light>();
        light.color = Color.Lerp(LightColor1, LightColor2, blend);

        var firewatchFog = lastInput.Camera.GetComponent<FirewatchBlendFog>();
        firewatchFog.colorRamp1 = fogRamp1;
        firewatchFog.colorRamp2 = fogRamp2;
        firewatchFog.blendAmount = blend;

    }
}
