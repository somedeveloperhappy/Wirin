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
            set_ultra(true);
            set_high(false); set_medium(false); set_low(false);
            break;
        case 1:
            set_high(true);
            set_ultra(false); set_medium(false); set_low(false);
            break;
        case 2 :
            set_medium(true);
            set_ultra(false); set_high(false); set_low(false);
            break;
        case 3 :
            set_low(true);
            set_ultra(false); set_high(false); set_medium(false);
            break;
        }
        
        Debug.Log($"{index}");
        
        void set_ultra(bool b) { if(b) blur_effect.EnableKeyword("_SAMPLES_ULTRA"); else blur_effect.DisableKeyword("_SAMPLES_ULTRA"); }
        void set_high(bool b)  { if(b) blur_effect.EnableKeyword("_SAMPLES_HIGH");  else blur_effect.DisableKeyword("_SAMPLES_HIGH"); }
        void set_medium(bool b){ if(b) blur_effect.EnableKeyword("_SAMPLES_MEDIUM");  else blur_effect.DisableKeyword("_SAMPLES_MEDIUM"); }
        void set_low(bool b)   { if(b) blur_effect.EnableKeyword("_SAMPLES_LOW");  else blur_effect.DisableKeyword("_SAMPLES_LOW"); }
    }
    
}
