// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GreyScaleSpriteShader"
{
	Properties
    {
        _FillMask("Fill mask", 2D) = "white" {}
        _value ("Value", Range(0, 1)) = 1
        
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
		    float _GrayScale;
            float _Smooth;
            
            sampler2D _FillMask;
            float _value;

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

		    fixed4 frag(v2f IN) : SV_Target
		    {
                // float mask = 1 - tex2D(_FillMask, IN.uv).r;
                if(_value == 0) discard;
                if(IN.uv.x > _value) discard;
                
			    fixed4 c = tex2D(_MainTex, IN.uv) * IN.color;

				
				c.rgb *= c.a;
				return c;
		    }

		    ENDCG
	    }
    }
}
