using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class FXBlendClip : PlayableAsset, ITimelineClipAsset
{
    public FXBlendBehaviour template = new FXBlendBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<FXBlendBehaviour>.Create (graph, template);
        return playable;
    }
}
