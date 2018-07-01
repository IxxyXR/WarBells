using UnityEngine.Events;
using UnityEngine.Playables;

namespace TimelineExtension {
	class UnityEventPlayable : PlayableBehaviour {
		UnityEvent unityEvent = null;

		public void Initialize( UnityEvent aUnityEvent ) {
			unityEvent = aUnityEvent;
		}

		public void OnTrigger() {
			if( unityEvent != null )
				unityEvent.Invoke();
		}

		public override void OnBehaviourPlay( Playable playable, FrameData info ) {
			if( info.evaluationType == FrameData.EvaluationType.Playback ) // ignore scrubs
				OnTrigger();
		}

		public override void OnBehaviourDelay( Playable playable, FrameData info ) {
			base.OnBehaviourDelay( playable, info );
		}

		public static Playable Create( PlayableGraph graph, UnityEvent aUnityEvent ) {
			var scriptPlayable = ScriptPlayable<UnityEventPlayable>.Create( graph );
			scriptPlayable.GetBehaviour().Initialize( aUnityEvent );
			return scriptPlayable;
		}
	}
}
