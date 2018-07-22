using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


[Serializable]
public class PauseClip : PlayableAsset, ITimelineClipAsset
{

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

	public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
	{
        PauseBehaviour template = new PauseBehaviour();
        template.clip = this;
        
        var playable = ScriptPlayable<PauseBehaviour>.Create(graph, template);
        PauseBehaviour behaviour = playable.GetBehaviour();
        behaviour.director = owner.GetComponent<PlayableDirector>();

        return playable;
	}
}

