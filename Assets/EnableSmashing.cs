using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

public class EnableSmashing : MonoBehaviour {

	void Start () {
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{	
			var g = gameObject.transform.GetChild(i).gameObject;
			g.GetComponent<Selectable>().enabled = false;
			g.GetComponent<Smashable>().enabled = false;
		}
	}
	
	public void DoEnableSmashing () {
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{	
			var g = gameObject.transform.GetChild(i).gameObject;
			g.GetComponent<Selectable>().enabled = true;
			g.GetComponent<Smashable>().enabled = true;
		}
	}
}
