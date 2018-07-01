using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace TimelineExtension {
	[System.Serializable]
	public class EventClip : PlayableAsset {
		public string eventName = string.Empty;

		public override Playable CreatePlayable( PlayableGraph graph, GameObject go ) {
			if( string.IsNullOrEmpty( eventName ) )
				return Playable.Create( graph );

			var eventTable = go.GetComponent<EventTable>();
			if( eventTable == null )
				return Playable.Create( graph );

			var unityEvent = eventTable.GetEvent( eventName, false );
			if( unityEvent == null )
				return Playable.Create( graph );

			return UnityEventPlayable.Create( graph, unityEvent );
		}
	}
}
