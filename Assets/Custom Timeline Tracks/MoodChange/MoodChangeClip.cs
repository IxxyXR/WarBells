using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[Serializable]
public class MoodChangeClip : PlayableAsset, ITimelineClipAsset
{
    public MoodChangeBehaviour template = new MoodChangeBehaviour ();
    public ExposedReference<Light> Light;
    public ExposedReference<Camera> Camera;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MoodChangeBehaviour>.Create (graph, template);
        MoodChangeBehaviour clone = playable.GetBehaviour ();
        clone.Light = Light.Resolve (graph.GetResolver ());
        clone.Camera = Camera.Resolve (graph.GetResolver ());
        return playable;
    }
}
