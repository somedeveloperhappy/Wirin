using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region main settings
    [SerializeField] AnimationCurve damageCurve;
    [SerializeField] AnimationCurve speedCurve;
    [SerializeField] float maxTime; // the time to reach damageCurve and speedCurve's max
    #endregion
    
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;
    
    public void Init(float t)
    {
        float time = t/maxTime;
        damage = damageCurve.Evaluate(time);
        speed = speedCurve.Evaluate(time);
    }
    
    #region editor settings
    [SerializeField] float boundryRange = 2;
    #endregion
    
    private void FixedUpdate() 
    {
        transform.position += transform.up * speed * Time.fixedDeltaTime;
        
        checkForScreenBound();
    }

    private void checkForScreenBound()
    {
        if( transform.position.x + boundryRange < 0 ||
            transform.position.x - boundryRange > Screen.width ||
            transform.position.y + boundryRange < 0 ||
            transform.position.y - boundryRange > Screen.height)
        {
            // it's out of screen
            Destroy(gameObject);
        }
    }
}
