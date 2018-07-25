using UnityEngine;

namespace Interactions
{
	public class Smashable : MonoBehaviour
	{
		public bool SmashingEnabled = false;
		public Transform ShatteredPrefab;
		public Transform clapper;
		private Transform shattered;

		public void Smash()
		{
			if (SmashingEnabled)
			{
				gameObject.GetComponent<MeshRenderer>().enabled = false;
				gameObject.GetComponent<AudioSource>().Play();
				clapper.gameObject.SetActive(false);
				shattered = Instantiate(ShatteredPrefab);
				shattered.transform.parent = gameObject.transform.parent;
				shattered.localPosition = new Vector3(0,0,0);
				shattered.localScale = gameObject.transform.localScale;
				Invoke(nameof(FallDown), 2);				
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
		}
	}
}


