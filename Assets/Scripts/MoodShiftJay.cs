using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class MoodShiftJay : MonoBehaviour {


// MoodShift allows you to change the inside and outside lighting. 
// Works with FirewatchFog script, Lighting Fog and a point light on a timeline
// Define a mood type set the colour values and set with a gradient png file
// Add MoodShift script to an Empty game object in your scene
// Add FirewatchFog to the main camera

public Camera camera;
public Texture2D colorRamp;
private FirewatchFog gradientScript;

	// Use this for initialization
	void Start () {
		gradientScript = this.camera.GetComponent<FirewatchFog>();
		colorRamp = gradientScript.colorRamp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
