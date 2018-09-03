using UnityEngine;
using UnityEngine.Playables;


public class AudioSourcePlayableMixerBehaviour : PlayableBehaviour
{
    float m_DefaultVolume;
    bool m_DefaultEnabled;
    float m_DefaultTime;

    float m_AssignedVolume;
    bool m_AssignedEnabled;
    float m_AssignedTime;
    
    private bool clipStarted1;
    private bool clipStarted2;
    private int numSlots;
    
    AudioSource audioSource1;
    AudioSource audioSource2;
    
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (audioSource1 == null)
        {
            audioSource1 = playerData as AudioSource;
        }

        if (audioSource2 == null)
        {
            var audioSources = audioSource1.gameObject.GetComponents<AudioSource>();
            if (audioSources.Length < 2)
            {
                audioSource2 = audioSource1.gameObject.AddComponent<AudioSource>();
                audioSource1.GetCopyOf(audioSource2);
            }
            else
            {
                audioSource2 = audioSources[1];
            }
        }

        if (audioSource1 == null)
            return;
        
        if (!Mathf.Approximately(audioSource1.volume, m_AssignedVolume))
            m_DefaultVolume = audioSource1.volume;
        if (audioSource1.enabled != m_AssignedEnabled)
            m_DefaultEnabled = audioSource1.enabled;
        if (!Mathf.Approximately(audioSource1.time, m_AssignedTime))
            m_DefaultTime = audioSource1.time;

        int inputCount = playable.GetInputCount();
        
        float blendedVolume = 0f;
        float blendedTime = 0f;
        float totalWeight = 0f;
        float greatestWeight = 0f;
        int currentInputs = 0;

        numSlots = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<AudioSourcePlayableBehaviour> inputPlayable = (ScriptPlayable<AudioSourcePlayableBehaviour>)playable.GetInput(i);
            AudioSourcePlayableBehaviour input = inputPlayable.GetBehaviour();

            if (inputWeight >= 0.001f)
            {
                if (input.ClipPlaying)
                {
                    
                    if (numSlots==0)
                    {
                        if (!clipStarted1)
                        {
                            audioSource1.clip = input.clip;
                            audioSource1.Play();
                            clipStarted1 = true;
                            numSlots = 1;
                            input.Slot = 1;
                        }
                    }
                    else if (numSlots == 1)
                    {
                        if (!clipStarted2)
                        {
                            audioSource2.clip = input.clip;
                            audioSource2.Play();
                            clipStarted2 = true;
                            numSlots = 2;
                            input.Slot = 2;
                        }
                    }
                }
            }

            blendedVolume += input.volume * inputWeight;
            blendedTime += input.startTime * inputWeight;
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                m_AssignedEnabled = input.enabled;
                audioSource1.enabled = m_AssignedEnabled;
                greatestWeight = inputWeight;
            }

            if (!Mathf.Approximately (inputWeight, 0f))
                currentInputs++;
        }
        
        m_AssignedVolume = blendedVolume + m_DefaultVolume * (1f - totalWeight);
        audioSource1.volume = m_AssignedVolume;
        m_AssignedTime = blendedTime + m_DefaultTime * (1f - totalWeight);
        
        //m_TrackBinding.time = m_AssignedTime;

        if (currentInputs != 1 && 1f - totalWeight > greatestWeight)
        {
            audioSource1.enabled = m_DefaultEnabled;
        }

    }

    public override void OnGraphStop(Playable playable)
    {
        base.OnGraphStop(playable);
        audioSource1.Stop();
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
        base.PrepareFrame(playable, info);
        var inputCount = playable.GetInputCount();

        int numPlaying = 0;
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<AudioSourcePlayableBehaviour> inputPlayable = (ScriptPlayable<AudioSourcePlayableBehaviour>)playable.GetInput(i);
            AudioSourcePlayableBehaviour input = inputPlayable.GetBehaviour();
            
            if (inputWeight < 0.001f)
            {
                if (input.Slot == 1)
                {
                    audioSource1.Stop();
                    audioSource1.clip = null;
                    clipStarted1 = false;
                    input.Slot = 0;

                } else if (input.Slot == 2)
                {
                    audioSource2.Stop();
                    audioSource2.clip = null;
                    clipStarted2 = false;
                    input.Slot = 0;
                }
            }
        }

       

    }    
}
