Shader "custom/post process"
{
    
    Properties
    {
        _chromatic_intensity ("Chromatic Intensity", Range(0, 1)) = 0.5
        _lens_distortion ("Lens Distortion", Float) = 1
        _BloomIntensity("Bloom Intensity", Float) = 0
        
        
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
            
            #define CHROMATIC_SAMPLES 1
            
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
            float _BloomIntensity;
            
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
                
                 
                // chromatic abberation
                float r = 0;
                float b = 0;
                
                for(float k=1; k<=CHROMATIC_SAMPLES; k ++) 
                {
                    r += saturate(tex2D(_MainTex, 
                            lerp(i.uv, Cent2Sat(float2(0,0)), (k/CHROMATIC_SAMPLES) * _chromatic_intensity)
                        ).r);
                    b += saturate(tex2D(_MainTex,
                            lerp(i.uv, Cent2Sat(normalize(Sat2Cent(i.uv))), (k/CHROMATIC_SAMPLES) * _chromatic_intensity)
                        ).b);
                }
                
                actual_col.r = (actual_col.r + r*5) / (5*CHROMATIC_SAMPLES + 1);
                actual_col.b = (actual_col.b + b*5) / (5*CHROMATIC_SAMPLES + 1);

                if(actual_col.r > 1)
                    return 1;
                
                return actual_col;    
            }
            
            ENDCG
        }
    }
}
