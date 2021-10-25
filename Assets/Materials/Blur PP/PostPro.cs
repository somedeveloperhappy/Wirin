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
    
    RenderTexture rt;
    
    private void OnPreRender() {
        rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);    
        camera.targetTexture = rt;
        
    }
    private void OnPostRender() 
    {
        
        text.text = ((Time.realtimeSinceStartup-t)*1000).ToString("F2");
        t = Time.realtimeSinceStartup;
        
        camera.targetTexture = null; // null means buffer
        
        RenderTexture tmp = RenderTexture.GetTemporary(rt.width, rt.height, rt.depth);
        
        Graphics.Blit(rt, tmp, blur_effect, 0); // null , directly render to screen
        RenderTexture.ReleaseTemporary(rt);
        
        Graphics.Blit(tmp, null, blur_effect, 1); // null , directly render to screen
        RenderTexture.ReleaseTemporary(tmp);
        
        text.text += "\n" +((Time.realtimeSinceStartup-t)*1000).ToString("F2");
        t = Time.realtimeSinceStartup;
            
    }
    
    public void SetBlurQuality(int index)
    {
        switch(index)
        {
        case 0 :
            blur_effect.EnableKeyword("ULTRA");
            blur_effect.DisableKeyword("HIGH");
            blur_effect.DisableKeyword("MEDIUM");
            blur_effect.DisableKeyword("LOW");
            break;
        case 1:
            blur_effect.DisableKeyword("ULTRA");
            blur_effect.EnableKeyword("HIGH");
            blur_effect.DisableKeyword("MEDIUM");
            blur_effect.DisableKeyword("LOW");
            break;
        case 2 :
            blur_effect.DisableKeyword("ULTRA");
            blur_effect.DisableKeyword("HIGH");
            blur_effect.EnableKeyword("MEDIUM");
            blur_effect.DisableKeyword("LOW");
            break;
        case 3 :
            blur_effect.DisableKeyword("ULTRA");
            blur_effect.DisableKeyword("HIGH");
            blur_effect.DisableKeyword("MEDIUM");
            blur_effect.EnableKeyword("LOW");
            break;
        }
        
        Debug.Log($"{index}");
    }
}
