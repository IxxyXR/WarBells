using UnityEngine;

namespace Interactions
{
	public class ResumeAfterTime : MonoBehaviour
	{

		public SingletonLoopSeek timeline;
		public float delay;
	
		void Start ()
		{
			Debug.Log($"Resuming in {delay} seconds");
			Invoke(nameof(Resume), delay);
		}

		public void Resume()
		{
			timeline.Resume();
		}
	}
}
