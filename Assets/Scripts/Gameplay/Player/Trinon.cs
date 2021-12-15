using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Player
{
    public class Trinon : MonoBehaviour, IOnPlayerPress, IPlayerPart
    {

        private const int SPEEDUP_BREAKSPD_MULTIP = 2;

        #region general settigns

        public SpeedDownCurve speedDown;
        public SpeedUpCurve speedUp;

        [SerializeField] private float rotateSpeed;
        [SerializeField] private Vector2 bulletPosOffset;
        [FormerlySerializedAs("bulletPrefab")] public PlayerNormalBullet playerNormalBulletPrefab;

        #endregion

        #region actions

        /// <summary>
        ///     gets called every Update frame during shooting press
        /// </summary>
        public delegate void onShootingPressHandler(float duration);

        public event onShootingPressHandler onShootingPressUpdate;
        public event onShootingPressHandler onShootingPressEnd;
        public event Action onShootingPressStart;

        #endregion


        #region refs

        public PlayerInfo playerInfo;

        #endregion

        [HideInInspector] public float rotateSpeedMultiplier = 1;
        private float rotateSpeedMultiplier_internal = 1;

        #region quick references

        public Pivot pivot => References.pivot;

        #endregion

        public void OnPressDown(float duration)
        {
            // speed up should be cancelled
            onShootingPressStart?.Invoke();
        }

        public void OnPressUp(float duration)
        {
            Shoot();
            onShootingPressEnd?.Invoke(duration);
        }

        public void OnPressDownUpdate()
        {
            speedDown.IncreaseTime();
            speedUp.DecreaseTime();

            apply_internal_speedMultiplier();
            Move();

            onShootingPressUpdate?.Invoke(References.playerPressManager.stateDuration);
        }

        public void OnPressUpUpdate()
        {
            speedDown.DecreaseTime();
            speedUp.IncreaseTime();

            apply_internal_speedMultiplier();
            Move();
        }

        public PlayerInfo GetPlayerInfo()
        {
            return playerInfo;
        }

        public Vector3 GetBulletPositionInWorld()
        {
            return transform.position + transform.up * bulletPosOffset.y + transform.right * bulletPosOffset.x;
        }


        private void Start()
        {
            this.Initialize();
            speedDown.Init();
            speedUp.Init();
        }

        private void apply_internal_speedMultiplier()
        {
            rotateSpeedMultiplier_internal = speedDown.Evaluate() * speedUp.Evaluate();
        }

        private void Move()
        {
            transform.RotateAround(pivot.transform.position, Vector3.forward,
                rotateSpeed * rotateSpeedMultiplier_internal * rotateSpeedMultiplier * Time.deltaTime);
        }

        private void Shoot()
        {
            Instantiate(playerNormalBulletPrefab, GetBulletPositionInWorld(), transform.rotation).Init(playerInfo);
        }

        [Serializable]
        public abstract class Curve
        {
            public AnimationCurve curve;
            [HideInInspector] public float last_key_time;
            protected float time;


            public abstract void IncreaseTime();
            public abstract void DecreaseTime();

            public float Evaluate()
            {
                return curve.Evaluate(time);
            }

            public void Init()
            {
                last_key_time = curve.keys[curve.keys.Length - 1].time;
            }
        }

        [Serializable]
        public class SpeedDownCurve : Curve
        {
            public float speedDown = 1;
            public float speedUpLerp = 1;


            public override void IncreaseTime()
            {
                time = Mathf.Lerp(time, last_key_time, Time.deltaTime * speedUpLerp);
            }

            public override void DecreaseTime()
            {
                time -= Time.deltaTime * speedDown;
                if (time < 0) time = 0;
            }
        }

        [Serializable]
        public class SpeedUpCurve : Curve
        {
            public float speedDownLerp = 2;
            public float speedUp = 1;

            public override void DecreaseTime()
            {
                time = Mathf.Lerp(time, 0, Time.deltaTime * speedDownLerp);
            }

            public override void IncreaseTime()
            {
                time += Time.deltaTime * speedUp;
                if (time > last_key_time)
                    time = last_key_time;
            }
        }



    }
}
