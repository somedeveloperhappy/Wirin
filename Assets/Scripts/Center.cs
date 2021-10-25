using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    public float minScaleOnHeld = 0.8f;
    public float heldScaleSpeed = 5f;
    public float unheldScaleSpeed = 10f;
    
    #region privae vars
        public SpriteRenderer targetSpriteRenderer;
        public PostPro postPro;
        public string blur_intensity_name = "_blur_intensity";
        public int min = 0, max = 40;
        private float blur;
    #endregion
    
    bool was_down = false;
    
    private void Start() 
    {
        Debug.Log($"{GetComponent("Transform").name}");
        blur = min;
    }
    
    
    private void Update() 
    {
        // Debug.Log($"{InputGetter.pointerPosition}");
        
        if(InputGetter.isPoinerDown &&  targetSpriteRenderer.bounds.Contains(InputGetter.GetPointerWorldPosition()))
        {
            // first frame
            if(!was_down)   onPointerDownStart();
            else            onPointerDownUpdate();
            
            was_down = true;
            
        } else {
            
            if(was_down) {
                // on first frame of not poiter down
                was_down = false;
                onPointerDownEnd();
            }
            
            onPointerUpUpdate();
        }
        
    }


    #region on pointer funcs
    private void onPointerUpUpdate()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, unheldScaleSpeed * Time.deltaTime);
        blur = Mathf.Lerp(blur, min, unheldScaleSpeed/4 * Time.deltaTime);
        postPro.blur_effect.SetFloat(blur_intensity_name, blur);
    }

    private void onPointerDownEnd()
    {
        
    }

    private void onPointerDownUpdate()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * minScaleOnHeld, heldScaleSpeed * Time.deltaTime);
        blur = Mathf.Lerp(blur, max, unheldScaleSpeed/4 * Time.deltaTime);
        postPro.blur_effect.SetFloat(blur_intensity_name, blur);
    }

    private void onPointerDownStart()
    {
        
    }
#endregion
    
}
