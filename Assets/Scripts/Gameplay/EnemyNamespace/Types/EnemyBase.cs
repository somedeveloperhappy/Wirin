using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.EnemyNamespace.Types
{
    public abstract class EnemyBase : MonoBehaviour
    {
        public static List<EnemyBase> instances = new List<EnemyBase>();
        private float m_health;

        public int points;

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

        protected virtual void Awake()
        {
            instances.Add(this);
        }


        protected virtual void OnDestroy()
        {
            instances.Remove(this);
        }

        public void DestroyEnemy()
        {
            Destroy(gameObject);
            References.levelManager.OnEnemyDestroy(this);
            AfterDestruction();
            onDestroy?.Invoke();
        }

        protected virtual void AfterDestruction() { }

        public void TakeDamage(Player.PlayerBulletDamageInfo damageInfo)
        {
            var health_before = Health;
            Health -= damageInfo.damage;
            OnTakeDamage();
            onTakeDamage?.Invoke(health_before, damageInfo);
        }

        protected virtual void OnTakeDamage() { }

        #region events

        public event Action<float, Player.PlayerBulletDamageInfo> onTakeDamage;
        public event Action onDestroy;

        #endregion

    }
}
