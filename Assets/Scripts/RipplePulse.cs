using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipplePulse : MonoBehaviour
{
	private ParticleSystem ps;
	private ParticleSystem.EmissionModule emission;

	void Start()
	{
		ps = GetComponent<ParticleSystem>();
		emission = ps.emission;
	}

	public void SinglePulse() {
		ps.Emit(1);	
	}
}
