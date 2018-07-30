// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

shader "Custom/DirectionalFirewatchFog" {

	Properties{
	    
        _Color1_F("Front Color", Color) = (1, 1, 1, 1)
        _Color1_B("Back Color", Color) = (1, 1, 1, 1)
        _Color1_L("Left Color", Color) = (1, 1, 1, 1)
        _Color1_R("Right Color", Color) = (1, 1, 1, 1)
        _Color1_T ("Top Color", Color) = (1, 1, 1, 1)
        _Color1_D ("Bottom Color", Color) = (1, 1, 1, 1)
        
        [MaterialToggle] _UnityFogEnable ("Unity Fog", Float ) = 0
        [MaterialToggle] _MulticolorFogEnable ("Multicolor Fog", Float ) = 0
        
        //_FWFogAmount("Multicolor Fog amount", float) = 250
        //_FWColorRamp("Multicolor Fog ramp", 2D) = "white" {}
        //_FWFogDensity("Multicolor Fog density", float) = 0.4
        
        
	}

	SubShader{
		Tags { "QUEUE"="Geometry" "RenderType"="Opaque" "LIGHTMODE"="ForwardBase"}

		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma fragmentoption ARB_precision_hint_fastest
			
            #pragma shader_feature UNITY_FOG

			#include "UnityCG.cginc"

			//Uniforms
            uniform sampler2D _MainTexture;
            uniform fixed4 _MainTexture_ST;

            uniform half3 _Color1_F;
            uniform half3 _Color1_B;
            uniform half3 _Color1_L;
            uniform half3 _Color1_R;
            uniform half3 _Color1_T;
            uniform half3 _Color1_D;
            
            float _UnityFogEnable;
            float _MulticolorFogEnable;
            uniform float _MulticolorFogEnableGlobal = 0;
            
            uniform float _FWFogAmount;
            uniform float _FWFogDensity;
            uniform sampler2D _FWColorRamp1;
            uniform sampler2D _FWColorRamp2;
            uniform float _FWRampBlend;

			//Direction vector constants
            static const half3 FrontDir = half3(0, 0, 1);
            static const half3 BackDir = half3(0, 0, -1);
            static const half3 LeftDir = half3(1, 0, 0);
            static const half3 RightDir = half3(-1, 0, 0);
            static const half3 TopDir = half3(0, 1, 0);
            static const half3 BottomDir = half3(0, -1, 0);
            static const half3 whiteColor = half3(1, 1, 1);

			struct vertexInput{
				float4 vertex : POSITION;
				half3 normal : NORMAL;
				half3 vColor : COLOR;
				float4 uv0 : TEXCOORD0;
			};

			struct vertexOutput{
			
				float4 pos : POSITION;
				float3 customLighting : COLOR0;
                
                UNITY_FOG_COORDS(3)
				
                float4 worldPos : TEXCOORD2;
                float3 viewDir : TEXCOORD4;
                
                UNITY_VERTEX_OUTPUT_STEREO
			};

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
				half3 normal = normalize(mul(unity_ObjectToWorld, half4(v.normal, 0))).xyz;

				//Calculating custom shadings
                half3 colorFront, colorBack, colorLeft, colorRight, colorTop, colorDown;
                fixed dirFront = max(dot(normal, FrontDir), 0.0);
                fixed dirBack = max(dot(normal, BackDir), 0.0);
                fixed dirLeft = max(dot(normal, LeftDir), 0.0);
                fixed dirRight = max(dot(normal, RightDir), 0.0);
                fixed dirTop = max(dot(normal, TopDir), 0.0);
                fixed dirBottom = max(dot(normal, BottomDir), 0.0);

                colorFront = colorBack = colorLeft = colorRight = colorTop = colorDown = v.vColor;

                colorFront = _Color1_F; 
                colorBack = _Color1_B;
                colorLeft = _Color1_L;
                colorRight = _Color1_R;
                colorTop = _Color1_T;
                colorDown = _Color1_D;

				o.customLighting = 
				    lerp(colorFront, whiteColor, 1-dirFront) * 
				    lerp(colorBack, whiteColor, 1-dirBack) * 
				    lerp(colorLeft, whiteColor, 1-dirLeft) * 
				    lerp(colorRight, whiteColor, 1-dirRight) * 
				    lerp(colorTop, whiteColor, 1-dirTop) * 
				    lerp(colorDown, whiteColor, 1-dirBottom);
				
				if (_MulticolorFogEnable)
                {
                    o.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);
                    o.worldPos.w = o.pos.z;
                    o.viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
                }
                
                UNITY_TRANSFER_FOG(o, o.pos);
				
				return o;
			}
			
			fixed4 frag(vertexOutput i) : COLOR
			{
    			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 Result = fixed4(whiteColor,1);				
                Result *= fixed4(i.customLighting, 1);
                
				if (_UnityFogEnable)
				{
					UNITY_APPLY_FOG(i.fogCoord, Result);
                }
                
                if (_MulticolorFogEnable && _MulticolorFogEnableGlobal)
                {
                    float camDist = distance(i.worldPos, _WorldSpaceCameraPos);	
                    
                    // By the book exp, Hopeless
                    //float fogFactor = camDist;
                    //float fogFactor = exp(-_FWFogDensity * camDist);
                    
                    // Seems to be close: 0.4 corresponds with about 0.5
                    float fogFactor =  (_FWFogDensity/10) * (camDist/2);
                    fogFactor = exp2(-fogFactor);
                    float2 rampPos = float2(saturate(camDist/_FWFogAmount/2), 0);
                    fixed4 fogColor1 = tex2D(_FWColorRamp1, rampPos);
                    fixed4 fogColor2 = tex2D(_FWColorRamp2, rampPos);
                    fixed4 fogColor = lerp(fogColor1, fogColor2, _FWRampBlend);
                    Result.rgb = lerp(fogColor.rgb, Result.rgb, saturate(fogFactor));
                }
				return  Result;
				
			}
			
			ENDCG
		}
	}
	FallBack "Standard"
}