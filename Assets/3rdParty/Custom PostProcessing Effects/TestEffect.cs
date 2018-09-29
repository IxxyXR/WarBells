using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
 
[Serializable]
[PostProcess(typeof(TestEffectRenderer), PostProcessEvent.AfterStack, "Custom/TestEffect")]
public sealed class TestEffect : PostProcessEffectSettings {
	
	public ColorParameter lineColor = new ColorParameter { value = Color.black };
	
	public ColorParameter backgroundColor = new ColorParameter { value = new Color(1, 1, 1, 0) };
	
	[Range(0, 1)]
	public FloatParameter lowThreshold = new FloatParameter { value =  0.05f };
	
	[Range(0, 1)]
	public FloatParameter highThreshold = new FloatParameter { value = 0.5f };
	
	[Range(0, 1)]
	public FloatParameter colorSensitivity = new FloatParameter { value = 1 };

	[Range(0, 1)]
	public FloatParameter depthSensitivity = new FloatParameter { value = 1 };
	
	[Range(0, 1)]
	public FloatParameter normalSensitivity = new FloatParameter { value = 0 };
	
	public FloatParameter fallOffDepth = new FloatParameter { value = 40 };
	
}
 

public sealed class TestEffectRenderer : PostProcessEffectRenderer<TestEffect> {
	
	public override void Render(PostProcessRenderContext context) {
		
		var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/TestEffect"));
		sheet.properties.SetColor("_Color", settings.lineColor);
		sheet.properties.SetColor("_BgColor", settings.backgroundColor);
		sheet.properties.SetFloat("_LowThreshold", settings.lowThreshold);
		var hi = Mathf.Max(settings.lowThreshold, settings.highThreshold);
		sheet.properties.SetFloat("_HighThreshold", hi);
		sheet.properties.SetFloat("_ColorSensitivity", settings.colorSensitivity);
		sheet.properties.SetFloat("_DepthSensitivity", settings.depthSensitivity);
		sheet.properties.SetFloat("_NormalSensitivity", settings.normalSensitivity);
		sheet.properties.SetFloat("_FallOffDepth", settings.fallOffDepth);
		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		
	}
	
}