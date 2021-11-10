using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteAlways]
public class OnPresseffects : MonoBehaviour, IOnPlayerPress
{
    public pivotSettings.FillingEffect fillingEffect;
    [SerializeField] float discardSpeed = 5;

    #region handy refs
    Trinon trinon => References.trinon;
    Pivot pivot => References.pivot;
    Bullet bullet => trinon.bulletPrefab;
    #endregion

    float max_bulletdmg_time; // last key time for bullet's damage
    float max_bulletdmg;
    float t = 0;

    [Range (0f, 1f)] public float fillingEffectIntensity = 0;


    private void Start() {
        this.Initialize ();

        max_bulletdmg_time = bullet.damageCurve.keys[ bullet.damageCurve.keys.Length - 1 ].time;
        max_bulletdmg = bullet.damageCurve.keys[ bullet.damageCurve.keys.Length - 1 ].value;
    }

    public void OnPressDown(float duration) {
        return;
    }

    public void OnPressUp(float duration) {
        return;
    }

    public void OnPressDownUpdate() {
        t += Time.deltaTime;
        if (t >= max_bulletdmg_time)
            t = max_bulletdmg_time;
        fillingEffectIntensity = bullet.damageCurve.Evaluate (t) / max_bulletdmg;
        UpdateFillingEffect();
    }

    public void OnPressUpUpdate() {
        t -= Time.deltaTime * discardSpeed;
        if (t < 0) t = 0;
        fillingEffectIntensity = bullet.damageCurve.Evaluate (t) / max_bulletdmg;
        UpdateFillingEffect();
    }

    [ContextMenu (nameof (UpdateFillingEffect))]
    public void UpdateFillingEffect() {
        fillingEffect.Update (fillingEffectIntensity);
    }
}
