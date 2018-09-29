Shader "Hidden/Custom/Invert" {

    HLSLINCLUDE

        #include "PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        
        float _Blend;

        float4 Frag(VaryingsDefault i) : SV_Target {
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            float inverse = float3(1.0-min(color.r, 1.0), 1.0-min(color.g, 1.0), 1.0-min(color.b, 1.0));
            color = float4(lerp(color.rgb, inverse, _Blend), color.a);
            return color;
        }

    ENDHLSL

    SubShader {
    
        Cull Off ZWrite Off ZTest Always

        Pass {
        
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
        }
    }
}