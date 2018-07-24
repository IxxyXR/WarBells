using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor (0.0f, 1.0f, 1.0f)]
[TrackClipType (typeof(LoopSeekClip))]
[TrackClipType (typeof(PauseClip))]
public class LoopSeekTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var scriptPlayable = ScriptPlayable<LoopSeekBehaviour>.Create(graph, inputCount);

        //LoopSeekBehaviour b = scriptPlayable.GetBehaviour();
        if (SingletonLoopSeek.Instance != null)
        {
            SingletonLoopSeek.Instance.ClearDic();    
        }

        //This foreach will rename clips based on what they do, and collect the markers and put them into a dictionary
        //Since this happens when you enter Preview or Play mode, the object holding the Timeline must be enabled or you won't see any change in names
        foreach (var c in GetClips())
        {
            if (c.asset.GetType() == typeof(LoopSeekClip))
            {
                var clip = (LoopSeekClip) c.asset;
                string clipName = c.displayName;

                if (clip.jump)
                {
                    if (clip.label == clip.label_next)
                    {
                        clipName = "⟲ ";
                    }
                    else if (clip.label_next > clip.label)
                    {
                        clipName = "↷ ";
                    }
                    else if (clip.label_next < clip.label)
                    {
                        clipName = "↶ ";
                    }
                    
                    clipName += clip.label.ToString() + " > " + clip.label_next.ToString();
                }
                else
                {
                    clipName = clip.label.ToString();
                }

                
                

                // c.startをSingletonに追加する
                if (SingletonLoopSeek.Instance != null)
                {
                    SingletonLoopSeek.Instance.AddLabelTime(clip.label, c.start);                        
                }

                c.displayName = clipName;
            }
        }

        return scriptPlayable;
    }
}
