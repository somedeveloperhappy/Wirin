using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trinon : MonoBehaviour, IOnPlayerPress, IPlayerPart
{
    private const int SPEEDUP_BREAKSPD_MULTIP = 2;
    
    
    #region general settigns
    
    [System.Serializable] public abstract class Curve
    {
        public AnimationCurve curve;
        protected float time;
        [HideInInspector] public float last_key_time;
        
        
        public abstract void IncreaseTime();
        public abstract void DecreaseTime();
        public float Evaluate() => curve.Evaluate(time);
        
        public void Init() {
            last_key_time = curve.keys[curve.keys.Length-1].time;
        }
    }
    
    [System.Serializable] public class SpeedDownCurve : Curve
    {
        public float speedUpLerp = 1;
        public float speedDown = 1;
        
        
        public override void IncreaseTime() {
            time = Mathf.Lerp(time, last_key_time, Time.deltaTime * speedUpLerp);
        }

        public override void DecreaseTime() {
            time -= Time.deltaTime * speedDown;
            if(time < 0) time = 0;
        }
    }
    [System.Serializable] public class SpeedUpCurve : Curve
    {
        public float speedDownLerp = 2;
        public float speedUp = 1;
        
        public override void DecreaseTime() {
            time = Mathf.Lerp(time, 0, Time.deltaTime * speedDownLerp);
        }

        public override void IncreaseTime() {
            time += Time.deltaTime * speedUp;
            if( time > last_key_time ) 
                time = last_key_time;
        }
    }
    
    public SpeedDownCurve speedDown;
    public SpeedUpCurve speedUp;
    
    [SerializeField] float rotateSpeed;
    [SerializeField] Vector2 bulletPosOffset;
    public Bullet bulletPrefab;
    
    #endregion
    
    #region quick references
         public Pivot pivot => References.pivot;
    #endregion
    
    #region refs
    public PlayerInfo playerInfo;
    #endregion
    
    public Vector3 GetBulletPositionInWorld() => transform.position + transform.up * bulletPosOffset.y + transform.right * bulletPosOffset.x;
    
    [HideInInspector]
    public float rotateSpeedMultiplier = 1;
    float rotateSpeedMultiplier_internal = 1;
    
    
    
    private void Start() 
    {
        this.Initialize();
        speedDown.Init();
        speedUp.Init();
    }
    
    public void OnPressDown(float duration)
    {
        // speed up should be cancelled
    }

    public void OnPressUp(float duration)
    {
        Shoot(duration);
    }

    public void OnPressDownUpdate()
    {
        speedDown.IncreaseTime();
        speedUp.DecreaseTime();
        
        apply_internal_speedMultiplier();
        Move();
    }

    public void OnPressUpUpdate()
    {
        speedDown.DecreaseTime();
        speedUp.IncreaseTime();
        
        apply_internal_speedMultiplier();
        Move();
    }
    
    void apply_internal_speedMultiplier()
    {
        rotateSpeedMultiplier_internal = speedDown.Evaluate() * speedUp.Evaluate();
    }
    
    void Move()
    {
        transform.RotateAround(pivot.transform.position, Vector3.forward, rotateSpeed * rotateSpeedMultiplier_internal * rotateSpeedMultiplier * Time.deltaTime);
    }
    
    void Shoot(float duration)
    {
        Instantiate(bulletPrefab, GetBulletPositionInWorld(), transform.rotation).Init(playerInfo, duration);
    }

    public PlayerInfo GetPlayerInfo() => playerInfo;
}
