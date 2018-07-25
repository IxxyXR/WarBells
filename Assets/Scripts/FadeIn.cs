using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{

	public float Speed = 3;
	private bool IsVisible = true;

	void Start ()
	{
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			var g = gameObject.transform.GetChild(i).gameObject;
			LeanTween.alpha(g, 0, 0);
		}

		IsVisible = false;
	}

	private void OnEnable()
	{
		if (!IsVisible)
		{
			DoFade();
			Debug.Log("Fading In");
		}
	}

	public void DoFade()
	{
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			var g = gameObject.transform.GetChild(i).gameObject;
			LeanTween.alpha(g, 1, Speed);
		}

		IsVisible = true;
	}
}
