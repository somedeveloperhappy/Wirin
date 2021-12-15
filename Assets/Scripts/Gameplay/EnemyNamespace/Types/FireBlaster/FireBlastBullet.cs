using Gameplay.Bullets;
using UnityEngine;

namespace Gameplay.EnemyNamespace.Types.FireBlaster
{
    public class FireBlastBullet : BulletBase
    {

        #region editor settings
        [SerializeField] private float boundryRange = 2;
        private const int CHECK_FOR_SCREEN_BOUND_T = 1;
        private float last_screen_bound_check;
        #endregion

        [HideInInspector] public float damage;
        [HideInInspector] public float speed;

        [HideInInspector] public Cannon cannon;

        #region events
        public System.Action onInit;
        public System.Action onHit;
        #endregion


        Rigidbody2D rigid;

        public void Init(Cannon cannon)
        {
            this.cannon = cannon;

            damage = cannon.bulletSettings.damage;
            speed = cannon.bulletSettings.buletSpeed;

            onInit?.Invoke();
        }

        protected override void Awake()
        {
            base.Awake();
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
            Debug.Log($"hitting {other.gameObject.name}");
            if (other.gameObject.TryGetComponent<Player.PlayerInfo>(out Player.PlayerInfo player))
            {
                Debug.Log($"Damaging player: {player.name} , damage : {damage}");
                player.TakeDamage(new Player.EnemyDamageInfo(damage));

                DestroyBullet();
            }
        }

        private void DestroyBullet()
        {
            Destroy(gameObject);
            onHit?.Invoke();
        }
    }
}
