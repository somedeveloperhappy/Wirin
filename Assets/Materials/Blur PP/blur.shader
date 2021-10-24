Shader "Hidden/blue"
{
    
    Properties
    {
        [HideInInspector]
        _blur_intensity ("Blur intensity", Int) = 10
        [HideInInspector]
        _blurCombiner ("Blur Combiner", Range(0, 1)) = 0.5
        [HideInInspector]
        [Toggle(BLUR_GUASSIAN)] _blurGuassian ("Is Guassian", Float) = 0
        
        [HideInInspector]
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma shader_feature BLUR_GUASSIAN
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
            
            // built-in
            sampler2D _MainTex;
            float2 _MainTex_TexelSize;
            
            
            float _blur_intensity;
            float _blurCombiner;
            

            fixed4 frag (v2f i) : SV_Target
            {
                
#ifdef BLUR_GUASSIAN

                float3 col = 0; // returning color
                
                // current kernel uv
                float2 c_uv;
                float3 tmp_pixel; // tmp
                float multip; // multiplier for each elemnt
                float multip_sum = 0;
                
                // finding out the sum of all colors
                for(int n = -_blur_intensity; n <= _blur_intensity; n++)
                {
                    c_uv.x = i.uv.x + (_MainTex_TexelSize.x * n);
                    for(int m = -_blur_intensity; m <= _blur_intensity; m++)
                    {
                        c_uv.y = i.uv.y + (_MainTex_TexelSize.y * m);
                        tmp_pixel = tex2D(_MainTex, c_uv);
                        
                        // INSTRUCTION :
                        // col += tmp_pixel * ( P(x) + P(y) )
                        // P(x) :
                        // if x <= _blur_intensity :
                        //      = x + 0.5
                        // else 
                        //      = (_blur_intensity * 2) - x + 0.5
                        if (n <= 0) multip = n + _blur_intensity + 0.5f;
                        else        multip = - n + _blur_intensity + 0.5f;
                        
                        if (m <= 0) multip += m + _blur_intensity + 0.5f;
                        else        multip += - m + _blur_intensity + 0.5f;
                        
                        col += tmp_pixel * multip;
                    }
                }
                
                n = _blur_intensity + 1;
                col /= (2*n - 1)*(2*(pow(n,2)) - 2*n + 1);
                
                // return 1;
                return lerp(tex2D(_MainTex, i.uv), fixed4(col, 1), _blurCombiner);
                    
                    
#else 
                    fixed4 actual_col = tex2D(_MainTex, i.uv);
                    
                    fixed3 col_avg = fixed3(0, 0, 0);
                    float2 uv_offset;
                    
                    for(int n = -_blur_intensity; n <= _blur_intensity; n++)
                    {
                        uv_offset.x = i.uv.x + (_MainTex_TexelSize.x * n);
                        for(int m = -_blur_intensity; m <= _blur_intensity; m++)
                        {
                            uv_offset.y = i.uv.y + (_MainTex_TexelSize.y * m);
                            col_avg += tex2D(_MainTex, uv_offset);
                        }
                    }
                    
                    // center
                    col_avg /= pow(_blur_intensity * 2 + 1, 2);
                    
                    return fixed4(lerp(actual_col.xyz, col_avg, _blurCombiner), 1);
#endif
            
            }
            
            
            
            
            ENDCG
        }
    }
    
    CustomEditor "pp_editor"
}
