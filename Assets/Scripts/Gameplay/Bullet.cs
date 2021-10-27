using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public string targetTag = "Enemy";
    [SerializeField] float distanceFromScreen = 2;
    
    Collider col;
    
    float t = 0;
    const float SCREEN_BOUND_CHECK = 1;
    
    #region static calculations
         static public List<Bullet> bullets = new List<Bullet>();
    #endregion
    
    private void Awake() {
        col = GetComponent<Collider>();
        bullets.Add(this);
    }
    
    private void Update() 
    {
        transform.position += transform.up * speed * Time.deltaTime;
        
        if((t+=Time.deltaTime)>=SCREEN_BOUND_CHECK) DestroyIfOutOfScreen();
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.transform.CompareTag(targetTag)) {
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy() {
        bullets.Remove(this);
    }
    

    private void DestroyIfOutOfScreen()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        
        if(pos.x + distanceFromScreen < 0 || pos.x - distanceFromScreen > Screen.width || pos.y + distanceFromScreen < 0 || pos.y - distanceFromScreen > Screen.height)
            Destroy(gameObject);
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(transform.position, Vector3.one * distanceFromScreen);
    }
}
