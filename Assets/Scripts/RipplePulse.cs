using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipplePulse : MonoBehaviour
{
	private ParticleSystem ps1, ps2;
	private SimpleAnimation bellAnimator;
	private AudioSource audioSource;

	void Start()
	{
		ps1 = GetComponent<ParticleSystem>();
		ps2 = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
		bellAnimator = gameObject.transform.parent.GetComponent<SimpleAnimation>();
		audioSource = gameObject.transform.parent.GetComponent<AudioSource>();
		bellAnimator.Stop();
	}

	public void SinglePulse()
	{
		ps1.Emit(1);
		ps2.Emit(1);
		bellAnimator.Rewind();
		bellAnimator.Play();
		audioSource.Play();
	}
}
 