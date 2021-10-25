Shader "Hidden/blue"
{
    
    Properties
    {
        [HideInInspector]
        _blur_intensity ("Blur intensity", Int) = 10
        [HideInInspector]
        [Toggle(BLUR_GUASSIAN)] _blurGuassian ("Is Guassian", Float) = 0
        [HideInInspector]
        _blur_mask ("blur mask", 2D) = "white" {}
        [HideInInspector]
        [Toggle(BLUR_MASK)] _is_blur_mask ("is blur mask", Float) = 0
        
        [HideInInspector]
        [Toggle(BLUR_ULTRA)] _blur_ultra ("blur ultra", Float) = 0
        [HideInInspector]
        [Toggle(BLUR_HIGH)] _blur_high ("blur high", Float) = 0
        [HideInInspector]
        [Toggle(BLUR_MEDIUM)] _blur_medium ("blur medium", Float) = 0
        
        
        
        
        [HideInInspector]
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        
        // horizontal pass
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma shader_feature BLUR_GUASSIAN
            #pragma shader_feature BLUR_MASK
            #pragma shader_feature BLUR_ULTRA
            #pragma shader_feature BLUR_HIGH
            #pragma shader_feature BLUR_MEDIUM
            #pragma fragment frag

            // blur quality
#ifdef BLUR_ULTRA
            #define BLUR_SAMPLES 20
#elif BLUR_HIGH
            #define BLUR_SAMPLES 10
#elif BLUR_MEDIUM
            #define BLUR_SAMPLES 9
#else
            #define BLUR_SAMPLES 2
#endif
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            // built-in
            sampler2D _MainTex;
            float2 _MainTex_TexelSize;
            
            float _blur_intensity;
            sampler2D _blur_mask;
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 actual_col = tex2D(_MainTex, i.uv);
                
#ifdef BLUR_MASK
                float _blurCombiner = tex2D(_blur_mask, i.uv).y;
#else
                float _blurCombiner = 1;
#endif

#ifdef BLUR_GUASSIAN
                // GUASSIAN BLUR
                float3 col = 0; // returning color
                
                // current kernel uv
                float multip; // multiplier for each elemnt
                
                for(int n = -BLUR_SAMPLES; n <= BLUR_SAMPLES; n++) {
                    multip = BLUR_SAMPLES - abs(n) + 1;
                        
                    // horizontal sample
                    col += tex2D( _MainTex, float2( i.uv.x + (_MainTex_TexelSize.x * (n/(float)BLUR_SAMPLES) * _blur_intensity),
                                                    i.uv.y )
                    ) * multip;
                }
                
                col /= pow(BLUR_SAMPLES + 1, 2);
#else
                // BOX BLUR
                float3 col = 0;
                
                for(int n = -BLUR_SAMPLES; n <= BLUR_SAMPLES; n++) {
                    col += tex2D( _MainTex, float2( i.uv.x + (_MainTex_TexelSize.x * (n/(float)BLUR_SAMPLES) * _blur_intensity),
                                                    i.uv.y));
                }
                
                // center
                col /= BLUR_SAMPLES * 2 + 1;
#endif
                
#ifdef BLUR_MASK
                return lerp(actual_col, fixed4(col, 1), _blurCombiner);
#else
                return fixed4(col, 1);
#endif
            }
            
            ENDCG
        }
        
        // vertical pass
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma shader_feature BLUR_GUASSIAN
            #pragma shader_feature BLUR_MASK
            #pragma shader_feature BLUR_ULTRA
            #pragma shader_feature BLUR_HIGH
            #pragma shader_feature BLUR_MEDIUM
            #pragma fragment frag

            // blur quality
#ifdef BLUR_ULTRA
            #define BLUR_SAMPLES 20
#elif BLUR_HIGH
            #define BLUR_SAMPLES 10
#elif BLUR_MEDIUM
            #define BLUR_SAMPLES 9
#else
            #define BLUR_SAMPLES 2
#endif
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            // built-in
            sampler2D _MainTex;
            float2 _MainTex_TexelSize;
            
            float _blur_intensity;
            sampler2D _blur_mask;
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 actual_col = tex2D(_MainTex, i.uv);
                
#ifdef BLUR_MASK
                float _blurCombiner = tex2D(_blur_mask, i.uv).y;
#else
                float _blurCombiner = 1;
#endif

#ifdef BLUR_GUASSIAN
                // GUASSIAN BLUR
                float3 col = 0; // returning color
                
                // current kernel uv
                float multip; // multiplier for each elemnt
                
                for(int n = -BLUR_SAMPLES; n <= BLUR_SAMPLES; n++) {
                    multip = BLUR_SAMPLES - abs(n) + 1;
                        
                    // horizontal sample
                    col += tex2D( _MainTex, float2( i.uv.x,
                                                    i.uv.y + (_MainTex_TexelSize.y * (n/(float)BLUR_SAMPLES) * _blur_intensity))
                    ) * multip;
                }
                
                col /= pow(BLUR_SAMPLES + 1, 2);
#else
                // BOX BLUR
                float3 col = 0;
                
                for(int n = -BLUR_SAMPLES; n <= BLUR_SAMPLES; n++) {
                    col += tex2D( _MainTex, float2( i.uv.x,
                                                    i.uv.y + (_MainTex_TexelSize.y * (n/(float)BLUR_SAMPLES) * _blur_intensity)));
                }
                
                // center
                col /= BLUR_SAMPLES * 2 + 1;
#endif
                
#ifdef BLUR_MASK
                return lerp(actual_col, fixed4(col, 1), _blurCombiner);
#else
                return fixed4(col, 1);
#endif
            }
            ENDCG
        }
    }
    
    CustomEditor "pp_editor"
}
