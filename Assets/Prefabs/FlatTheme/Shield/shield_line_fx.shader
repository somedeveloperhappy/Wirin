Shader "Unlit/shield_line_fx"
{
    Properties
    {
        _Offset("Offset", Vector) = (0, 0, 1, 1)
        _SpaceX("SpaceX", Float) = 0
        _SpaceY("SpaceY", Float) = 0
        
        _OverlayTex ("Overlay Tex", 2D) = "white" {}
        _MainTex ("Main Tex", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Lighting Off
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _OverlayTex;
            
            float4 _Offset;
            float _SpaceX;
            float _SpaceY;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.x /= _MainTex_ST.x;
                uv.y /= _MainTex_ST.y;
                fixed4 add_col = tex2D(_OverlayTex, uv);
                
                i.uv.x *= _Offset.z;
                i.uv.y *= _Offset.w;
                i.uv.x += _Offset.x;
                i.uv.y += _Offset.y;
                
                i.uv.x %= 1 + _SpaceX;
                i.uv.y %= 1 + _SpaceY;
                
                if(i.uv.x > 1 || i.uv.y > 1 || i.uv.x < 0 || i.uv.y < 0) discard;
                
                fixed4 col = tex2D(_MainTex, i.uv);
                col.r *= add_col.r;
                col.g *= add_col.g;
                col.b *= add_col.b;
                col.a *= add_col.a;
                return col;
            }
            ENDCG
        }
    }
}
