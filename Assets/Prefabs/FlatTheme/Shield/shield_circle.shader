Shader "Unlit/shield_circle"
{
    Properties
    {
        _Tile ("Tile", Float) = 0.5
        _Color ("Color", Color) = (1, 1, 1, 1)
        
        _Range("Range", Range(0, 1)) = 0.5
        _Soft("Soft", Range(0, 1)) = 0.2
        _MainTex ("Texture", 2D) = "white" {}
        _Cutout("Cutout", Range(0, 1)) = 1
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

            float _Tile;
            fixed4 _Color;
            float _Range;
            float _Soft;
            float _Cutout;
            
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
                i.uv.xy -= 0.5f;
                
                if(distance(i.uv, 0) > _Cutout) discard;
                
                float _dot = dot(normalize(i.uv), float2(0, 1)) * 0.5f + 0.5f;
                float dist_to_top = _dot - _Range;
                if(dist_to_top < 0) discard;
                
                _Color.a *= ((dist_to_top)/_Soft) * _dot;// - distance(_Range, dotVal);
                
                i.uv.x *= _Tile;  
                i.uv.y *= _Tile;
                
                
                float dis = distance(0, i.uv)%1;
                if(dis < 0.25f || dis > 0.75f)
                    discard;
                return _Color;
            }
            ENDCG
        }
    }
}
