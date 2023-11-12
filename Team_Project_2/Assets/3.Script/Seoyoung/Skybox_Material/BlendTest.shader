Shader "Example/URPUnlitShaderBasic"
{
    Properties
    {
        _BaseMap("BaseMap",2D) = "white"{}
        _BaseMap2("BaseMap2",2D) = "white"{}
         _LerpControl("LerpControl", Range(0,1)) = 0

    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            TEXTURE2D(_BaseMap2);
            SAMPLER(sampler_BaseMap);
            SAMPLER(sampler_BaseMap2);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseMap2_ST;
                float _LerpControl;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 colorA = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, TRANSFORM_TEX(IN.uv, _BaseMap));
                half4 colorB = SAMPLE_TEXTURE2D(_BaseMap2, sampler_BaseMap2, TRANSFORM_TEX(IN.uv, _BaseMap2));
                //finalColor 변수를 만들어서 , 두 텍스쳐를 lerp 한 값을 넣는다. 
                // 맨 뒤 인자가 0일경우 colorA 가 나오고, 1일 경우 colorB가 나오고 0.5 인 경우 섞인다. 
                half4 finalColor;
                finalColor = lerp(colorA, colorB, _LerpControl);

                return finalColor;
           
                
            }
            ENDHLSL
        }
    }
}
//출처: https://chulin28ho.tistory.com/671