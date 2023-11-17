//Shader "SkyBox/asdf"
//{
//    Properties
//    {
//        _Texture("Texture",2D) = "white"{}
//        _Texture2("Texture2",2D) = "white"{}
//        // _LerpTexture("LerpTexture", 2D) = "white"{}
//         _LerpControl("LerpControl", Range(0,1)) = 0
//             // [Gamma]  _Exposure("Exposure", Range(0.000000,8.000000)) = 1.000000
//
//    }
//
//        SubShader
//         {
//             Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
//
//             Pass
//             {
//                 HLSLPROGRAM
//                 #pragma vertex vert
//                 #pragma fragment frag
//
//                 #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
//
//
//
//
//                    #define M_PI 3.141592653589793
//                   inline float2 ToRadialCoords(float3 coords)
//                   {
//                        float3 normalizedCoords = normalize(coords);
//                        float latitude = acos(normalizedCoords.y);
//                        float longitude = atan2(normalizedCoords.z, normalizedCoords.x);
//                        float2 sphereCoords = float2(longitude, latitude) * float2(0.5 / M_PI, 1.0 / M_PI);
//                        return float2(0.5, 1.0) - sphereCoords;
//                   }
//
//
//
//         // Attributes 구조체를 버텍스 셰이더의 인풋 구조체로 사용
//         struct Attributes
//         {
//             float4 positionOS   : POSITION;
//             float2 uv           : TEXCOORD0;
//
//             // 쉐이더에서 여러 개의 텍스처를 동시에 사용할 때, 둘 이상의 UV 좌표를 사용할 경우가 있는데 그럴 때에는 TEXCOORD0, TEXCOORD1등으로 시맨틱을 사용
//             //유니티는 왼쪽 아래가 0,0 (언리얼은 왼쪽위)
//         };
//
//         //프래그먼트 셰이더 인풋 구조체
//     //이 구조체의 포지션 변수는 반드시 SV_POSITION 시멘틱을 가지고 있어야 함
//         struct Varyings
//         {
//              float4 positionHCS  : SV_POSITION;
//              float2 uv           : TEXCOORD0;
//
//         };
//
//
//            TEXTURE2D(_Texture);
//            TEXTURE2D(_Texture2);
//            //  TEXTURE2D(_Texture3);
//
//              SAMPLER(sampler_Texture);
//              SAMPLER(sampler_Texture2);
//              //  SAMPLER(sampler_Texture3);
//
//                CBUFFER_START(UnityPerMaterial)
//                    float4 _Texture_ST;
//                    float4 _Texture2_ST;
//                    //  float4 _Texture3_ST;
//
//                    float4 _finalTexture;
//
//                    float _LerpControl;
//                CBUFFER_END
//
//
//
//
//                //정점쉐이더Shader "Example/URPUnlitShaderBasic"
//                {
//                    Properties
//                    {
//                        _BaseMap("BaseMap",2D) = "white"{}
//                        _BaseMap2("BaseMap2",2D) = "white"{}
//                        _LerpControlTex("LerpControlTexure",2D) = "white"{}
//                        //사라지기
//                        _Fade("사라지기", Range(-1,1)) = 0
//
//                    }
//
//                    SubShader
//                    {
//                        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
//
//                        Pass
//                        {
//                            HLSLPROGRAM
//                            #pragma vertex vert
//                            #pragma fragment frag
//
//                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
//
//                            struct Attributes
//                            {
//                                float4 positionOS   : POSITION;
//                                float2 uv           : TEXCOORD0;
//                            };
//
//                            struct Varyings
//                            {
//                                float4 positionHCS  : SV_POSITION;
//                                float2 uv           : TEXCOORD0;
//                            };
//
//                            TEXTURE2D(_BaseMap);
//                            TEXTURE2D(_BaseMap2);
//                            TEXTURE2D(_LerpControlTex);
//                            SAMPLER(sampler_BaseMap);
//                            SAMPLER(sampler_BaseMap2);
//                            SAMPLER(sampler_LerpControlTex);
//
//                            CBUFFER_START(UnityPerMaterial)
//                                float4 _BaseMap_ST;
//                                float4 _BaseMap2_ST;
//                                float4 _LerpControlTex_ST;
//                                float _Fade; //사라지기
//                            CBUFFER_END
//
//                            Varyings vert(Attributes IN)
//                            {
//                                Varyings OUT;
//                                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
//                                OUT.uv = IN.uv;
//                                return OUT;
//                            }
//
//                            half4 frag(Varyings IN) : SV_Target
//                            {
//                                half4 colorA = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, TRANSFORM_TEX(IN.uv, _BaseMap));
//                                half4 colorB = SAMPLE_TEXTURE2D(_BaseMap2, sampler_BaseMap2, TRANSFORM_TEX(IN.uv, _BaseMap2));
//                                half4 LerpControlTex = SAMPLE_TEXTURE2D(_LerpControlTex, sampler_LerpControlTex, TRANSFORM_TEX(IN.uv, _LerpControlTex));
//
//                                half4 finalColor;
//                                finalColor = lerp(colorA, colorB, saturate(LerpControlTex.r + _Fade)); //더해주고 saturate로 0과 1 사이를 못 벗어나게 강제로 만든다
//
//                                return finalColor;
//                            }
//                            ENDHLSL
//                        }
//                    }
//                }
//                Varyings vert(Attributes IN)
//                {
//
//                    Varyings OUT;
//                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
//                    OUT.uv = IN.uv;
//                    // OUT.uv = TRANSFORM_TEX(IN.uv, _Texture);
//                     return OUT;
//
//                     //Varyings OUT;
//                     //UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
//
//                     ////버텍스 포지션을 클립스페이스로 변환
//                     //OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
//                     //OUT.uv = IN.uv;
//                     //return OUT;
//                 }
//
//
//
//                 //색상 쉐이더
//                 half4 frag(Varyings IN) : SV_Target
//                 {
//
//
//
//                     //float2 equiUV = ToRadialCoords(IN.normal);
//                     //tex2D(sampler_Texture3, equiUV); //float4 형
//
//                     //half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv); //매크로
//                     //half4 color = _BaseMap.Sample(sampler_BaseMap, IN.uv); //실제
//
//                     float4 colorA = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, TRANSFORM_TEX(IN.uv, _Texture));
//                     float4 colorB = SAMPLE_TEXTURE2D(_Texture2, sampler_Texture2, TRANSFORM_TEX(IN.uv, _Texture2));
//                     //finalColor 변수를 만들어서 , 두 텍스쳐를 lerp 한 값을 넣는다. 
//                     // 맨 뒤 인자가 0일경우 colorA 가 나오고, 1일 경우 colorB가 나오고 0.5 인 경우 섞임
//                     float4 finalColor;
//
//                     finalColor = lerp(colorA, colorB, _LerpControl);
//                     //SAMPLER()로 
//                    //SAMPLE_TEXTURE2D로 샘플러 변수에 텍스쳐 넣기
//
//                //finalColor(float4)를 Sampler_Textrue3(Texture2D)의 색상 값으로 넣고 tex2D()호출
//
//                    return finalColor;
//
//                }
//
//
//
//
//                ENDHLSL
//            }
//         }
//
//             Fallback Off
//}