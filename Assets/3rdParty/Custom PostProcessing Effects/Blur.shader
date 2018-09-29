Shader "Hidden/Blur" {
    
    HLSLINCLUDE

        #include "PostProcessing/Shaders/StdLib.hlsl"
    
        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        
        float4 _MainTex_TexelSize;
    
        half4 FragBlur(VaryingsDefault i) : SV_Target {
            return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord) * half4(1, 0, 0, 1);
        }
        
        half4 FragBlend(VaryingsDefault i) : SV_Target {
            return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        }

    ENDHLSL
    
    SubShader {
    
        Cull Off ZWrite Off ZTest Always

        Pass { 
        
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment FragBlur
            ENDHLSL
        }

        Pass {
       
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment FragBlend
            ENDHLSL
        }
    }
}