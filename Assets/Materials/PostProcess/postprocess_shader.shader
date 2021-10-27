Shader "custom/post process"
{
    
    Properties
    {
        // shared
        [HideInInspector]
        [KeywordEnum(ULTRA, HIGH, MEDIUM, LOW)] _Samples ("sample amount", Float) = 2
        
        [HideInInspector] [Toggle(BLUR)] _blurEffect("Blur", float) = 0
        [HideInInspector] _blur_1_dir_x ("Blur direction pass 1 x", Float) = 1
        [HideInInspector] _blur_1_dir_y ("Blur direction pass 1 y", Float) = 0
        [HideInInspector] _blur_2_dir_x ("Blur direction pass 2 x", Float) = 0
        [HideInInspector] _blur_2_dir_y ("Blur direction pass 2 y", Float) = 1
        [HideInInspector] [KeywordEnum(GUASSIAN, BOX)] _blur ("Blur Type", Float) = 0
        [HideInInspector] _blur_intensity ("Intensity", Float) = 10
        [HideInInspector] _blur_mask ("blur mask", 2D) = "white" {}
        
        [HideInInspector] [Toggle(CHROM)] _chromEffect("Chromatic Aberration", float) = 0
        [HideInInspector] _chrom_intensity ("Intensity", Float) = 10
        
        
         
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
            #pragma fragment frag
            
            #pragma multi_compile _SAMPLES_ULTRA _SAMPLES_HIGH _SAMPLES_MEDIUM _SAMPLES_LOW
            
            #pragma shader_feature BLUR
            #pragma multi_compile _BLUR_GUASSIAN _BLUR_BOX
            #pragma shader_feature BLUR_MASK
            
            #pragma shader_feature CHROM


            // blur quality
#if defined(_SAMPLES_ULTRA)
            #define SAMPLE_COUNT 20
#elif defined (_SAMPLES_HIGH)
            #define SAMPLE_COUNT 9
#elif defined (_SAMPLES_MEDIUM)
            #define SAMPLE_COUNT 4
#elif defined (_SAMPLES_LOW)
            #define SAMPLE_COUNT 2
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
            
#ifdef BLUR
            float _blur_intensity;
            sampler2D _blur_mask;
            float _blur_1_dir_x;
            float _blur_1_dir_y;
#endif

#ifdef CHROM
            float _chrom_intensity;
#endif

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 actual_col = tex2D(_MainTex, i.uv);
                
#ifdef BLUR

    #ifdef BLUR_MASK
                float _blurCombiner = tex2D(_blur_mask, i.uv).y;
    #else
                float _blurCombiner = 1;
    #endif
                
                // BLUR output
                half3 blur_col = 0; // returning color
                // current kernel uv
                float multip; // multiplier for each elemnt
                half2 blur_dir = half2(_blur_1_dir_x, _blur_1_dir_y);
#endif

#ifdef CHROM
                // chrom output
                float3 chrom_col = 0;
                half2 chrom_dir = half2(1, 1);
                float chrom_r;
                float chrom_b;
                half chrom_samples_total_count = 0;
                for(half ind = 0; ind<= SAMPLE_COUNT; ind++) chrom_samples_total_count += ind;
#endif
                
                
                for(int n = -SAMPLE_COUNT; n <= SAMPLE_COUNT; n++) 
                {
#ifdef BLUR
    #ifdef _BLUR_GUASSIAN
                    multip = SAMPLE_COUNT - abs(n) + 1;
                        
                    blur_col += tex2D( _MainTex, i.uv + blur_dir * (n / (half)SAMPLE_COUNT) * _blur_intensity) * multip;
    #else
                    
                    blur_col += tex2D( _MainTex, i.uv + blur_dir * (n/(float)SAMPLE_COUNT) * _blur_intensity);
    #endif
#endif
#ifdef CHROM
                    // getting Red channel
                    if(n < 0) {
                        chrom_r += tex2D(_MainTex, i.uv + chrom_dir * (n / (half)SAMPLE_COUNT) * _chrom_intensity).x / SAMPLE_COUNT; // * (abs(n)-SAMPLE_COUNT + 1) / chrom_samples_total_count;
                    } else if ( n > 0 ) {
                        chrom_b += tex2D(_MainTex, i.uv + chrom_dir * (n / (half)SAMPLE_COUNT) * _chrom_intensity).z / SAMPLE_COUNT; // * (abs(n)-SAMPLE_COUNT + 1) / chrom_samples_total_count;
                    }
#endif 
                }
                
#ifdef BLUR
    #ifdef _BLUR_GUASSIAN
                blur_col /= pow(SAMPLE_COUNT + 1, 2);
    #else
                // center
                blur_col /= SAMPLE_COUNT * 2 + 1;
    #endif
                
    #ifdef BLUR_MASK
                actual_col.xyz = lerp(actual_col.xyz, blur_col, _blurCombiner);
    #else
                actual_col = fixed4(blur_col, 1);
    #endif
#endif

#ifdef CHROM
                chrom_col.x = chrom_r;
                chrom_col.y = actual_col;
                chrom_col.z = chrom_b;
                actual_col = fixed4(chrom_col, 1);
#endif
                
                return actual_col;
            }
            
            ENDCG
        }
        
        // vertical pass
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma multi_compile _SAMPLES_ULTRA _SAMPLES_HIGH _SAMPLES_MEDIUM _SAMPLES_LOW
            
            #pragma shader_feature BLUR
            #pragma multi_compile _BLUR_GUASSIAN _BLUR_BOX
            #pragma shader_feature BLUR_MASK

            // blur quality
#if defined(_SAMPLES_ULTRA)
            #define SAMPLE_COUNT 20
#elif defined (_SAMPLES_HIGH)
            #define SAMPLE_COUNT 9
#elif defined (_SAMPLES_MEDIUM)
            #define SAMPLE_COUNT 4
#elif defined (_SAMPLES_LOW)
            #define SAMPLE_COUNT 2
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
            
#ifdef BLUR
            float _blur_intensity;
            sampler2D _blur_mask;
            float _blur_2_dir_x;
            float _blur_2_dir_y;
#endif
            
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 actual_col = tex2D(_MainTex, i.uv);
                
#ifdef BLUR

    #ifdef BLUR_MASK
                float _blurCombiner = tex2D(_blur_mask, i.uv).y;
    #else
                float _blurCombiner = 1;
    #endif
                
                // BLUR output
                float3 blur_col = 0; // returning color
                // current kernel uv
                float multip; // multiplier for each elemnt
                half2 blur_dir = half2(_blur_2_dir_x, _blur_2_dir_y);
#endif
                
                
                for(int n = -SAMPLE_COUNT; n <= SAMPLE_COUNT; n++) 
                {
#ifdef BLUR
                    if(blur_dir.x != 0 || blur_dir.y != 0)
                    {
                        
    #ifdef _BLUR_GUASSIAN
                    multip = SAMPLE_COUNT - abs(n) + 1;
                        
                    blur_col += tex2D( _MainTex, i.uv + blur_dir * (n / (half)SAMPLE_COUNT) * _blur_intensity) * multip;
    #else
                    blur_col += tex2D( _MainTex, i.uv + blur_dir * (n/(float)SAMPLE_COUNT) * _blur_intensity);
    #endif
                    }
#endif
                    
                }
                
#ifdef BLUR
                if(blur_dir.x != 0 || blur_dir.y != 0)
                {
                    
    #ifdef _BLUR_GUASSIAN
                blur_col /= pow(SAMPLE_COUNT + 1, 2);
    #else
                // center
                blur_col /= SAMPLE_COUNT * 2 + 1;
    #endif
                
    #ifdef BLUR_MASK
                actual_col.xyz = lerp(actual_col.xyz, blur_col, _blurCombiner);
    #else
                actual_col = fixed4(blur_col, 1);
    #endif
                } 
#endif
                
                return actual_col;
            }
            ENDCG
        }
    }
    
    CustomEditor "pp_editor"
}
