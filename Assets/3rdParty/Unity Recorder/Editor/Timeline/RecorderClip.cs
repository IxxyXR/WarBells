using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Recorder.Timeline
{
    [DisplayName("Recorder Clip")]
    public class RecorderClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        public RecorderSettings settings;

        readonly SceneHook m_SceneHook = new SceneHook(Guid.NewGuid().ToString());

        Type recorderType
        {
            get { return settings == null ? null : RecordersInventory.GetRecorderInfo(settings.GetType()).recorderType; }
        }

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<RecorderPlayableBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();
            if (recorderType != null && UnityHelpers.IsPlaying())
            {
                behaviour.session = m_SceneHook.CreateRecorderSession(settings);
            }
            return playable;
        }

        public void OnDestroy()
        {
            UnityHelpers.Destroy( settings, true );
        }
    }
}