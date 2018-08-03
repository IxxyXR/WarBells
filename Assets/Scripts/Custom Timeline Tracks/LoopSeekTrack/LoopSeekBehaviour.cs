using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class LoopSeekBehaviour : PlayableBehaviour
{
    public LoopSeekClip clip;
    public PlayableDirector director { get; set; }

    // �J�n���Ɉ�xOnBehaviourPause���Ă΂�邱�Ƃ̉��p
    bool init = false;

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
        if (init)
        {
            if ((bool)clip?.jump)
            {
                SingletonLoopSeek.Instance.SetTime(clip.label_next, true);                
            }
        }
        else
        {
            init = true;
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData) { }
}
