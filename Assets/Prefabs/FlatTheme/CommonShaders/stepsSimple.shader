Shader "Custom/Lighting/StepsSimple"
{
    Properties
    {
        _BrightColor ("Bright Color", Color) = (1, 1, 1, 1)
        _DarkColor ("dark Color", Color) = (1, 1, 1, 1)
        _Steps ("Steps", Range(0.0001, 10)) = 3
        
    }
 
    SubShader
    {
        Pass
        {
            Tags { "RenderType"="Opaque" "Queue"="Geometry" "LightMode"="ForwardBase" }
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            // Change "shader_feature" with "pragma_compile" if you want set this keyword from c# code
            #pragma shader_feature __ _SPEC_ON
 
            #include "UnityCG.cginc"
 
            struct v2f {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };
 
            v2f vert(appdata_base v)
            {
                v2f o;
                // World position
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
 
                // Clip position
                o.pos = mul(UNITY_MATRIX_VP, float4(o.worldPos, 1.));
 
                // Normal in WorldSpace
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
 
 
                return o;
            }
 
            float4 _BrightColor;
            float4 _DarkColor;
            float _Steps;
 
            fixed4 frag(v2f i) : SV_Target
            {
                // Light direction
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 worldNormal = normalize(i.worldNormal);
 
                fixed4 c = 1;
                c.rgb = lerp(_DarkColor, _BrightColor, (round(dot(worldNormal, lightDir) * _Steps))/_Steps);
 
                return c;
            }
 
            ENDCG
        }
    }
}
