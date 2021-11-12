using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PostPro : MonoBehaviour
{
    [UnityEngine.Serialization.FormerlySerializedAs("blur_effect")]
    public Material pp_mat;
    
    
    new Camera camera;
    
    private void Awake() {
        camera = GetComponent<Camera>();
    }
    
    RenderTexture rt;
    
    
    
    // private void OnEnable() {
    //     Debug.Log($"{camera.targetTexture.graphicsFormat}");
    //     // rt = new RenderTexture(Screen.width, Screen.height, 0, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8_UNorm, 0);
    //     // // rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);    
    //     // camera.targetTexture = rt;
    // }
    
    // private void OnPostRender() 
    // {
    //     // camera.targetTexture = null; // null means buffer
        
    //     // RenderTexture tmp = RenderTexture.GetTemporary(rt.width, rt.height, rt.depth);
        
    //     Graphics.Blit(camera.targetTexture, null, pp_mat, 0); // null , directly render to screen
    //     // RenderTexture.ReleaseTemporary(rt);
    // }
    public void SetChromIntensity(float value) {
        pp_mat.SetFloat("_chromatic_intensity", value);
    }
    
    /// <summary>
    /// sets lens distortion value. value better be between -1 and 1
    /// </summary>
    /// <param name="value"></param>
    public void SetLensDistortion(float value) {
        pp_mat.SetFloat("_lens_distortion", value);
    }
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, null, pp_mat);
    }
}
