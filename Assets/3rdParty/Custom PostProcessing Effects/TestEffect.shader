Shader "Hidden/Custom/TestEffect" {

    HLSLINCLUDE

        #include "PostProcessing/Shaders/StdLib.hlsl"

        #pragma multi_compile _ USE_DEPTH
        #pragma multi_compile _ USE_NORMAL
        
        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float2 _MainTex_TexelSize;
    
        sampler2D _CameraGBufferTexture2;
    
        half4 _Color;
        half4 _BgColor;
    
        float _LowThreshold;
        float _HighThreshold;
    
        float _ColorSensitivity;
        float _DepthSensitivity;
        float _NormalSensitivity;
    
        float _FallOffDepth;

        float4 Frag(VaryingsDefault i) : SV_Target {
        //float4 Frag(VaryingsDefault i) : SV_Target {
        
            float4 disp = float4(_MainTex_TexelSize.xy, -_MainTex_TexelSize.x, 0);
    
            // four sample points for the roberts cross operator
            float2 uv0 = i.texcoord;           // TL
            float2 uv1 = i.texcoord + disp.xy; // BR
            float2 uv2 = i.texcoord + disp.xw; // TR
            float2 uv3 = i.texcoord + disp.wy; // BL
    
            float edge = 0;
    
//            #ifdef USE_DEPTH
//    
//            // sample depth values
//            float zs0 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv0);
//            float zs1 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv1);
//            float zs2 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv2);
//            float zs3 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv3);
//    
//            // calculate fall-off parameter from the depth of the nearest point
//            float zm = min(min(min(zs0, zs1), zs2), zs3);
//            float falloff = 1.0 - saturate(LinearEyeDepth(zm) / _FallOffDepth);
//    
//            // convert to linear depth value
//            float z0 = Linear01Depth(zs0);
//            float z1 = Linear01Depth(zs1);
//            float z2 = Linear01Depth(zs2);
//            float z3 = Linear01Depth(zs3);
//    
//            // roberts cross operator
//            float zg1 = z1 - z0;
//            float zg2 = z3 - z2;
//            float zg = sqrt(zg1 * zg1 + zg2 * zg2);
//    
//            edge = zg * falloff * _DepthSensitivity / Linear01Depth(zm);
//    
//            #endif
    
//            #ifdef USE_NORMAL
//    
//            // sample normal vector values from the g-buffer
//            float3 n0 = SAMPLE_TEXTURE2D(_CameraGBufferTexture2, uv0);
//            float3 n1 = SAMPLE_TEXTURE2D(_CameraGBufferTexture2, uv1);
//            float3 n2 = SAMPLE_TEXTURE2D(_CameraGBufferTexture2, uv2);
//            float3 n3 = SAMPLE_TEXTURE2D(_CameraGBufferTexture2, uv3);
//    
//            // roberts cross operator
//            float3 ng1 = n1 - n0;
//            float3 ng2 = n3 - n2;
//            float ng = sqrt(dot(ng1, ng1) + dot(ng2, ng2));
//    
//            edge = max(edge, ng * _NormalSensitivity);
//    
//            #endif
    
            // thresholding
            edge = saturate((edge - _LowThreshold) / (_HighThreshold - _LowThreshold));
    
            half4 cs = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            half3 c0 = lerp(cs.rgb, _BgColor.rgb, _BgColor.a);
            half3 co = lerp(c0, _Color.rgb, edge * _Color.a);
            return float4(edge, edge, edge, cs.a);
            
        }

    ENDHLSL

    SubShader {
    
        Cull Off ZWrite Off ZTest Always

        Pass {
            
            ZTest Always Cull Off ZWrite Off
            
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
                
        }
    }
}