using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class FogMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount();
        Color fogColor = Color.black;
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<FogBehaviour> inputPlayable = (ScriptPlayable<FogBehaviour>)playable.GetInput(i);
            FogBehaviour input = inputPlayable.GetBehaviour ();
            fogColor += input.fogColor * inputWeight;
        }

        RenderSettings.fogColor = fogColor;
    }
}
