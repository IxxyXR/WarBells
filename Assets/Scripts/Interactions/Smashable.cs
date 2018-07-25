using UnityEditor;
using UnityEngine;

namespace Interactions
{
	public class Smashable : MonoBehaviour
	{
		public bool SmashingEnabled = false;
		public Transform ShatteredPrefab;
		public Transform clapper;
		private Transform shattered;
		public SingletonLoopSeek timeline;
		private bool Smashed = false;

		void Start()
		{
			timeline = GameObject.FindGameObjectWithTag("MainTimeline").GetComponent<SingletonLoopSeek>();
		}

		public void Smash()
		{
			if (SmashingEnabled && !Smashed)
			{
				gameObject.GetComponent<MeshRenderer>().enabled = false;
				gameObject.GetComponent<AudioSource>().Play();
				clapper.gameObject.SetActive(false);
				shattered = Instantiate(ShatteredPrefab);
				shattered.transform.parent = gameObject.transform.parent;
				shattered.localPosition = new Vector3(0,0,0);
				shattered.localScale = gameObject.transform.localScale;
				Invoke(nameof(FallDown), 2);
				Smashed = true;
			}
		}

		public void FallDown()
		{
			var pieces = shattered.gameObject.GetComponentsInChildren<Rigidbody>();
			foreach (var piece in pieces)
			{
				piece.useGravity = true;
				piece.drag = 10;
			}
			Invoke(nameof(ResumeTimeline), 1);
		}

		public void ResumeTimeline()
		{
			timeline.Resume();
		}
	}
}


