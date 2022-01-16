// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/UI_img_repeat_advance"
{
	Properties
    {
	    _GrayScale("Saturation", Range(0, 1.0)) = 0.0
        _Smooth("Smoothness", Range(0.0, 1)) = 0.5
        
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
			    fixed4 c = tex2D(_MainTex, IN.uv) * IN.color;

                IN.uv.x = IN.uv.x * 2 - 1;
                IN.uv.y = IN.uv.y * 2 - 1;
                
				float alpha = c.a;
				float d = (dot(float2(0, 1), normalize(IN.uv)) + 1)/2;
				if ( d < 0 ) d = 0;
				
				if(_GrayScale >= d) {
					alpha *= 1 - distance(_GrayScale, d);
					alpha = (alpha-1+_Smooth)/_Smooth;
					if(alpha<0) discard;
				}
				
				saturate(alpha);
				// return alpha;
                // if(c.a <0.1f) c.a = 0;
				
				c.a = alpha;
				c.rgb *= alpha;
				return c;
		    }

		    ENDCG
	    }
    }
}
