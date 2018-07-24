using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


[Serializable]
public class LoopSeekClip : PlayableAsset, ITimelineClipAsset
{
    public int label = 0;
    public int label_next = 0;
	public bool jump;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

	public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
	{
        LoopSeekBehaviour template = new LoopSeekBehaviour();
        template.clip = this;
        

        var playable = ScriptPlayable<LoopSeekBehaviour>.Create(graph, template);
        LoopSeekBehaviour behaviour = playable.GetBehaviour();
        behaviour.director = owner.GetComponent<PlayableDirector>();

        return playable;
	}
}

