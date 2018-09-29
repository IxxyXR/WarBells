Shader "PostProcessing/EdgeDetectNormals" {
	
    HLSLINCLUDE
	
        #include "PostProcessing/Shaders/StdLib.hlsl"
	
		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv[5] : TEXCOORD0;
		};
	
		struct v2fd {
			float4 pos : SV_POSITION;
			float2 uv[2] : TEXCOORD0;
		};

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		uniform float4 _MainTex_TexelSize;
		half4 _MainTex_ST;

        TEXTURE2D_SAMPLER2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture);
		half4 _CameraDepthNormalsTexture_ST;

        TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
		half4 _CameraDepthTexture_ST;

		uniform half4 _Sensitivity; 
		uniform half4 _BgColor;
		uniform half _BgFade;
		uniform half _SampleDistance;
		uniform float _Exponent;
		
		uniform float _Threshold;
		
		inline float DecodeFloatRG( float2 enc ) {
			float2 kDecodeDot = float2(1.0, 1/255.0);
			return dot( enc, kDecodeDot );
		}
	
		inline half CheckSame (half2 centerNormal, float centerDepth, half4 theSample) {
			// difference in normals
			// do not bother decoding normals - there's no need here
			half2 diff = abs(centerNormal - theSample.xy) * _Sensitivity.y;
			int isSameNormal = (diff.x + diff.y) * _Sensitivity.y < 0.1;
			// difference in depth
			float sampleDepth = DecodeFloatRG (theSample.zw);
			float zdiff = abs(centerDepth-sampleDepth);
			// scale the required threshold by the distance
			int isSameDepth = zdiff * _Sensitivity.x < 0.09 * centerDepth;
	
			// return:
			// 1 - if normals and depth are similar enough
			// 0 - otherwise
		
			return isSameNormal * isSameDepth ? 1.0 : 0.0;
		}	
	
		// THIN
		v2f vertThin( AttributesDefault v ) {
			v2f o;
			o.pos = float4(v.vertex.xy, 0.0, 1.0);
		
			float2 uv = TransformTriangleVertexToUV(v.vertex.xy);
			#if UNITY_UV_STARTS_AT_TOP
			uv = uv * float2(1.0,-1.0) + float2(0.0, 1.0);
			#endif

			o.uv[0] = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);
		
			o.uv[1] = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);
			o.uv[4] = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);
				
			// offsets for two additional samples
			o.uv[2] = UnityStereoScreenSpaceUVAdjust(uv + float2(-_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * _SampleDistance, _MainTex_ST);
			o.uv[3] = UnityStereoScreenSpaceUVAdjust(uv + float2(+_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * _SampleDistance, _MainTex_ST);
		
			return o;
		}	  
	
		half4 fragThin (v2f i) : SV_Target {
			half4 original = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv[0]);
		
			half4 center = SAMPLE_TEXTURE2D (_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[1]);
			half4 sample1 = SAMPLE_TEXTURE2D (_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[2]);
			half4 sample2 = SAMPLE_TEXTURE2D (_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[3]);
		
			// encoded normal
			half2 centerNormal = center.xy;
			// decoded depth
			float centerDepth = DecodeFloatRG (center.zw);

			half edge = 1.0;
		
			edge *= CheckSame(centerNormal, centerDepth, sample1);
			edge *= CheckSame(centerNormal, centerDepth, sample2);
			
			return edge * lerp(original, _BgColor, _BgFade);
		}

		struct v2flum {
			float4 pos : SV_POSITION;
			float2 uv[3] : TEXCOORD0;
		};

		// ROBERT
		v2f vertRobert( AttributesDefault v ) {
			v2f o;
			o.pos = float4(v.vertex.xy, 0.0, 1.0);
		
			float2 uv = TransformTriangleVertexToUV(v.vertex.xy);
			#if UNITY_UV_STARTS_AT_TOP
			uv = uv * float2(1.0,-1.0) + float2(0.0, 1.0);
			#endif
				
			o.uv[0] = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);

			// calc coord for the X pattern
			// maybe nicer TODO for the future: 'rotated triangles'
		
			o.uv[1] = UnityStereoScreenSpaceUVAdjust(uv + _MainTex_TexelSize.xy * half2(1,1) * _SampleDistance, _MainTex_ST);
			o.uv[2] = UnityStereoScreenSpaceUVAdjust(uv + _MainTex_TexelSize.xy * half2(-1,-1) * _SampleDistance, _MainTex_ST);
			o.uv[3] = UnityStereoScreenSpaceUVAdjust(uv + _MainTex_TexelSize.xy * half2(-1,1) * _SampleDistance, _MainTex_ST);
			o.uv[4] = UnityStereoScreenSpaceUVAdjust(uv + _MainTex_TexelSize.xy * half2(1,-1) * _SampleDistance, _MainTex_ST);
				 
			return o;
		} 

		half4 fragRobert(v2f i) : SV_Target {				
			half4 sample1 = SAMPLE_TEXTURE2D (_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[1]);
			half4 sample2 = SAMPLE_TEXTURE2D (_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[2]);
			half4 sample3 = SAMPLE_TEXTURE2D (_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[3]);
			half4 sample4 = SAMPLE_TEXTURE2D (_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.uv[4]);

			half edge = 1.0;
		
			edge *= CheckSame(sample1.xy, DecodeFloatRG(sample1.zw), sample2);
			edge *= CheckSame(sample3.xy, DecodeFloatRG(sample3.zw), sample4);

			return edge * lerp( SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv[0]), _BgColor, _BgFade);
		}

		// SOBEL
	 
		v2fd vertD( AttributesDefault v ) {
			v2fd o;
			o.pos = float4(v.vertex.xy, 0.0, 1.0);
		
			float2 uv = TransformTriangleVertexToUV(v.vertex.xy);

			#if UNITY_UV_STARTS_AT_TOP
			uv = uv * float2(1.0,-1.0) + float2(0.0, 1.0);
			#endif
	
			o.uv[0] = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);

			o.uv[1] = uv;
		
			return o;
		}

		float4 fragDCheap(v2fd i) : SV_Target {	
			// inspired by borderlands implementation of popular "sobel filter"

			float centerDepth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.uv[1]));
			float4 depthsDiag;
			float4 depthsAxis;

			float2 uvDist = _SampleDistance * _MainTex_TexelSize.xy;

			depthsDiag.x = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]+uvDist, _CameraDepthTexture_ST))); // TR
			depthsDiag.z = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]-uvDist*float2(-1,1), _CameraDepthTexture_ST))); // BR
			depthsDiag.y = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]+uvDist*float2(-1,1), _CameraDepthTexture_ST))); // TL
			depthsDiag.w = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]-uvDist, _CameraDepthTexture_ST))); // BL

			depthsAxis.x = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]+uvDist*float2(0,1), _CameraDepthTexture_ST))); // T
			depthsAxis.y = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]-uvDist*float2(1,0), _CameraDepthTexture_ST))); // L
			depthsAxis.z = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]+uvDist*float2(1,0), _CameraDepthTexture_ST))); // R
			depthsAxis.w = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]-uvDist*float2(0,1), _CameraDepthTexture_ST))); // B

			depthsDiag -= centerDepth;
			depthsAxis /= centerDepth;

			const float4 HorizDiagCoeff = float4(1,1,-1,-1);
			const float4 VertDiagCoeff = float4(-1,1,-1,1);
			const float4 HorizAxisCoeff = float4(1,0,0,-1);
			const float4 VertAxisCoeff = float4(0,1,-1,0);

			float4 SobelH = depthsDiag * HorizDiagCoeff + depthsAxis * HorizAxisCoeff;
			float4 SobelV = depthsDiag * VertDiagCoeff + depthsAxis * VertAxisCoeff;

			float SobelX = dot(SobelH, float4(1,1,1,1));
			float SobelY = dot(SobelV, float4(1,1,1,1));
			float Sobel = sqrt(SobelX * SobelX + SobelY * SobelY);

			Sobel = 1.0-pow(saturate(Sobel), _Exponent);
			return Sobel * lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv[0]), _BgColor, _BgFade);
		}
		
		// pretty much also just a sobel filter, except for that edges "outside" the silhouette get discarded
		//  which makes it compatible with other depth based post fx
		float4 fragD(v2fd i) : SV_Target {	
			// inspired by borderlands implementation of popular "sobel filter"

			float centerDepth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1], _CameraDepthTexture_ST)));
			float4 depthsDiag;
			float4 depthsAxis;

			float2 uvDist = _SampleDistance * _MainTex_TexelSize.xy;

			depthsDiag.x = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]+uvDist, _CameraDepthTexture_ST))); // TR
			depthsDiag.y = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]+uvDist*float2(-1,1), _CameraDepthTexture_ST))); // TL
			depthsDiag.z = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]-uvDist*float2(-1,1), _CameraDepthTexture_ST))); // BR
			depthsDiag.w = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]-uvDist, _CameraDepthTexture_ST))); // BL

			depthsAxis.x = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]+uvDist*float2(0,1), _CameraDepthTexture_ST))); // T
			depthsAxis.y = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]-uvDist*float2(1,0), _CameraDepthTexture_ST))); // L
			depthsAxis.z = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]+uvDist*float2(1,0), _CameraDepthTexture_ST))); // R
			depthsAxis.w = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv[1]-uvDist*float2(0,1), _CameraDepthTexture_ST))); // B

			// make it work nicely with depth based image effects such as depth of field:
			depthsDiag = (depthsDiag > centerDepth.xxxx) ? depthsDiag : centerDepth.xxxx;
			depthsAxis = (depthsAxis > centerDepth.xxxx) ? depthsAxis : centerDepth.xxxx;

			depthsDiag -= centerDepth;
			depthsAxis /= centerDepth;

			const float4 HorizDiagCoeff = float4(1,1,-1,-1);
			const float4 VertDiagCoeff = float4(-1,1,-1,1);
			const float4 HorizAxisCoeff = float4(1,0,0,-1);
			const float4 VertAxisCoeff = float4(0,1,-1,0);

			float4 SobelH = depthsDiag * HorizDiagCoeff + depthsAxis * HorizAxisCoeff;
			float4 SobelV = depthsDiag * VertDiagCoeff + depthsAxis * VertAxisCoeff;

			float SobelX = dot(SobelH, float4(1,1,1,1));
			float SobelY = dot(SobelV, float4(1,1,1,1));
			float Sobel = sqrt(SobelX * SobelX + SobelY * SobelY);

			Sobel = 1.0-pow(saturate(Sobel), _Exponent);
			return Sobel * lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv[0]), _BgColor, _BgFade);
		}

		// LUM
		v2flum vertLum (AttributesDefault v) {
			v2flum o;
			o.pos = float4(v.vertex.xy, 0.0, 1.0);
			float2 uv = TransformTriangleVertexToUV(v.vertex.xy);
		
			#if UNITY_UV_STARTS_AT_TOP
			uv = uv * float2(1.0,-1.0) + float2(0.0, 1.0);
			#endif

			o.uv[0] = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);
			o.uv[1] = UnityStereoScreenSpaceUVAdjust(uv + float2(-_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * _SampleDistance, _MainTex_ST);
			o.uv[2] = UnityStereoScreenSpaceUVAdjust(uv + float2(+_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * _SampleDistance, _MainTex_ST);
			return o;
		}


		float4 fragLum (v2flum i) : SV_Target {
			float4 original = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv[0]);

			// a very simple cross gradient filter

			half3 p1 = original.rgb;
			half3 p2 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv[1]).rgb;
			half3 p3 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv[2]).rgb;
		
			half3 diff = p1 * 2 - p2 - p3;
			half len = dot(diff, diff);
			len = step(len, _Threshold);
			//if(len >= _Threshold)
			//	original.rgb = 0;

			return len * lerp(original, _BgColor, _BgFade);			
		}

    ENDHLSL

Subshader {
 Pass {
	  ZTest Always Cull Off ZWrite Off

      HLSLPROGRAM
      #pragma vertex vertThin
      #pragma fragment fragThin
      ENDHLSL
  }
 Pass {
	  ZTest Always Cull Off ZWrite Off

      HLSLPROGRAM
      #pragma vertex vertRobert
      #pragma fragment fragRobert
      ENDHLSL
  }

 Pass {
	  ZTest Always Cull Off ZWrite Off

      HLSLPROGRAM
	  #pragma target 3.0   
      #pragma vertex vertD
      #pragma fragment fragDCheap
      ENDHLSL
  }
 Pass {
	  ZTest Always Cull Off ZWrite Off

      HLSLPROGRAM
	  #pragma target 3.0   
      #pragma vertex vertD
      #pragma fragment fragD
      ENDHLSL
  }
 Pass {
	  ZTest Always Cull Off ZWrite Off

      HLSLPROGRAM
      #pragma vertex vertLum
      #pragma fragment fragLum
      ENDHLSL
  }
}

Fallback off
	
} // shader