Shader "Codercat/URP_Test"
{
	Properties
	{
		_MainTex("RGB 01", 2D) = "white" {}
		_MainTex2("RGB 01", 2D) = "white" {}
		_lerpTexture("lerp", Range(0,1)) = 0

		 //_레퍼런스 이름 ("유니티 프로퍼티에서 보여지는 이름", 자료형) = "자료형에 맞는 기본 값"{}
	}

	SubShader
	{

			Tags
			{
				"RenderPipeline" = "UniversalPipeline"
				"RenderType" = "Opaque" // 렌더 타입이 불투명이라는 선언
										//불투명 = "Opaque", 알파 클립 "TransparentCutout"
										//반투명 = "Transparent", 배경(스카이박스) = "Background"
										//오버레이(GUI 텍스쳐, 후광 등 효과) = "Overlay"
				"Queue" = "Geometry"	//배경 = "Background", 불투명 = "Geometry", 알파 클립 = "AlphaTest", 반투명 = "Transparent", 오버레이 = "Overlay"
			}

			LOD 100		//Subshader로 여러 사양의 쉐이더를 만들고,
						//그 안의 LOD 값을 이용해 낮은 성능의 기기에서 낮은 사양의 쉐이더가
						//돌아갈 수 있도록 조절할 수 있다고 한다. 낮은 쉐이더 성능으로 돌리려면
						//스크립트에서 Maximum LOD 수치를 따로 정해줘야한다고 한다.


				Pass
				{
				 Name "Universal Forward"
					  Tags { "LightMode" = "UniversalForward" }

				HLSLPROGRAM
			#pragma prefer_hlslcc gles
					#pragma exclude_renderers d3d11_9x
					#pragma vertex vert
					#pragma fragment frag

					#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"        	

					struct VertexInput
					{
						float4 vertex : POSITION;
						float2 uv 	: TEXCOORD0;
					};

					struct VertexOutput
					{
					float4 vertex  	: SV_POSITION;
					float2 uv 	: TEXCOORD0;
					};

					Texture2D _MainTex;
					float4 _MainTex_ST;
					SamplerState sampler_MainTex;

					Texture2D _MainTex2;
					float4 _MainTex_ST2;
					SamplerState sampler_MainTex2;

					float _lerpTexture;


					VertexOutput vert(VertexInput v)
					{

						VertexOutput o;
						o.vertex = TransformObjectToHClip(v.vertex.xyz);
						o.uv = v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
						return o;
					}


					half4 frag(VertexOutput i) : SV_Target
					{

					float4 _tex = _MainTex.Sample(sampler_MainTex, i.uv);
					float4 _tex2 = _MainTex2.Sample(sampler_MainTex2, i.uv);
					i.RGB = lerp(_tex.rgb, _tex2.rgb, _lerpTexture);
					return _tex;
					}




					ENDHLSL
				}
	}
}


