using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipplePulse : MonoBehaviour
{
	private ParticleSystem ps1, ps2;
	private ParticleSystem.EmissionModule emission;
	private SimpleAnimation bellAnimator;
	private Vector3 shakeRot;

	void Start()
	{
		ps1 = GetComponent<ParticleSystem>();
		ps2 = GetComponentInChildren<ParticleSystem>();
		emission = ps1.emission;
		bellAnimator = gameObject.transform.parent.GetComponent<SimpleAnimation>();
	}

	public void SinglePulse()
	{
		ps1.Emit(1);
		ps2.Emit(1);
		bellAnimator.Play();
	}
}
