using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPlayerPart
{
    #region main settings

    [SerializeField] public AnimationCurve damageCurve;
    [SerializeField] AnimationCurve speedCurve;
    [SerializeField] AnimationCurve stunCurve;

    #endregion

    #region refs

    public PlayerInfo playerInfo;

    #endregion

    public float damage;
    public float speed;
    public float stunDuration;

    public void Init(PlayerInfo playerInfo, float pressed_duration) {
        this.playerInfo = playerInfo;
        damage = damageCurve.Evaluate(pressed_duration);
        speed = speedCurve.Evaluate(pressed_duration);
        stunDuration = stunCurve.Evaluate(pressed_duration);
    }

    #region editor settings

    [SerializeField] float boundryRange = 2;
    const int CHECK_FOR_SCREEN_BOUND_T = 1;
    float last_screen_bound_check = 0;

    #endregion

    Rigidbody2D rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        transform.position += transform.up * speed * Time.fixedDeltaTime;

        checkForScreenBound();
    }

    private void checkForScreenBound() {
        if (Time.timeSinceLevelLoad - last_screen_bound_check > CHECK_FOR_SCREEN_BOUND_T) {
            last_screen_bound_check = Time.timeSinceLevelLoad;

            Vector2 pos_in_screen = References.currentCamera.WorldToScreenPoint(transform.position);

            if (pos_in_screen.x + boundryRange < 0 ||
                pos_in_screen.x - boundryRange > Screen.width ||
                pos_in_screen.y + boundryRange < 0 ||
                pos_in_screen.y - boundryRange > Screen.height) {
                // it's out of screen
                Destroy(gameObject);
            }
        }
    }


    public void OnCollisionEnter2D(Collision2D other) 
    {
        Debug.Log($"collided with {other.gameObject.name}");
        var enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy != null) {
            Debug.Log($"Damaging enemy {enemy.name} , damage : {damage}");
            enemy.TakeDamage(new PlayerBulletDamageInfo(damage, stunDuration));

            DestroyBullet();
        }
    }

    private void DestroyBullet() {
        Destroy(gameObject);
    }

    public PlayerInfo GetPlayerInfo() => playerInfo;
}
