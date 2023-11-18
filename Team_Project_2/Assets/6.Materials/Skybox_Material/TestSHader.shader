Shader "SkyBox/TestSHader
{
    Properties
    {   

        _Texture("Texture",2D) = "white"{}
        _Texture2("Texture2",2D) = "white"{}
        // _LerpTexture("LerpTexture", 2D) = "white"{}
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
                 // Transformations : CG -> HLSL Compatability
          #define UnityObjectToClipPos(x)     TransformObjectToHClip(x)
          #define UnityObjectToWorldDir(x)    TransformObjectToWorldDir(x)
          #define UnityObjectToWorldNormal(x) TransformObjectToWorldNormal(x)

          #define UnityWorldToViewPos(x)      TransformWorldToView(x)
          #define UnityWorldToObjectDir(x)    TransformWorldToObjectDir(x)
          #define UnityWorldSpaceViewDir(x)   _WorldSpaceCameraPos.xyz - x
    
                 
             //텍스쳐 샘플링======
                TEXTURE2D(_Texture);
                TEXTURE2D(_Texture2);

                SAMPLER(sampler_Texture);
                SAMPLER(sampler_Texture2);
               

                CBUFFER_START(UnityPerMaterial)
                    float4 _Texture_ST;
                    float4 _Texture2_ST;
             //=====================
                    half4 _Tex_HDR;
                    float4 _finalTexture;

                    float _LerpControl;
                CBUFFER_END

#define M_PI 3.141592653589793

                float3 RotateAroundYInDegrees(float3 vertex, float degrees)
                {
                    float alpha = degrees * M_PI / 180.0;
                    float sina, cosa;
                    sincos(alpha, sina, cosa);
                    float2x2 m = float2x2(cosa, -sina, sina, cosa);
                    return float3(mul(m, vertex.xz), vertex.y).xzy;
                }


                
                   inline float2 ToRadialCoords(float3 coords)
                   {
                        float3 normalizedCoords = normalize(coords);
                        float latitude = acos(normalizedCoords.y);
                        float longitude = atan2(normalizedCoords.z, normalizedCoords.x);
                        float2 sphereCoords = float2(longitude, latitude) * float2(0.5 / M_PI, 1.0 / M_PI);
                        return float2(0.5, 1.0) - sphereCoords;
                   }



         // Attributes 구조체를 버텍스 셰이더의 인풋 구조체로 사용
         struct Attributes
         {
             float4 positionOS   : POSITION;
             //float2 uv           : TEXCOORD0;
             UNITY_VERTEX_INPUT_INSTANCE_ID
             // 쉐이더에서 여러 개의 텍스처를 동시에 사용할 때, 둘 이상의 UV 좌표를 사용할 경우가 있는데 그럴 때에는 TEXCOORD0, TEXCOORD1등으로 시맨틱을 사용
             //유니티는 왼쪽 아래가 0,0 (언리얼은 왼쪽위)
         };

         //프래그먼트 셰이더 인풋 구조체
     //이 구조체의 포지션 변수는 반드시 SV_POSITION 시멘틱을 가지고 있어야 함
         struct Varyings
         {
              float4 positionHCS  : SV_POSITION;
              float3 uv           : TEXCOORD0;

              float2 image180ScaleAndCutoff : TEXCOORD1;
              float4 layout3DScaleAndOffset : TEXCOORD2;
         };






                Varyings vert(Attributes IN)
                {

                    Varyings OUT;

                    float3 rotated = RotateAroundYInDegrees(IN.positionOS, 0);
                    //OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                    OUT.positionHCS = UnityObjectToClipPos(rotated);
                    OUT.uv = IN.positionOS.xyz;

                    OUT.image180ScaleAndCutoff = float2(1.0, 1.0);
                    OUT.layout3DScaleAndOffset = float4(0, 0, 1, 1);
                     return OUT;

                   
                 }




                 half4 frag(Varyings IN) : SV_Target
                 {



                     //float2 equiUV = ToRadialCoords(IN.normal);
                     //tex2D(sampler_Texture3, equiUV); //float4 형

                     //half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv); //매크로
                     //half4 color = _BaseMap.Sample(sampler_BaseMap, IN.uv); //실제

                     float4 colorA = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, TRANSFORM_TEX(IN.uv, _Texture));
                     float4 colorB = SAMPLE_TEXTURE2D(_Texture2, sampler_Texture2, TRANSFORM_TEX(IN.uv, _Texture2));
       
                     float4 finalColor;

                     finalColor = lerp(colorA, colorB, _LerpControl);

                     tc.x = fmod(tc.x * IN.image180ScaleAndCutoff[0], 1);
                     tc = (tc + IN.layout3DScaleAndOffset.xy) * IN.layout3DScaleAndOffset.zw;


                     half3 c = DecodeHDR(finalColor, _Tex_HDR);

                    return half4(c,1);

                }




                ENDHLSL
            }
         
         }
             Fallback Off

}