using Gameplay.CoinsSystem;
using UnityEngine;

namespace Gameplay.EnemyNamespace.Types.Trig
{
    public class EnemyBaseTrig : EnemyBase
    {
        private Vector3 targetPosition;

        #region caches

        private new Transform transform;

        #endregion

        protected void Awake()
        {
            // caching
            transform = gameObject.transform;

        }

        protected override void OnInit()
        {
            // set up values
            Health = Mathf.Ceil(points * 0.01f);

            targetPosition = FindObjectOfType<Player.PlayerInfo>(true).parts.pivot.transform.position;

            RoateTowrardsTarget();
        }

        protected override void OnSetHealth(ref float m_health)
        {
            if (m_health <= 0)
            {
                m_health = 0;
                DestroyEnemy();
            }
        }

        private void RoateTowrardsTarget()
        {
            transform.up = targetPosition - transform.position;
        }

        private void Update()
        {
            MoveTowardsTarget();
        }

        private void MoveTowardsTarget()
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log($"{name} colliding with {other.gameObject.name}");
            if (other.gameObject.TryGetComponent<Player.PlayerInfo>(out var playerInfo))
            {
                var damageInfo = new Player.EnemyDamageInfo(
                    damage);
                playerInfo.TakeDamage(damageInfo);

                DestroyEnemy();
            }
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

        #region settings

        public float speed = 0.2f;
        public float damage = 1;

        #endregion

    }

}
