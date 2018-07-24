/*
	Author: Alberto Mellado Cruz
	Date: 09/11/2017

	Comments: 
	This is just a test that would depend on the 3D Model used.
	Vertex animations would allow the use of GPU Instancing, 
	enabling the use of a dense amount of animated fish.
  	The code may not be optimized but it was just a test
*/

Shader "Custom/Document" {
	Properties{
        _Color ("Main Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
    	_Angle("Angle", Range(0, 1)) = 1
		_SpeedX("SpeedX", Range(0, 10)) = 1
		_FrequencyX("FrequencyX", Range(0, 10)) = 1
		_AmplitudeX("AmplitudeX", Range(0, 0.2)) = 1
		_SpeedY("SpeedY", Range(0, 10)) = 1
		_FrequencyY("FrequencyY", Range(0, 10)) = 1
		_AmplitudeY("AmplitudeY", Range(0, 1)) = 1
		_SpeedZ("SpeedZ", Range(0, 10)) = 1
		_FrequencyZ("FrequencyZ", Range(0, 10)) = 1
		_AmplitudeZ("AmplitudeZ", Range(0,  2)) = 1
	}
		SubShader{
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    ColorMask RGB

		Pass{

		CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"
    #define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))

    fixed4 _Color;
	sampler2D _MainTex;
	float4 _MainTex_ST;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		#if USING_FOG
            fixed fog : TEXCOORD3;
        #endif
	};
	
	float _Angle;
	
	float _SpeedX;
	float _FrequencyX;
	float _AmplitudeX;
	
	float _SpeedY;
	float _FrequencyY;
	float _AmplitudeY;

	float _SpeedZ;
	float _FrequencyZ;
	float _AmplitudeZ;


	v2f vert(appdata_base v)
	{
		v2f o;

        v.vertex.x += sin(((v.vertex.x * _Angle) + v.vertex.z + _Time.y * _SpeedX) * _FrequencyX)* _AmplitudeX;
		v.vertex.y += sin(((v.vertex.x * _Angle) + v.vertex.z + _Time.y * _SpeedY) * _FrequencyY)* _AmplitudeY;
		v.vertex.z += sin(((v.vertex.x * _Angle) + v.vertex.z + _Time.y * _SpeedZ) * _FrequencyZ)* _AmplitudeZ;		

		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
        #if USING_FOG
            float fogCoord = length(eyePos.xyz);
            UNITY_CALC_FOG_FACTOR_RAW(fogCoord);
            o.fog = saturate(unityFogFactor);
        #endif
		return o;
		
	}

	fixed4 frag(v2f i) : SV_Target
	{
    	fixed4 tex = tex2D(_MainTex, i.uv);
    	fixed4 col = tex * _Color;
        #if USING_FOG
            col.rgb = lerp(unity_FogColor.rgb, col.rgb, IN.fog);
        #endif
		return col;
	}

		ENDCG

	}
	}
		FallBack "Diffuse"
}