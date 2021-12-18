using Gameplay.Bullets;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerNormalBullet : BulletBase
    {

        #region main settings

        [SerializeField, Tooltip("T between 0 and 1")]
        private AnimationCurve damageCurve;

        [SerializeField, Tooltip("T between 0 and 1")]
        private AnimationCurve speedCurve;

        [SerializeField, Tooltip("T between 0 and 1")]
        private AnimationCurve stunCurve;

        #endregion

        #region editor settings

        [SerializeField] private float boundryRange = 2;
        private const int CHECK_FOR_SCREEN_BOUND_T = 1;
        private float last_screen_bound_check;

        #endregion

        public float damage { get; private set; }
        public float damageMultiplier = 1;

        #region refs

        [HideInInspector] public PlayerInfo playerInfo;

        #endregion

        #region events

        public delegate void OnInit(float normalizedT);

        /// <summary>
        ///     executes on initializing it, T normalized
        /// </summary>
        public OnInit onInit_fine;

        /// <summary>
        ///     executes on initializing it, T not normalized
        /// </summary>
        public OnInit onInit_raw;

        public delegate void OnHit(float damage);

        /// <summary>
        ///     happens on damage
        /// </summary>
        public OnHit onHit;

        #endregion


        private Rigidbody2D rigid;
        public float speed;
        public float stunDuration;

        public void Init(PlayerInfo playerInfo)
        {
            this.playerInfo = playerInfo;
            var raw_charge = playerInfo.GetRawCharge();
            var normal_charge = raw_charge / playerInfo.GetMaxPossibleCharge();


            damage = damageCurve.Evaluate(normal_charge) * damageMultiplier;
            speed = speedCurve.Evaluate(normal_charge);
            stunDuration = stunCurve.Evaluate(normal_charge);

            onInit_fine?.Invoke(normal_charge);
            onInit_raw?.Invoke(raw_charge);
        }

        protected void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            transform.position += transform.up * speed * Time.fixedDeltaTime;

            checkForScreenBound();
        }

        private void checkForScreenBound()
        {
            if (Time.timeSinceLevelLoad - last_screen_bound_check > CHECK_FOR_SCREEN_BOUND_T)
            {
                last_screen_bound_check = Time.timeSinceLevelLoad;

                Vector2 pos_in_screen = References.currentCamera.WorldToScreenPoint(transform.position);

                if (pos_in_screen.x + boundryRange < 0 ||
                    pos_in_screen.x - boundryRange > Screen.width ||
                    pos_in_screen.y + boundryRange < 0 ||
                    pos_in_screen.y - boundryRange > Screen.height)
                {
                    // it's out of screen
                    Destroy(gameObject);
                }
            }
        }


        public void OnCollide(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<EnemyNamespace.Types.EnemyBase>(out EnemyNamespace.Types.EnemyBase enemy))
            {
                Debug.Log($"Damaging enemy {enemy.name} , damage : {damage}");
                enemy.TakeDamage(new PlayerBulletDamageInfo(damage, stunDuration));

                DestroyBullet();
            }
        }

        private void DestroyBullet()
        {
            Destroy(gameObject);
            onHit?.Invoke(damage);
        }


    }
}
