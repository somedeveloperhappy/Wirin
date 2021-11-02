using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour, IOnPlayerPress
{
    public AnimationCurve scaleOnPress;
    public float speedOnDown = 1, speedOnUp = 10;
    float scale_curve_t = 0;
    
    virtual protected void Start() {
        this.Initialize();
    }

    public void OnPressDown()
    {
        throw new NotImplementedException();
    }

    public void OnPressUp()
    {
        throw new NotImplementedException();
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
        scale_curve_t = Mathf.Clamp(scale_curve_t, 0, scaleOnPress.keys[scaleOnPress.keys.Length-1].time);
        transform.localScale = Vector3.one * scaleOnPress.Evaluate(scale_curve_t);
    }
}
