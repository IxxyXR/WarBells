using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(BlurRenderer), PostProcessEvent.BeforeStack, "Bug/Blur")]
public sealed class Blur : PostProcessEffectSettings
{}



public sealed class BlurRenderer : PostProcessEffectRenderer<Blur> {
    
    Shader _shader;
    int _tempRTID;

    public override void Init() {
        _shader = Shader.Find("Hidden/Blur");
        _tempRTID = Shader.PropertyToID("_TempRT");
    }
    
    public override void Render(PostProcessRenderContext context) {
        
        var sheet = context.propertySheets.Get(_shader);

        // step 1
        context.command.GetTemporaryRT(_tempRTID, context.width, context.height, 0, FilterMode.Bilinear, context.sourceFormat);
        context.command.BlitFullscreenTriangle(context.source, _tempRTID, sheet, 0);  // source -> tempRT

        // step 2
        context.command.SetRenderTarget(context.destination, BuiltinRenderTextureType.CameraTarget);
        context.command.SetGlobalTexture(Shader.PropertyToID("_MainTex"), _tempRTID);
        //context.command.DrawMesh(RuntimeUtilities.fullscreenTriangle, Matrix4x4.identity, sheet.Material, 0, 1); // tempRT -> destination
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        

        // release
        context.command.ReleaseTemporaryRT(_tempRTID);
    }
}

