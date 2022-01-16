using Gameplay.CoinsSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.EnemyNamespace.Types
{
    public abstract class EnemyBase : MonoBehaviour, IOnGameplayEnd
    {
        public static List<EnemyBase> instances = new List<EnemyBase>();
        private float m_health;

        protected int points;
        public int Points => points;
        public CoinsSystem.CoinSpawner coinSpawner;

        /// <returns>the current health for this enemyBase</returns>
        public float Health
        {
            get
            {
                OnGetHealth(ref m_health);
                return m_health;
            }
            set
            {
                m_health = value;
                OnSetHealth(ref m_health);
            }
        }

        protected virtual void OnGetHealth(ref float health) { }
        protected virtual void OnSetHealth(ref float health) { }


        public void Init(int points)
        {
            this.points = points;
            OnInit();
        }

        protected abstract void OnInit();


        private void OnEnable()
        {
            instances.Add(this);
            this.ResettableInit();
        }
        protected virtual void OnDisable()
        {
            instances.Remove(this);
            this.ResettableDestroy();
        }

        public void DestroyEnemy()
        {
            SpawnCoins(coinSpawner.GetCoins((uint)points));

            Destroy(gameObject);
            References.levelManager.OnEnemyDestroy(this);
            AfterDestruction();
            onDestroy?.Invoke();
        }

        protected abstract void SpawnCoins((Coin coin, int amount)[] coinCases);

        protected virtual void AfterDestruction() { }

        public void TakeDamage(Player.PlayerBulletDamageInfo damageInfo)
        {
            var health_before = Health;
            Health -= damageInfo.damage;
            OnTakeDamage();
            onTakeDamage?.Invoke(health_before, damageInfo);
        }

        protected virtual void OnTakeDamage() { }

        public void OnGameplayEnd()
        {
            // silent destruction
            Destroy(gameObject);
        }

        #region events

        public event Action<float, Player.PlayerBulletDamageInfo> onTakeDamage;
        public event Action onDestroy;

        #endregion

    }
}
