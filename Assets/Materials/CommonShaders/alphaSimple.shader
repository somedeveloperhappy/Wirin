Shader "Custom/Lighting/alphaSimple"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        
    }
 
    SubShader
    {
        Pass
        {
            Tags { "Queue"="Transparent" "LightMode"="ForwardBase" }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            // Change "shader_feature" with "pragma_compile" if you want set this keyword from c# code
            #pragma shader_feature __ _SPEC_ON
 
            #include "UnityCG.cginc"
 
            struct v2f {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };
 
            v2f vert(appdata_base v)
            {
                v2f o;
                
                // World position
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
 
                // Clip position
                o.pos = mul(UNITY_MATRIX_VP, float4(o.worldPos, 1.));
 
                return o;
            }
 
            float4 _MainColor;
        
            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(_MainColor.xyz, _MainColor.w) ;
            }
 
            ENDCG
        }
    }
}
