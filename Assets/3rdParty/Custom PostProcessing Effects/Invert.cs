﻿using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
 

[Serializable]
[PostProcess(typeof(InvertRenderer), PostProcessEvent.AfterStack, "Custom/Invert")]
public sealed class Invert : PostProcessEffectSettings {
	[Range(0f, 1f), Tooltip("Effect intensity.")]
	public FloatParameter blend = new FloatParameter { value = 0.5f };
}

public sealed class InvertRenderer : PostProcessEffectRenderer<Invert> {
	
	public override void Render(PostProcessRenderContext context) {
		
		var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Invert"));
		sheet.properties.SetFloat("_Blend", settings.blend);
		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		
	}
}