using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour, IOnPlayerPress
{
    public AnimationCurve scaleOnPress;
    public float speedOnDown = 1, speedOnUp = 10;
    float scale_curve_t = 0;
    
    /// <summary>
    /// the time for last key in scaleOnPress
    /// </summary>
    float scale_onpress_curve_last_time;
    
    virtual protected void Start() {
        this.Initialize();
        scale_onpress_curve_last_time = scaleOnPress.keys[scaleOnPress.keys.Length-1].time;
    }

    public void OnPressDown()
    {
        
    }

    public void OnPressUp()
    {
        
    }

    public void OnPressDownUpdate(float duration)
    {
        scale_curve_t += Time.deltaTime * speedOnDown;
        ApplyScale();
    }


    public void OnPressUpUpdate(float duration)
    {
        scale_curve_t -= Time.deltaTime * speedOnUp;
        ApplyScale();
    }
    
    private void ApplyScale()
    {
        scale_curve_t = Mathf.Clamp(scale_curve_t, 0, scale_onpress_curve_last_time);
        transform.localScale = Vector3.one * scaleOnPress.Evaluate(scale_curve_t);
    }
}
