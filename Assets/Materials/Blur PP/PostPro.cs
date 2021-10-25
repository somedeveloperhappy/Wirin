using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PostPro : MonoBehaviour
{
    public Material blur_effect;
    
    new Camera camera;
    
    public UnityEngine.UI.Text text;
    
    private float t = 0;
    
    private void Awake() {
        camera = GetComponent<Camera>();
    }
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest) 
    {
        if(blur_effect != null) {
            
            text.text = ((Time.realtimeSinceStartup-t)*1000).ToString("F2");
            t = Time.realtimeSinceStartup;
            
            RenderTexture tmp = RenderTexture.GetTemporary(src.width, src.height, src.depth, src.format);
            
            Graphics.Blit(src, tmp, blur_effect, 0); // first pass ( horizontal )
            Graphics.Blit(tmp, dest, blur_effect, 1); // second pass ( vertical )
            
            text.text += "\n" +((Time.realtimeSinceStartup-t)*1000).ToString("F2");
            t = Time.realtimeSinceStartup;
            
            RenderTexture.ReleaseTemporary(tmp);
            // src.Release();
            // tmp.Release();
        }
    }
}
