Shader "Unlit/Test"
{
    Properties
    {
        _MainTex1("Albedo (RGB)", 2D) = "white" {}
        _MainTex2("Albedo (RGB)", 2D) = "white" {}
        _lerpTexture("lerp", Range(0,1)) = 0
    }
        SubShader
        {
            Tags { 
               
                "RenderType" = "Opaque" 
              
            }
            LOD 200



            CGPROGRAM 
           //HLSLPROGRAM
      


           #pragma surface surf Standard fullforwardshadows

               sampler2D _MainTex1;
               sampler2D _MainTex2;
               float _lerpTexture;

               struct Input
               {
                   float2 uv_MainTex1;
                   float2 uv_MainTex2;
               };

               void surf(Input IN, inout SurfaceOutputStandard o)
               {
                   half4 c = tex2D(_MainTex1, IN.uv_MainTex1);
                   half4 d = tex2D(_MainTex2, IN.uv_MainTex2);
                   o.Albedo = lerp(c.rgb, d.rgb, _lerpTexture);
                   o.Alpha = c.a;
               }


               ENDCG
                //ENDHLSL

            
        
          
        }
            //FallBack "Diffuse"
            FallBack "Standard"
}

