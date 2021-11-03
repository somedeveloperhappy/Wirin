using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region main settings
    [SerializeField] AnimationCurve damageCurve;
    [SerializeField] AnimationCurve speedCurve;
    #endregion
    
    public float damage;
    public float speed;
    
    public void Init(float pressed_duration) {
        damage = damageCurve.Evaluate(pressed_duration);
        speed = speedCurve.Evaluate(pressed_duration);
    }
    
    #region editor settings
    [SerializeField] float boundryRange = 2;
    const int CHECK_FOR_SCREEN_BOUND_T = 1;
    float last_screen_bound_check = 0;
    #endregion
    
    private void FixedUpdate() 
    {
        transform.position += transform.up * speed * Time.fixedDeltaTime;
        
        checkForScreenBound();
    }

    private void checkForScreenBound()
    {
        if(Time.timeSinceLevelLoad - last_screen_bound_check > CHECK_FOR_SCREEN_BOUND_T) 
        {
            last_screen_bound_check = Time.timeSinceLevelLoad;
            
            Vector2 pos_in_screen = References.currentCamera.WorldToScreenPoint(transform.position);
            
            if( pos_in_screen.x + boundryRange < 0 ||
                pos_in_screen.x - boundryRange > Screen.width ||
                pos_in_screen.y + boundryRange < 0 ||
                pos_in_screen.y - boundryRange > Screen.height)
            {
                // it's out of screen
                Destroy(gameObject);
            }
        }
    }
}
