using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[Serializable]
public class MoodChangeClip : PlayableAsset, ITimelineClipAsset
{
    public MoodChangeBehaviour template = new MoodChangeBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MoodChangeBehaviour>.Create (graph, template);
        template.ControlledCamera = Camera.main;
        template.ControlledLight = GameObject.FindGameObjectWithTag("FX Light")?.GetComponentInChildren<Light>();
        return playable;
    }
}
