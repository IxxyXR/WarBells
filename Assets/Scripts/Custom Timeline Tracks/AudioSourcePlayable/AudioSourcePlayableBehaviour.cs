using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AudioSourcePlayableBehaviour : PlayableBehaviour
{
    public float volume;
    public bool enabled;
    public float startTime;
    public AudioClip clip;

    public bool ClipPlaying;
    public int Slot = 0;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        ClipPlaying = true;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        base.OnBehaviourPause(playable, info);
        ClipPlaying = false;
    }

    
    
    
}
