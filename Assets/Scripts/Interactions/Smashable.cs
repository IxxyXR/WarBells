using UnityEngine;

namespace Interactions
{
	public class Smashable : MonoBehaviour
	{

		public GameObject Fragments;

		// Use this for initialization
		void Start ()
		{
			Fragments.active = false;
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public void Smash()
		{
			Fragments.active = true;
			gameObject.active = false;
		
		}
	}
}
