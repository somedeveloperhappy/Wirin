using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;


public class EnemyLifeShow : MonoBehaviour
{
    public Enemy enemy;
    public TextMesh textMesh;

    float previousHealth;
    float speed;

    #region normals

    Color normal_col;
    Vector3 normal_scale;

    #endregion

    float t = 0; // timer for lero effects

    [SerializeField] EnemyOnDamageSettings.OnDamageFx onDamageFx;


    private void Start() {
        normal_col = textMesh.color;
        normal_scale = transform.localScale;

        enemy.onTakeDamage += onTakeDamage;
    }


    private void onTakeDamage(float previousHealth, PlayerBulletDamageInfo damageInfo) {
        this.previousHealth = previousHealth;

        t = 0;
        ApplyEffectBasedOnT();
    }

    private void Update() {
        if (t >= 1) return;

        t += Time.deltaTime;

        if (t >= 1) {
            t = 1;
            backToNormal();
        }
        else {
            ApplyEffectBasedOnT();
        }
    }

    private void ApplyEffectBasedOnT() {
        textMesh.text = Mathf.CeilToInt(Mathf.Lerp(previousHealth, enemy.Health, t)).ToString();
        textMesh.color = Color.Lerp(onDamageFx.color, normal_col, t);
        transform.localScale = Vector3.Lerp(onDamageFx.scale, normal_scale, t);
    }

    private void backToNormal() {
        textMesh.text = Mathf.CeilToInt(enemy.Health).ToString();
        textMesh.color = normal_col;
        transform.localScale = normal_scale;
    }
}
