using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(MoodChangeClip))]
public class MoodChangeTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MoodChangeMixerBehaviour>.Create (graph, inputCount);
    }

}
