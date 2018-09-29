using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
 

[Serializable]
[PostProcess( typeof( EdgeDetectRenderer ), PostProcessEvent.BeforeStack, "Custom/EdgeDetectNormals" )]
public sealed class EdgeDetect : PostProcessEffectSettings {
    
    public enum EdgeDetectMode {
        TriangleDepthNormals = 0,
        RobertsCrossDepthNormals = 1,
        SobelDepth = 2,
        SobelDepthThin = 3,
        TriangleLuminance = 4,
    }

    [Serializable]
    public sealed class EdgeDetectParameter : ParameterOverride<EdgeDetectMode> { }

    public EdgeDetectParameter mode = new EdgeDetectParameter { value = EdgeDetectMode.SobelDepthThin };

    public FloatParameter sensitivityDepth = new FloatParameter { value = 1.0f };
    public FloatParameter sensitivityNormals = new FloatParameter { value = 1.0f };
    public FloatParameter lumThreshold = new FloatParameter { value = .2f };
    public FloatParameter edgeExp = new FloatParameter { value = 1.0f };
    public FloatParameter sampleDist = new FloatParameter { value = 1.0f };
    [Range( 0, 1 )]
    public FloatParameter edgesOnly = new FloatParameter { value = 0.0f };
    public ColorParameter edgesOnlyBgColor = new ColorParameter { value = Color.white };
}


public sealed class EdgeDetectRenderer : PostProcessEffectRenderer<EdgeDetect> {
    
    public override DepthTextureMode GetCameraFlags() {
        var mode = settings.mode.value;
        switch ( mode ) {
            case EdgeDetect.EdgeDetectMode.TriangleDepthNormals:
            case EdgeDetect.EdgeDetectMode.RobertsCrossDepthNormals:
                return DepthTextureMode.DepthNormals;
            case EdgeDetect.EdgeDetectMode.SobelDepth:
            case EdgeDetect.EdgeDetectMode.SobelDepthThin:
                return DepthTextureMode.Depth;
        }

        return DepthTextureMode.None;
    }

    
    public override void Render( PostProcessRenderContext context ) {
        
        var sheet = context.propertySheets.Get( Shader.Find( "PostProcessing/EdgeDetectNormals" ) );

        Vector2 sensitivity = new Vector2( settings.sensitivityDepth, settings.sensitivityNormals );
        sheet.properties.SetVector( "_Sensitivity", new Vector4( sensitivity.x, sensitivity.y, 1.0f, sensitivity.y ) );
        sheet.properties.SetFloat( "_BgFade", settings.edgesOnly );
        sheet.properties.SetFloat( "_SampleDistance", settings.sampleDist );
        sheet.properties.SetVector( "_BgColor", settings.edgesOnlyBgColor.value );
        sheet.properties.SetFloat( "_Exponent", settings.edgeExp );
        sheet.properties.SetFloat( "_Threshold", settings.lumThreshold );

        context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, (int)settings.mode.value );
    }
    
}