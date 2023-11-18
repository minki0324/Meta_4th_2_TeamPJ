Shader "SkyBox/BlendTwoSkybox"
{
    Properties
    {
        [NoScaleOffset] _Texture("Texture",2D) = "grey"{}
        [NoScaleOffset] _Texture2("Texture2",2D) = "grey"{}
        // _LerpTexture("LerpTexture", 2D) = "white"{}
         _LerpControl("LerpControl", Range(0,1)) = 0
             // [Gamma]  _Exposure("Exposure", Range(0.000000,8.000000)) = 1.000000

    }

        SubShader
         {
             Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

             Pass
             {
                 //HLSLPROGRAM
                 CGPROGRAM
                 #pragma vertex vert
                 #pragma fragment frag

                // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                  #include "UnityCG.cginc"
                // //텍스쳐로 변환
                //TEXTURE2D(_Texture);
                //TEXTURE2D(_Texture2);
         

                ////sampler_Texture 변수를 Sampler2D로 선언
                //SAMPLER(sampler_Texture);
                //SAMPLER(sampler_Texture2);

             sampler2D _Texture;
             sampler2D _Texture2;
            // float4 _Textrue_TexelSize;
             half4 _Texture_HDR;
            /* texture2D _Texture;
             texture2D _Texture2;*/

                CBUFFER_START(UnityPerMaterial)
                    float4 _Texture_ST;
                    float4 _Texture2_ST;
                    

                    float4 _finalTexture;
                    float _LerpControl;
                CBUFFER_END

                    float3 RotateAroundYInDegrees(float3 vertex, float degrees)
                {
                    float alpha = degrees * UNITY_PI / 180.0;
                    float sina, cosa;
                    sincos(alpha, sina, cosa);
                    float2x2 m = float2x2(cosa, -sina, sina, cosa);
                    return float3(mul(m, vertex.xz), vertex.y).xzy;
                }

                    #define M_PI 3.141592653589793
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
              UNITY_VERTEX_OUTPUT_STEREO
         };


      



                //정점계산
                Varyings vert(Attributes IN)
                {

                    Varyings OUT;
                  //  OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                    float3 rotated = RotateAroundYInDegrees(IN.positionOS, 0);
                    OUT.uv = UnityObjectToClipPos(rotated);
                   
                    OUT.uv = IN.positionOS.xyz;
                    
                    OUT.image180ScaleAndCutoff = float2(1.0, 1.0); 
                    // Calculate constant scale and offset for 3D layouts
                    
                    OUT.layout3DScaleAndOffset = float4(0, 0, 1, 1);
                 

          
                    UNITY_INITIALIZE_OUTPUT(Varyings, OUT);
                    return OUT;
                 }



                 //픽셀마다 색상계산
                 half4 frag(Varyings IN) : SV_Target
                 {
   
                     //tex2D(sampler_Texture3, equiUV); //float4 형

                     //half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv); //매크로
                     //half4 color = _BaseMap.Sample(sampler_BaseMap, IN.uv); //실제

                     /*float4 colorA = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, TRANSFORM_TEX(IN.uv, _Texture));
                     float4 colorB = SAMPLE_TEXTURE2D(_Texture2, sampler_Texture2, TRANSFORM_TEX(IN.uv, _Texture2));*/
                   
                   float4 col_B = tex2D(_Texture, IN.uv);
                   float4 col_A = tex2D(_Texture2, IN.uv);
                     
                    

                     //finalColor 변수를 만들어서 , 두 텍스쳐를 lerp 한 값을 넣는다. 
                     // 맨 뒤 인자가 0일경우 colorA 가 나오고, 1일 경우 colorB가 나오고 0.5 인 경우 섞임
                     //float4 finalColor;
                     //finalColor = lerp(colorA, colorB, _LerpControl);
                     
                     float2 tc = ToRadialCoords(IN.uv);
                     if (tc.x > IN.image180ScaleAndCutoff[1])
                         return half4(0, 0, 0, 1);

                     tc.x = fmod(tc.x * IN.image180ScaleAndCutoff[0], 1);
                     tc = (tc + IN.layout3DScaleAndOffset.xy) * IN.layout3DScaleAndOffset.zw;
 
                   
                     float4 tex = tex2D(_Texture, tc);
                     float4 tex2 = tex2D(_Texture2, tc);
                     
                     float4 final_C = lerp(col_A, col_B, _LerpControl);

                     half3 c = DecodeHDR(final_C, _Texture_HDR);
                     c = c * unity_ColorSpaceDouble.rgb;
                     c *= 1;

                     return half4(c, 1);
                    //return final_C;


                    //SAMPLER()로 
//SAMPLE_TEXTURE2D로 샘플러 변수에 텍스쳐 넣기

//finalColor(float4)를 Sampler_Textrue3(Texture2D)의 색상 값으로 넣고 tex2D()호출


                }



                ENDCG
                //ENDHLSL
            }
         }

             Fallback Off
}