using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipplePulse : MonoBehaviour
{
	private ParticleSystem ps1, ps2;
	private SimpleAnimation bellAnimator;
	public Rigidbody jointRigidbody;
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
		jointRigidbody.isKinematic = true;
		ps1.Emit(1);
		ps2.Emit(1);
		bellAnimator.Rewind();
		bellAnimator.Play();
		audioSource.Play();
	}

	public void PhysicsPulse()
	{
		jointRigidbody.isKinematic = false;
		jointRigidbody.AddTorque(0,0,550);
		
	}
}
 