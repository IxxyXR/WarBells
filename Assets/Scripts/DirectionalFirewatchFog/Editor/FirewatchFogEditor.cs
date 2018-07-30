using UnityEditor;
using UnityEngine;
using UnityStandardAssets.ImageEffects;


public class FirewatchFogEditor : EditorWindow
{

	public bool NewFog = true;

	public float FwFogAmount = 250f;
	public float FwFogDensity = 0.4f;
	public Texture2D FwColorRamp1;
	public Texture2D FwColorRamp2;
	public float FwRampBlend = 0.5f;

	[MenuItem("Window/Firewatch Fog Editor")]
	public static void ShowWindow()
	{
		GetWindow(typeof(FirewatchFogEditor));
	}

	void OnEnable()
	{
		ConfigureFogs();
	}

	void OnGUI()
	{
		if (GUILayout.Button("Sync"))
		{
			FwFogAmount = Shader.GetGlobalFloat("_FWFogAmount");
			FwFogDensity = Shader.GetGlobalFloat("_FWFogDensity");
			FwColorRamp1 = (Texture2D)Shader.GetGlobalTexture("_FWColorRamp1");
			FwColorRamp2 = (Texture2D)Shader.GetGlobalTexture("_FWColorRamp2");
			FwRampBlend = Shader.GetGlobalFloat("_FWRampBlend");
		}

		NewFog = EditorGUILayout.Toggle("New Fog", NewFog);
		FwFogAmount = EditorGUILayout.Slider ("Fog Amount", FwFogAmount, 0, 500);
		FwFogDensity = EditorGUILayout.Slider ("Fog Density", FwFogDensity, 0, 1);
		FwColorRamp1 = (Texture2D)EditorGUILayout.ObjectField("Color Ramp 1", FwColorRamp1, typeof(Texture2D), true);
		FwColorRamp2 = (Texture2D)EditorGUILayout.ObjectField("Color Ramp 2", FwColorRamp2, typeof(Texture2D), true);
		FwRampBlend = EditorGUILayout.Slider ("Ramp Blend", FwRampBlend, 0, 1);
	
	}

	private void OnInspectorUpdate()
	{
		Shader.SetGlobalFloat("_FWFogAmount", FwFogAmount);
		Shader.SetGlobalFloat("_FWFogDensity", FwFogDensity);
		Shader.SetGlobalTexture("_FWColorRamp1", FwColorRamp1);
		Shader.SetGlobalTexture("_FWColorRamp2", FwColorRamp2);
		Shader.SetGlobalFloat("_FWRampBlend", FwRampBlend);
		ConfigureFogs();
	}

	private void ConfigureFogs()
	{
		var imageFx = Camera.main.GetComponent<FirewatchBlendFog>();

		if (NewFog)
		{
			imageFx.enabled = false;
			Shader.SetGlobalFloat("_MulticolorFogEnableGlobal", 1);
		}
		else
		{
			imageFx.enabled = true;
			Shader.SetGlobalFloat("_MulticolorFogEnableGlobal", 0);
		}
	}
}
