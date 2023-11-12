Shader "SkyBox/BlendTwoSkybox"
{
    Properties
    {
        _Texture("Texture",2D) = "white"{}
        _Texture2("Texture2",2D) = "white"{}
        _LerpControl("LerpControl", Range(0,1)) = 0
        [Gamma]  _Exposure("Exposure", Range(0.000000,8.000000)) = 1.000000

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
                     // 쉐이더에서 여러 개의 텍스처를 동시에 사용할 때, 둘 이상의 UV 좌표를 사용할 경우가 있는데 그럴 때에는 TEXCOORD0, TEXCOORD1등으로 시맨틱을 사용
                     //유니티는 왼쪽 아래가 0,0 (언리얼은 왼쪽위)
                 };

                 struct Varyings
                 {
                     float4 positionHCS  : SV_POSITION;
                     float2 uv           : TEXCOORD0;
                 };

                 TEXTURE2D(_Texture);
                 TEXTURE2D(_Texture2);
                 SAMPLER(sampler_Texture);
                 SAMPLER(sampler_Texture2);

                 CBUFFER_START(UnityPerMaterial)
                     float4 _Texture_ST;
                     float4 _Texture2_ST;
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
                     half4 colorA = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, TRANSFORM_TEX(IN.uv, _Texture));
                     half4 colorB = SAMPLE_TEXTURE2D(_Texture2, sampler_Texture2, TRANSFORM_TEX(IN.uv, _Texture2));
                     //finalColor 변수를 만들어서 , 두 텍스쳐를 lerp 한 값을 넣는다. 
                     // 맨 뒤 인자가 0일경우 colorA 가 나오고, 1일 경우 colorB가 나오고 0.5 인 경우 섞인다. 
                     half4 finalColor;
                     finalColor = lerp(colorA, colorB, _LerpControl);

                     return finalColor;


                 }
                 ENDHLSL
             }
         }
            
         Fallback Off
}
