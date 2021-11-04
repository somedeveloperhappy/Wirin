using System;
using System.Collections.Generic;
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
    
    
    private void OnEnable() {
        instances.Add(this);
        RotateTowardsPivot();
    }
    private void OnDisable() {
        instances.Remove(this);
    }

    private void FixedUpdate()
    {
        deltaTime = Time.fixedDeltaTime * timeScale;
        
        
        if(stunTime <= 0) {
            
            if(was_stunned) {
                was_stunned = false;
                StunEnd();
            }
            MoveForward();
            
        } else {
            
            stunTime -= deltaTime;
            if(was_stunned) StunUpdate();
            else {
                StunStart();
                was_stunned = true;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        var playerpart = other.GetComponent<IPlayerPart>();
        if(playerpart != null) {
            HitPlayer(playerpart.GetPlayerInfo());
        }
    }


    private void StunStart()
    {
        
    }
    private void StunUpdate()
    {
        
    }
    private void StunEnd()
    {
        
    }
    private void DestroyEnemy()
    {
        
    }
    

    private void RotateTowardsPivot() {
        transform.up = pivotPos - transform.position;
    }
    
    private void MoveForward() {
        transform.Translate(transform.up * speed * deltaTime, Space.World);
    }

    public void TakeDamage(PlayerBulletDamageInfo damageInfo)
    {
        health -= damageInfo.damage;
        stunTime += damageInfo.stunDuration;
        
        if(health < 0) health = 0;
        
        if(health == 0) 
            DestroyEnemy();
    }
    

    private void HitPlayer(PlayerInfo playerInfo) {
        playerInfo.TakeDamage(new EnemyDamageInfo(damage));
    }
}
