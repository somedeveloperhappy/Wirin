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
        SpriteRenderer spriteRenderer;
    #endregion
    
    bool was_down = false;
    
    private void Start() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    
    private void Update() 
    {
        if(InputGetter.isPoinerDown &&  spriteRenderer.bounds.Contains(InputGetter.pointerPosition))
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
    }

    private void onPointerDownEnd()
    {
        
    }

    private void onPointerDownUpdate()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * minScaleOnHeld, heldScaleSpeed * Time.deltaTime);
    }

    private void onPointerDownStart()
    {
        
    }
#endregion
    
}
