using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayManagement
{
    public class PlayManager : MonoBehaviour
    {
        
        public PivotScaleSettings pivotScaleSettings;
        public BlurOnPressSettings blurOnPressSettings;
        public TrinonRotateSettings trinonRotateSettings;
        public BulletSpeedSettings bulletSpeedSettings;
        
        
        #region quick references
            Pivot pivot => References.pivot;
            Trinon trinon => References.trinon;
            PostPro postPro => References.postPro;
        #endregion
        
        float deltaTime;
        bool pivotWasPressed = false; // for checking if in previous frame the pivot was pressed
        float pivotDownTIme = 0; // how much pivot has been down
        
        
        private void Update() 
        {
            deltaTime = Time.deltaTime;
            
            InputUpdates();
        }
        
        private void FixedUpdate() 
        {
            deltaTime = Time.fixedDeltaTime;
        }

        private void InputUpdates()
        {
            if(InputGetter.isPoinerDown && pivot.bounds.Contains(InputGetter.GetPointerWorldPosition())) {
                OnPivotPressUpdate();
                pivotWasPressed = true;
            }
            else{
                if(pivotWasPressed) {
                    pivotWasPressed = false;
                    OnPivotPressStop();
                }
                OnPivotReleasedUpdate();
            }
        }


        private void OnPivotPressUpdate()
        {
            pivot.transform.localScale = Vector3.one * pivotScaleSettings.IncreaseAndGet(ref deltaTime);
            trinon.rotateSpeedMultiplier = trinonRotateSettings.IncreaseAndGet(ref deltaTime);
            
            postPro.blurSettings.intensity = blurOnPressSettings.IncreaseAndGet(ref deltaTime);
            postPro.ApplyIntensity();
            
            pivotDownTIme += Time.deltaTime;
        }
        
        private void OnPivotPressStop()
        {
            Shoot();
            
            pivotDownTIme = 0;
        }


        private void OnPivotReleasedUpdate()
        {
            pivot.transform.localScale = Vector3.one * pivotScaleSettings.DecreaseAndGet(ref deltaTime);
            trinon.rotateSpeedMultiplier = trinonRotateSettings.DecreaseAndGet(ref deltaTime);
            
            postPro.blurSettings.intensity = blurOnPressSettings.DecreaseAndGet(ref deltaTime);
            postPro.ApplyIntensity();
        }
        
        
        private void Shoot()
        {
            float bulletSpeed = bulletSpeedSettings.GetSpeedByTime(pivotDownTIme);
            
            var bullet = Instantiate<Bullet>(References.bulletPrefab, trinon.GetBulletPositionInWorld(), trinon.transform.rotation);
            bullet.speed *= bulletSpeed;
            
        }
    }
}