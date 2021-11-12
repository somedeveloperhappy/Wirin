Shader "custom/post process"
{
    
    Properties
    {
        // shared
        // [HideInInspector]
        // [KeywordEnum(ULTRA, HIGH, MEDIUM, LOW)] _Samples ("sample amount", Float) = 2
        
        // [HideInInspector] [Toggle(BLUR)] _blurEffect("Blur", float) = 0
        // [HideInInspector] _blur_1_dir_x ("Blur direction pass 1 x", Float) = 1
        // [HideInInspector] _blur_1_dir_y ("Blur direction pass 1 y", Float) = 0
        // [HideInInspector] _blur_2_dir_x ("Blur direction pass 2 x", Float) = 0
        // [HideInInspector] _blur_2_dir_y ("Blur direction pass 2 y", Float) = 1
        // [HideInInspector] [KeywordEnum(GUASSIAN, BOX)] _blur ("Blur Type", Float) = 0
        // [HideInInspector] _blur_intensity ("Intensity", Float) = 10
        // [HideInInspector] _blur_mask ("blur mask", 2D) = "white" {}
        
        // [HideInInspector] [Toggle(CHROM)] _chromEffect("Chromatic Aberration", float) = 0
        // [HideInInspector] _chrom_intensity ("Intensity", Float) = 10
        
        _chromatic_intensity ("Chromatic Intensity", Range(0, 1)) = 0.5
        _lens_distortion ("Lens Distortion", Float) = 1
        
        
         
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
            
            #define SAMPLE_COUNT 2
            
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
            // float2 _MainTex_TexelSize;
            
            float _chromatic_intensity;
            float _lens_distortion;
            
            float2 Cent2Sat(float2 uv) {
                return (uv + 1) / 2;
            }
            
            float2 Sat2Cent(float2 uv) {
                return uv * 2 - 1;
            }
            
            float2 DistanceToCorner(float2 uv) {
                uv.x = abs(uv.x);
                uv.y = abs(uv.y);
                if(uv.x >= uv.y) {
                    return 1 - uv.x;
                } else {
                    return 1 - uv.y;
                }
            }
            

            fixed4 frag (v2f i) : SV_Target {
                
                i.uv = Sat2Cent(i.uv);
                i.uv = lerp(i.uv, float2(0,0), _lens_distortion * DistanceToCorner(i.uv));
                i.uv = Cent2Sat(i.uv);
                
                fixed4 actual_col = tex2D(_MainTex, i.uv);
                
                float r = 0;
                float b = 0;
                
                for(float k=1; k<=SAMPLE_COUNT; k ++) {
                    r += tex2D(_MainTex, 
                            lerp(i.uv, Cent2Sat(float2(0,0)), (k/SAMPLE_COUNT) * _chromatic_intensity)
                        ).r;
                    b += tex2D(_MainTex,
                            lerp(i.uv, Cent2Sat(normalize(Sat2Cent(i.uv))), (k/SAMPLE_COUNT) * _chromatic_intensity)
                        ).b;
                }
                
                
                
                actual_col.r = (actual_col.r + r*5) / (5*SAMPLE_COUNT + 1);
                actual_col.b = (actual_col.b + b*5) / (5*SAMPLE_COUNT + 1);
                
                return actual_col;    
            }
            
            ENDCG
        }
    }
}
