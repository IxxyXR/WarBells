using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Rendering.PostProcessing;

public class FXBlendMixerBehaviour : PlayableBehaviour
{
    float m_DefaultWeight;

    float m_AssignedWeight;

    PostProcessVolume m_TrackBinding;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_TrackBinding = playerData as PostProcessVolume;

        if (m_TrackBinding == null)
            return;

        if (!Mathf.Approximately(m_TrackBinding.weight, m_AssignedWeight))
            m_DefaultWeight = m_TrackBinding.weight;

        int inputCount = playable.GetInputCount ();

        float blendedWeight = 0f;
        float totalWeight = 0f;
        float greatestWeight = 0f;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<FXBlendBehaviour> inputPlayable = (ScriptPlayable<FXBlendBehaviour>)playable.GetInput(i);
            FXBlendBehaviour input = inputPlayable.GetBehaviour ();
            
            blendedWeight += input.weight * inputWeight;
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
            }
        }

        m_AssignedWeight = blendedWeight + m_DefaultWeight * (1f - totalWeight);
        m_TrackBinding.weight = m_AssignedWeight;
    }
}
