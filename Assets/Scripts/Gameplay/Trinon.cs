using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trinon : MonoBehaviour
{
    #region general settigns
        public float rotateSpeed;
        [SerializeField] Vector2 bulletPosOffset;
    #endregion
    
    #region quick references
         public Pivot pivot => References.pivot;
    #endregion
    
    public Vector3 GetBulletPositionInWorld() => transform.position + transform.up * bulletPosOffset.y + transform.right * bulletPosOffset.x;
    
    [HideInInspector]
    public float rotateSpeedMultiplier = 1;
    
    
    private void Update() 
    {
        transform.RotateAround(pivot.transform.position, Vector3.forward, rotateSpeed * rotateSpeedMultiplier * Time.deltaTime);
    }
    
}
