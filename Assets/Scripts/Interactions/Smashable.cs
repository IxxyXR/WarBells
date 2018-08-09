using UnityEngine;


namespace Interactions
{
	public class Smashable : MonoBehaviour
	{
		public Transform ShatteredPrefab;
		public Transform Clapper;
		private Transform shattered;
		public AudioClip SmashingSound;
		public GameObject UnsmashedBell;

		public void SmashAfter(float delay)
		{
			Invoke(nameof(Smash), delay);
		}

		public void Smash()
		{
			UnsmashedBell.GetComponent<MeshRenderer>().enabled = false;
			gameObject.GetComponent<AudioSource>().PlayOneShot(SmashingSound);
			Clapper.gameObject.SetActive(false);
			shattered = Instantiate(ShatteredPrefab);
			shattered.transform.parent = gameObject.transform;
			shattered.localPosition = UnsmashedBell.transform.localPosition;
			shattered.localRotation = UnsmashedBell.transform.localRotation;
			shattered.Rotate(90,0,0);
			shattered.localScale = UnsmashedBell.transform.localScale;
			Explode();
			Invoke(nameof(FallDown), 1.2f);
		}

		public void Explode()
		{
			var pieces = shattered.gameObject.GetComponentsInChildren<Rigidbody>();
			foreach (var piece in pieces)
			{
				piece.drag = 5f;
				piece.angularDrag = .03f;
				piece.AddExplosionForce(2f, UnsmashedBell.transform.position, 10f, 1f, ForceMode.Impulse);
			}
		}

		public void FallDown()
		{
			var pieces = shattered.gameObject.GetComponentsInChildren<Rigidbody>();
			foreach (var piece in pieces)
			{
				piece.useGravity = true;
			}
		}
	}
}


