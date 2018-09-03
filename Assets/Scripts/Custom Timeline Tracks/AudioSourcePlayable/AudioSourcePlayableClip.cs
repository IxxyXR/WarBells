using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AudioSourcePlayableClip : PlayableAsset, ITimelineClipAsset
{
    public AudioSourcePlayableBehaviour template = new AudioSourcePlayableBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AudioSourcePlayableBehaviour>.Create (graph, template);
        return playable;
    }
    
}
