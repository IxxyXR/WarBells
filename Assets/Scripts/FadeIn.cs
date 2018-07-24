using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{

	public float Speed = 3;

	void Start ()
	{
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			Debug.Log($"Fading in {gameObject} in {Speed} seconds");
			var g = gameObject.transform.GetChild(i).gameObject;
			LeanTween.alpha(g, 0, 0);
			LeanTween.alpha(g, 1, Speed);
		}
	}
}
