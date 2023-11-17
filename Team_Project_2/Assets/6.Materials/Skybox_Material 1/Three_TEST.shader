Shader "SkyBox/Three_TEST"
{
    Properties
    {
        _BaseMap("BaseMap",2D) = "white"{}
        _BaseMap2("BaseMap2",2D) = "white"{}
        _LerpControlTex("LerpControlTexure",2D) = "white"{}
        //사라지기
        _Fade("Fade", Range(-1,1)) = 0

    }

        SubShader
        {
            Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" "RenderPipeline" = "UniversalPipeline" }

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
                TEXTURE2D(_LerpControlTex);
                SAMPLER(sampler_BaseMap);
                SAMPLER(sampler_BaseMap2);
                SAMPLER(sampler_LerpControlTex);

                CBUFFER_START(UnityPerMaterial)
                    float4 _BaseMap_ST;
                    float4 _BaseMap2_ST;
                    float4 _LerpControlTex_ST;
                    float _Fade; //사라지기
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
                    half4 LerpControlTex = SAMPLE_TEXTURE2D(_LerpControlTex, sampler_LerpControlTex, TRANSFORM_TEX(IN.uv, _LerpControlTex));

                    half4 finalColor;
                    finalColor = lerp(colorA, colorB, saturate(LerpControlTex.r + _Fade)); //더해주고 saturate로 0과 1 사이를 못 벗어나게 강제로 만든다

                    return finalColor;
                }
                ENDHLSL
            }
        }
}