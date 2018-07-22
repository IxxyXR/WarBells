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

    // 開始時に一度OnBehaviourPauseが呼ばれることの回避用
    bool init = false;

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        //Debug.Log("Pause!");

        if (init)
        {
            SingletonLoopSeek.Instance.SetTime(clip.label_next, true);
        }
        else
        {
            init = true;
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData) { }
}
