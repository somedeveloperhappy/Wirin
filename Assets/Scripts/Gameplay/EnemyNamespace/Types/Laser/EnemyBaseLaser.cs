using Gameplay.CoinsSystem;
using UnityEngine;

namespace Gameplay.EnemyNamespace.Types.Laser
{
    public class EnemyBaseLaser : EnemyBase
    {
        [System.Serializable]
        public struct Settings
        {
            public float speed;
            public float laserDistance;
            public SimpleScripts.MinMax damageRange, damageRateRange;
        }
        public Settings settings;

        #region refs
        public Laser[] lasers;
        #endregion

        #region  caches
        Vector3 target;
        private float nextDamageRate, nextDamage;
        #endregion
        bool is_close = false; // is close to target

        protected override void OnInit()
        {
            Health = Mathf.Ceil(points / 200f);
            target = References.playerInfo.transform.position;
            RotateTowardsTarget();

            ReevaluateDamage();
        }

        private void ReevaluateDamage()
        {
            nextDamage = settings.damageRange.Evaluate(Random.Range(0, 1f));
            nextDamageRate = settings.damageRateRange.Evaluate(Random.Range(0, 1f));
        }

        protected override void OnSetHealth(ref float health)
        {
            base.OnSetHealth(ref health);
            if (health <= 0)
            {
                health = 0;
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

        private void Update()
        {
            if (is_close)
            {
                UpdateShoot();
            }
            else
            {
                MoveTowardsTarget();
                if (Vector2.Distance(transform.position, target) <= settings.laserDistance)
                {
                    is_close = true;
                    startShootProcess();
                }
            }
        }

        private void startShootProcess()
        {
            foreach(var laser in lasers) laser.StartShooting();
        }
        private void StopShootProcess()
        {
            foreach(var laser in lasers) laser.StopShooting();
        }

        float shoot_t = 0;
        private void UpdateShoot()
        {
            shoot_t += Time.deltaTime;
            if (shoot_t >= nextDamageRate)
            {
                shoot_t = 0;
                Shoot();
            }
        }

        private void Shoot()
        {
            References.playerInfo.TakeDamage(new Player.EnemyDamageInfo(nextDamage));
            ReevaluateDamage();
        }

        private void RotateTowardsTarget() => transform.up = target - transform.position;
        private void MoveTowardsTarget() => transform.position += transform.up * settings.speed * Time.deltaTime;



    }
}