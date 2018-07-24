using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PauseBehaviour : PlayableBehaviour
{
    public PauseClip clip;
    public PlayableDirector director { get; set; }
    private bool paused = false;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!paused)
        {
            SingletonLoopSeek.Instance.Pause();
            if (clip.ResumeAfter > 0)
            {
                SingletonLoopSeek.Instance.ScheduleResume(clip.ResumeAfter);
            }
            paused = true;
        }
    }
    
    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        if (SingletonLoopSeek.Instance != null)
        {
            SingletonLoopSeek.Instance.Resume();            
            paused = false;
        }
    }
}
