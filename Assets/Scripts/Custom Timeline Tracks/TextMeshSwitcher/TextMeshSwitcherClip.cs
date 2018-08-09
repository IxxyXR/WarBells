using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TextMeshSwitcherClip : PlayableAsset, ITimelineClipAsset
{
    public TextMeshSwitcherBehaviour template = new TextMeshSwitcherBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextMeshSwitcherBehaviour>.Create (graph, template);
        return playable;    }
}
