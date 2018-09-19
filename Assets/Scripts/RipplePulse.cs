using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipplePulse : MonoBehaviour
{
	private ParticleSystem airwavePS, visiblePS;
	private SimpleAnimation bellAnimator;
	public Rigidbody jointRigidbody;
	public Rigidbody clapperRigidbody;
	private AudioSource audioSource;

	void Start()
	{
		airwavePS = GetComponent<ParticleSystem>();
		visiblePS = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
		bellAnimator = gameObject.transform.parent.GetComponent<SimpleAnimation>();
		audioSource = gameObject.transform.parent.GetComponent<AudioSource>();
		bellAnimator.Stop();
	}

	public void SinglePulse()
	{
		jointRigidbody.isKinematic = true;
		playRipples();
		bellAnimator.Rewind();
		bellAnimator.Play();
		audioSource.Play();
	}

	public void PhysicsPulse(float force=500)
	{ 
		jointRigidbody.angularDrag = .1f;
		clapperRigidbody.angularDrag = .1f;
		jointRigidbody.isKinematic = false;
		jointRigidbody.AddTorque(0,0,force);
		
	}
	
	public void SlowBell(float drag)
	{
		jointRigidbody.angularDrag = drag;
		clapperRigidbody.angularDrag = drag;
	}

	public void playRipples()
	{
		airwavePS.Emit(1);
		visiblePS.Emit(1);
		Debug.Log("EMIT");
	}
}
 