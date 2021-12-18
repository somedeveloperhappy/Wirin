using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Player
{
    public abstract class PlayerInfo : MonoBehaviour, IOnPlayerPress, IOnGameplayEnd
    {
        public static List<PlayerInfo> instances = new List<PlayerInfo>();

        public UpgradeSystem.MoneyManager moneyManager;
        public UpgradeSystem.UpgradeManager upgradeManager;
        public Parts parts;

        [SerializeField]
        [InspectorName( "HealthStats" )]
        private HealthStats m_stats;

        [SerializeField]
        [InspectorName( "Shootings" )]
        private Shootings m_shootings;


        private float maxcharge; // maximum possible charge
        private float maxfineCharge; // the maximun charge in the shootCharge where T is fine
        private float maxpressT; // the maximun t in the shootCharge



        private float pressT;

        public void OnPressDown(float duration) { }
        public void OnPressUp(float duration) { }

        public void OnPressDownUpdate()
        {
            pressT += Time.deltaTime;
            if (pressT > maxpressT) pressT = maxpressT;
        }

        public void OnPressUpUpdate()
        {
            pressT = Mathf.Lerp( pressT, 0, Time.deltaTime * m_shootings.chargeDownLerp );
            if (pressT < 0.001) pressT = 0;
        }
        public Shootings GetShootings()
        {
            return m_shootings;
        }

        public void TakeDamage(EnemyDamageInfo damageinfo)
        {
            SetHealth( GetHealth() - damageinfo.damage );
            Debug.Log( $"taking {damageinfo.damage} damage to player. player health now {GetHealth()}" );
        }

        private void Awake()
        {
            instances.Add( this );

            maxpressT = m_shootings.shootCharge.keys[m_shootings.shootCharge.keys.Length - 1].time;
            maxcharge = m_shootings.shootCharge.Evaluate( maxpressT );
            maxfineCharge = m_shootings.shootCharge.Evaluate( m_shootings.maxFineT );

            m_stats.m_health = m_stats.maxHealth;

        }

        #region health things

        [Serializable]
        public class HealthStats
        {

            public delegate void OnHealthChanged(float newHealth, float previousHealth);
            public float m_health;
            public float maxHealth;
        }
        /// <summary>
        ///     called after health changed
        /// </summary>
        public HealthStats.OnHealthChanged onHealthChanged;

        public float GetMaxHealth() => m_stats.maxHealth;
        public float GetHealth() => m_stats.m_health;
        public void SetHealth(float value)
        {
            // check if player is already dead
            if (m_stats.m_health == 0) return;

            var prevHealth = m_stats.m_health;

            if (value < 0) value = 0;
            m_stats.m_health = value;

            if (value == 0) StartCoroutine(Lose());
            onHealthChanged?.Invoke( value, prevHealth );
        }

        #endregion

        public IEnumerator Lose()
        {
            Debug.Log( $"Player {name} is Lost" );
            References.gameController.DisableAllGameplayMechanics( timeScaleTo0: false, stopInputingAbruptly: true );
            yield return StartCoroutine( OnLose() );
            References.gameController.OnLoseLevel();
        }
        protected abstract IEnumerator OnLose();

        [Serializable]
        public class Shootings
        {
            [FormerlySerializedAs( "bulletPrefab" )] public PlayerNormalBullet playerNormalBulletPrefab;
            public float chargeDownLerp = 5;
            [Tooltip( "x where Y is 1" )] public float maxFineT;
            public AnimationCurve shootCharge;
        }

        [Serializable]
        public class Parts
        {
            public Pivot pivot;
            public Trinon trinon;
        }


        #region helper functions

        /// <returns>the current charge. not guaranteed to be between any two numbers</returns>
        public float GetRawCharge()
        {
            return m_shootings.shootCharge.Evaluate( pressT );
        }

        /// <returns>between 0 and 1, and it won't take too much time</returns>
        public float GetNormalCharge()
        {
            return m_shootings.shootCharge.Evaluate(
                pressT > m_shootings.maxFineT ? m_shootings.maxFineT : pressT
            ) / maxfineCharge;
        }

        /// <returns>the maximum charge time</returns>
        public float GetMaxPossibleChargeTime()
        {
            return maxpressT;
        }

        public float GetMaxPossibleCharge()
        {
            return maxcharge;
        }

        public bool CanAfford(uint cost) => moneyManager.Coins >= cost;

        #endregion
        public virtual void OnGameplayEnd()
        {
            // reset health
            m_stats.m_health = m_stats.maxHealth;
            onHealthChanged?.Invoke( m_stats.m_health, m_stats.m_health ); // for effects

            // transforms
            //parts.trinon.transform.rotation = Quaternion.identity;


            // effects
            pressT = 0;
        }


        private void OnEnable()
        {
            this.OnPlayerPressInit();
            this.ResettableInit();
        }
        private void OnDisable()
        {
            this.OnPlayerPressDestroy();
            this.ResettableDestroy();
        }


    }

    public interface IPlayerPart
    {
        public PlayerInfo GetPlayerInfo();
    }
}
