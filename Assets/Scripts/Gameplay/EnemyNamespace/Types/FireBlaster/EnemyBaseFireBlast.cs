using Gameplay.CoinsSystem;
using UnityEngine;

namespace Gameplay.EnemyNamespace.Types.FireBlaster
{
    public class EnemyBaseFireBlast : EnemyBase
    {

        [System.Serializable]
        public class Settings
        {
            public float distanceToShoot = 10;
            public float speed = 0.2f;

            [System.Serializable]
            public class CannonSettings
            {
                public Cannon cannon;
                public float fireRate = 1;
                [Tooltip("The time used to check if it can shoot. change it for fire rate offset")]
                public float time = 0;
            }
            public CannonSettings[] cannonSettings;
        }

        public Settings settings;

        #region caches
        private new Transform transform;
        #endregion

        private Transform m_target_transform;
        [HideInInspector] public Vector3 target;

        protected void Awake()
        {
            transform = ((Component)this).transform; // caching transform
        }

        protected override void OnInit()
        {
            Health = points * 0.2f;
            m_target_transform = FindObjectOfType<Player.PlayerInfo>().transform;
            target = m_target_transform.position;
            RotateTowardsTarget();
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, target) > settings.distanceToShoot)
            {
                MoveTowardsTarget();
            }
            else
            {
                // shoot
                for (int i = 0; i < settings.cannonSettings.Length; i++)
                {
                    Settings.CannonSettings cannonSetting = settings.cannonSettings[i];
                    if (!cannonSetting.cannon.canAim) cannonSetting.cannon.canAim = true;

                    cannonSetting.time %= cannonSetting.fireRate;
                    cannonSetting.time += Time.deltaTime;
                    if (cannonSetting.time >= cannonSetting.fireRate)
                    {
                        cannonSetting.cannon.Shoot();
                    }
                }
            }

        }


        protected override void OnSetHealth(ref float health)
        {
            if (health <= 0)
            {
                health = 0;
                DestroyEnemy();
            }
        }

        private void MoveTowardsTarget()
        {
            transform.position += transform.up * settings.speed * Time.deltaTime;
        }

        private void RotateTowardsTarget()
        {
            transform.up = target - transform.position;
        }

        protected override void SpawnCoins((Coin coin, int amount)[] ps)
        {
            var playerInfo = FindObjectOfType<Player.PlayerInfo>();

            var dist = 0.5f;
            for (int i = 0; i < ps.Length; i++)
            {
                for (int j = 0; j < ps[i].amount; j++)
                {
                    var c = Instantiate(
                        original: ps[i].coin,
                        position: transform.position + new Vector3(
                            Random.Range(-dist, dist),
                            Random.Range(-dist, dist),
                            transform.position.z),
                        rotation: Quaternion.identity);
                    c.Init(playerInfo, ps[i].coin.coinsWorth);
                }
            }
        }
        // protected override void OnTakeDamage()
        // {
        // 	base.OnTakeDamage ();
        // }
    }
}
