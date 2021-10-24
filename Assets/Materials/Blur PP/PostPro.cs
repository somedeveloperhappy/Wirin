using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PostPro : MonoBehaviour
{
    [SerializeField]
    Material blur_effect;
    
    new Camera camera;
    
    private void Awake() {
        camera = GetComponent<Camera>();
    }
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest) 
    {
        if(blur_effect != null) {
            Graphics.Blit(src, dest, blur_effect); // first pass ( horizontal )
        }
    }
}
