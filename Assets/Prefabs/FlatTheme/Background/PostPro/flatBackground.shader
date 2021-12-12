Shader "Hidden/flatBackground"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0.1, 0.1, 0.1, 1)
        _FogIntensity ("Fog Intensity", Range(0, 1)) = 0.5
        _Saturation ("Saturation", Range(0, 1)) = 0.4
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed4 _FogColor;
            float _FogIntensity;
            float _Saturation;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col = lerp(col, _FogColor, _FogIntensity);
                fixed4 avg = (col.r + col.g + col.b) / 3;
                col = lerp(col, avg, _Saturation);
                col.a = 1;
                return col;
            }
            ENDCG
        }
    }
}
