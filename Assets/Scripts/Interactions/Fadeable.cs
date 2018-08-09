using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadeable : MonoBehaviour
{

	public float Speed = 3;
	private bool isVisible = true;

	void Start ()
	{
		DoFade(0, 0);
		isVisible = false;
	}

	void Update()
	{
		if (GvrControllerInput.ClickButtonUp)
		{
			FadeOut();
		}	
	}

	private void OnEnable()
	{
		if (!isVisible)
		{
			FadeIn();
		}
	}

	public void DoFade(float alpha, float speed)
	{
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			var g = gameObject.transform.GetChild(i).gameObject;
			LeanTween.alpha(g, alpha, speed);
		}
	}

	public void FadeIn()
	{
		DoFade(1, Speed);
		gameObject.GetComponent<AudioSource>().Play();
		isVisible = true;
	}
	
	public void FadeOut()
	{
		DoFade(0, 1);
		isVisible = false;
	}
}
