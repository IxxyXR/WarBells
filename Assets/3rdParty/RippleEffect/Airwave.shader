// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Airwave"
{
	Properties
	{
		_TintColor("Color", Color) = (1, 1, 1, 1)
		_WaveNormal("Wave Normal", 2D) = "white" {}
		_WaveStrength("Wave Strength", Float) = 1
		_InvFade("Soft Particles Factor", Range(0, 3)) = 1
	}

	SubShader
	{
		// Draw ourselves after all opaque geometry
		Tags{ "Queue" = "Transparent"  }

		// Grab the screen behind the object into _BackgroundTexture
		GrabPass { "_BackgroundTexture"}

		//Blend SrcAlpha OneMinusSrcAlpha
		//ZTest Always
		//Zwrite Off

		// Render the object with the texture generated above, and invert the colors
		Pass
		{
    		Cull Off
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles

			#include "UnityCG.cginc"

			sampler2D _BackgroundTexture;
			sampler2D _WaveNormal;

			float _WaveStrength;
			fixed4 _TintColor;

			// Soft particles
			sampler2D_float _CameraDepthTexture;
			float _InvFade;

			struct v2f
			{
				float4 grabPos : TEXCOORD0;
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
				float2 uv_WaveNormal : TEXCOORD1;
				float4 screenpos : TEXCOORD2;
				UNITY_VERTEX_OUTPUT_STEREO
				//#if USING_FOG
                //    fixed fog : TEXCOORD3;
                //#endif

			};

			v2f vert(appdata_full v)
			{
				v2f o;
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				// use UnityObjectToClipPos from UnityCG.cginc to calculate 
				// the clip-space of the vertex
				o.pos = UnityObjectToClipPos(v.vertex);
				o.screenpos = ComputeScreenPos(o.pos);
                            

                
				// use ComputeGrabScreenPos function from UnityCG.cginc
				// to get the correct texture coordinate
				o.grabPos = ComputeGrabScreenPos(o.pos);

				o.color = v.color;
				o.uv_WaveNormal = v.texcoord;
				
                //#if USING_FOG
                //    float fogCoord = length(eyePos.xyz);
                //    UNITY_CALC_FOG_FACTOR_RAW(fogCoord);
                //    o.fog = saturate(unityFogFactor);
                //#endif

				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
			
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				half3 wave = UnpackNormal(tex2D(_WaveNormal, i.uv_WaveNormal.xy));

				i.grabPos.xy += wave.xy / wave.z * _WaveStrength * i.color.a;
				half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);
				
				
				
                // screenspace coordinates with offset
                float4 screen = float4(i.screenpos.xy + bgcolor, i.screenpos.zw);
                                
				// calculating depth with offset for frag and scene
                float sceneZ = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(screen)));
                float fragZ = screen.z;
    
                // masking out the refraction for objects above water surface
                float mask = step(fragZ, sceneZ);
                float4 refrmasked = bgcolor * mask;
                
                

				
				return refrmasked * _TintColor;
			}
			ENDCG
		}

	}
	FallBack "Particle/AlphaBlended"
}
