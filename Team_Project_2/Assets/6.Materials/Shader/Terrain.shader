Shader "Custom/Terrain" {
    Properties{
        _BaseMap("Base Map", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _MinHeight("Minimum Height", Range(0, 1)) = 0
        _MaxHeight("Maximum Height", Range(0, 1)) = 1
        _BaseColourCount("Base Colour Count", Int) = 1
        _BaseColour0("Base Colour 0", Color) = (1, 0, 0, 1)
        _BaseStartHeight0("Base Start Height 0", Range(0, 1)) = 0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass {
                Name "ForwardLit"
                Tags { "LightMode" = "UniversalForward" }
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                TEXTURE2D(_BaseMap);
                SAMPLER(sampler_BaseMap);
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


                struct Attributes {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings {
                    float4 pos : POSITION;
                    float3 normal : NORMAL;
                    float3 worldPos : TEXCOORD0;
                    float2 uv : TEXCOORD1;
                };

                CBUFFER_START(UnityPerMaterial)
                float _MinHeight;
                float _MaxHeight;
                float _BaseStartHeight0;
                float4 _BaseColor;
                float4 _BaseColour0;
                CBUFFER_END

                    Varyings vert(Attributes input) {
                    Varyings output;
                    UNITY_SETUP_INSTANCE_ID(input);
                    UNITY_TRANSFER_INSTANCE_ID(input, output);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                    output.uv = input.uv;
                    output.pos = TransformObjectToHClip(input.vertex.xyz);
                    output.normal = TransformObjectToWorldNormal(input.normal);
                    output.worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
                    return output;
                }


                half4 frag(Varyings input) : SV_Target {
                    UNITY_SETUP_INSTANCE_ID(input);
                    half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                    float3 worldPos = input.worldPos;
                    float3 normal = normalize(input.normal);
                    float3 viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                    float heightPercent = saturate((_MaxHeight - worldPos.y) / (_MaxHeight - _MinHeight));
                    float drawStrength = saturate(sign(heightPercent - _BaseStartHeight0));
                    float3 finalColor = lerp(_BaseColor.rgb * baseMap.rgb, _BaseColour0.rgb, drawStrength);
                    half4 c = half4(finalColor, 1);
                    c.rgb *= dot(normal, -viewDir);
                    c.rgb += unity_LightData.rgb * (1.0 - dot(normal, -viewDir));
                    return c;
                }
                ENDHLSL
            }
        }
            FallBack "Universal Render Pipeline/UnLit"
}