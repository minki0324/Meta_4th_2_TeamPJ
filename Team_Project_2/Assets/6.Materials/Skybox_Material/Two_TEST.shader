// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Skybox/mon_skybox1" {
    Properties{

        [NoScaleOffset] _Texture("Texture",2D) = "grey"{}
        [NoScaleOffset] _Texture2("Texture2",2D) = "grey"{}
        // _LerpTexture("LerpTexture", 2D) = "white"{}
         _LerpControl("LerpControl", Range(0,1)) = 0
    }

        SubShader{
            Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
            Cull Off ZWrite Off

            Pass {

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
                #include "Lighting.cginc"

                

                   sampler2D _Texture;
                   float4 _Texture_TexelSize;
                   half4 _Texture_HDR;

                   sampler2D _Texture2;
                   float4 _Texture2_TexelSize;
                   half4 _Texture2_HDR;
                   float4 _finalTexture;
                   
                   float _LerpControl;

  




              half _stars;
              half _stars_int;


              // Calculates the Rayleigh phase function
              half getRayleighPhase(half eyeCos2)
              {
                  return 0.75 + 0.75 * eyeCos2;
              }
              half getRayleighPhase(half3 light, half3 ray)
              {
                  half eyeCos = dot(light, ray);
                  return getRayleighPhase(eyeCos * eyeCos);
              }


              struct appdata_t
              {
                  float4 vertex : POSITION;
                  float3 texcoord :TEXCOORD0;
                  UNITY_VERTEX_INPUT_INSTANCE_ID
              };

              struct v2f
              {
                  float4  pos             : SV_POSITION;

                  // calculate sky colors in vprog

           
                  half3 uv : TEXCOORD4;

                  UNITY_VERTEX_OUTPUT_STEREO
              };




              v2f vert(appdata_t v)
              {
                  v2f OUT;
                  UNITY_SETUP_INSTANCE_ID(v);
                  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                  OUT.pos = UnityObjectToClipPos(v.vertex);
                  OUT.uv = v.texcoord.xyz;
               
                  return OUT;

                 
                     
                              }




               half4 frag(v2f IN) : SV_Target
               {
                    float4 col_B = tex2D(_Texture, IN.uv);
                    float4 col_A = tex2D(_Texture2, IN.uv);
                    float4 final_C = lerp(col_A, col_B, _LerpControl);

                   return final_C;
                       }
                       ENDCG
                   }
    }


        Fallback Off
                           CustomEditor "SkyboxProceduralShaderGUI"
}