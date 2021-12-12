// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprite/NewEmission"
{
	Properties
    {
        _FillValue ("FillValue", Range(0, 1)) = 0.5
        _Smoothness ("Smoothness", Range(0, 1)) = 0.1
        
        _Shininess ("Shininess", Range(0, 1)) = 1
        
	    [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	    _Color("Tint", Color) = (1,1,1,1)
	    [MaterialToggle] PixelSnap("Pixel snap", Float) = 0

	    [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
	    [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
	    [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
	    [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

	    // required for UI.Mask
	    [HideInInspector] _StencilComp("Stencil Comparison", Float) = 8
	    [HideInInspector] _Stencil("Stencil ID", Float) = 0
	    [HideInInspector] _StencilOp("Stencil Operation", Float) = 0
	    [HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255
	    [HideInInspector] _StencilReadMask("Stencil Read Mask", Float) = 255
	    [HideInInspector] _ColorMask("Color Mask", Float) = 15
    }

    SubShader
    {
	    Tags
	    {
		    "Queue" = "Transparent"
		    "IgnoreProjector" = "True"
		    "RenderType" = "Transparent"
		    "PreviewType" = "Plane"
		    "CanUseSpriteAtlas" = "True"
	    }

	    Cull Off
	    Lighting Off
	    ZWrite Off
	    Blend One OneMinusSrcAlpha

	    // required for UI.Mask
	    Stencil
	    {
		    Ref[_Stencil]
		    Comp[_StencilComp]
		    Pass[_StencilOp]
		    ReadMask[_StencilReadMask]
		    WriteMask[_StencilWriteMask]
	    }
        ColorMask[_ColorMask]

	    Pass
	    {
		    CGPROGRAM
		    #pragma vertex vert
		    #pragma fragment frag
		    #pragma multi_compile _ PIXELSNAP_ON
		    #pragma multi_compile _ GRAYSCALE_ON GRAYSCALE_OFF
		    #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		    #include "UnityCG.cginc"

		    struct appdata_t
		    {
			    float4 vertex   : POSITION;
			    float4 color    : COLOR;
			    float2 uv : TEXCOORD0;
		    };

		    struct v2f
		    {
			    float4 vertex   : SV_POSITION;
			    fixed4 color : COLOR;
			    half2 uv  : TEXCOORD0;
		    };

		    fixed4 _Color;
            
            
		    v2f vert(appdata_t IN)
		    {
			    v2f OUT;
			    OUT.vertex = UnityObjectToClipPos(IN.vertex);
			    OUT.uv = IN.uv;
			    OUT.color = IN.color * _Color;
			    #ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
			    #endif

			    return OUT;
		    }

		    sampler2D _MainTex;
            float _FillValue;
            float _Smoothness;
            float _Shininess;
            
		    fixed4 frag(v2f IN) : SV_Target
		    {
			    fixed4 c = tex2D(_MainTex, IN.uv) * IN.color;
                
                IN.uv -= 0.5f;
                IN.uv *= 2;
                
                float val = dot(normalize(IN.uv), float2(0,-1)); // -1 to 1
                val = (val+1)/2; // 0 to 1 now
                
                
                // fade effect
                if(val > _FillValue)
                {
                    c.a *= lerp(1, 0, distance(val, _FillValue) / _Smoothness);
                }
                
                // shininess effect
                if(_Shininess > 0) {
                    val = 1 - val; // reversed
                    c.rgb += (val + (2-distance(IN.uv, float2(0, 1)))) * _Shininess;
                }
                
				c.rgb *= c.a;
				return c;
		    }

		    ENDCG
	    }
    }
}
