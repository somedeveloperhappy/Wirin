using System;
using System.Collections.Generic;
using LevelManaging;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region general settings

    [SerializeField] float health;
    public float damage;
    public int points;
    public float speed;

    #endregion

    #region handy refs

    Vector3 pivotPos => References.pivot.transform.position;
    LevelManager levelManager => References.levelManager;

    #endregion


    public float Health => health;
    float deltaTime;

    #region static

    static public List<Enemy> instances = new List<Enemy>();
    static public float timeScale = 1;

    #endregion

    #region private info

    float stunTime = 0;
    bool was_stunned = false;

    #endregion

    #region events

    public event System.Action<float, PlayerBulletDamageInfo> OnTakeDamage;

    #endregion


    private void Start() {
        instances.Add(this);
        RotateTowardsPivot();
    }

    public void Init(int points) {
        this.points = points;

        health = points / (10f + UnityEngine.Random.Range(-5f, 5f));
    }

    private void FixedUpdate() {
        deltaTime = Time.fixedDeltaTime * timeScale;


        if (stunTime <= 0) {
            if (was_stunned) {
                was_stunned = false;
                StunEnd();
            }

            MoveForward();
        }
        else {
            stunTime -= deltaTime;
            if (was_stunned) StunUpdate();
            else {
                StunStart();
                was_stunned = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log($"Hit {other.name}");
        var playerpart = other.GetComponent<IPlayerPart>();
        if (playerpart != null) {
            Debug.Log($"even here!");
            HitPlayer(playerpart.GetPlayerInfo());
            DestroyEnemy();
        }
    }

    private void OnDestroy() {
        instances.Remove(this);
    }


    private void StunStart() { }
    private void StunUpdate() { }
    private void StunEnd() { }

    private void DestroyEnemy() {
        Destroy(gameObject);
        levelManager.OnEnemyDestroy(this);
    }


    private void RotateTowardsPivot() {
        transform.up = pivotPos - transform.position;
    }

    private void MoveForward() {
        transform.Translate(transform.up * speed * deltaTime, Space.World);
    }

    public void TakeDamage(PlayerBulletDamageInfo damageInfo) {
        var health_before = health;

        health -= damageInfo.damage;
        stunTime += damageInfo.stunDuration;

        if (health < 0) health = 0;

        if (health == 0)
            DestroyEnemy();

        OnTakeDamage?.Invoke(health_before, damageInfo);
    }


    private void HitPlayer(PlayerInfo playerInfo) {
        playerInfo.TakeDamage(new EnemyDamageInfo(damage));
    }
}
