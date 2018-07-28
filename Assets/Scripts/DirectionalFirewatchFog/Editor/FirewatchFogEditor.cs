using UnityEditor;
using UnityEngine;

public class FirewatchFogEditor : EditorWindow
{
	float _FWFogAmount = 250f;
	float _FWFogDensity = 0.4f;
	Texture2D _FWColorRamp1;
	Texture2D _FWColorRamp2;
	float _FWRampBlend = 0.5f;
	
	[MenuItem("Window/Firewatch Fog Editor")]
	public static void ShowWindow()
	{
		GetWindow(typeof(FirewatchFogEditor));
	}
    
	void OnGUI()
	{
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		_FWFogAmount = EditorGUILayout.Slider ("Fog Amount", _FWFogAmount, 0, 500);
		_FWFogDensity = EditorGUILayout.Slider ("Fog Density", _FWFogDensity, 0, 1);
		_FWColorRamp1 = (Texture2D)EditorGUILayout.ObjectField("Color Ramp 1", _FWColorRamp1, typeof(Texture2D));
		_FWColorRamp2 = (Texture2D)EditorGUILayout.ObjectField("Color Ramp 2", _FWColorRamp2, typeof(Texture2D));
		_FWRampBlend = EditorGUILayout.Slider ("Ramp Blend", _FWRampBlend, 0, 1);

	}

	private void OnInspectorUpdate()
	{
		Shader.SetGlobalFloat("_FWFogAmount", _FWFogAmount);
		Shader.SetGlobalFloat("_FWFogDensity", _FWFogDensity);
		Shader.SetGlobalTexture("_FWColorRamp1", _FWColorRamp1);
		Shader.SetGlobalTexture("_FWColorRamp2", _FWColorRamp2);
		Shader.SetGlobalFloat("_FWRampBlend", _FWRampBlend);
	}
}