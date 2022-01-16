using SimpleScripts;
using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Player
{
    public abstract class PlayerInfo : MonoBehaviour, IOnPlayerPress, IOnGameplayEnd
    {
        public UpgradeSystem.MoneyManager moneyManager;
        public UpgradeSystem.UpgradeManager upgradeManager;
        public Parts parts;

        [SerializeField]
        [InspectorName( "HealthStats" )]
        private HealthStats m_stats;

        [SerializeField]
        private Shootings m_shootings;

        [Serializable]
        public class Shootings
        {
            public PlayerNormalBullet playerNormalBulletPrefab;
            [Tooltip( "time to charge at maximum" )] public float maxChargeTime;
            public float chargeDownLerp = 5;

            /// <returns>almost-smoothstep function used for shooting charge</returns>
            public float EvaluateShootPower01(float value)
            {
                // between 0 and 1
                float x = Mathf.Clamp( value, 0, maxChargeTime ) / maxChargeTime;

                // evaluated to amost-smoothstep between 0 and 1
                return x < 0.5f ? f( x ) : g( x );

                float f(float x) => Mathf.Pow( 2 * x, 3 ) / 2f;
                float g(float x) => -f( -x + 1 ) + 1;
            }

            [Space( 10 )]
            public uint trinonMaxCount;
            internal uint trinonCount = 1;
            internal bool canModifyTrinonCount = true;
            public MinMax trinonDistance;
        }

        [Serializable]
        public class Parts
        {
            public Pivot pivot;
            [HideInInspector]
            public System.Collections.Generic.List<Trinon> trinons = new System.Collections.Generic.List<Trinon>();
            public Trinon mainTrinon;
            public Transform trinonsParent;
        }

        /// <summary>
        /// all values in Degree
        /// </summary>
        [System.Serializable]
        public class RotationSettings
        {
            public float speedUpAcceleration = 1;
            public float breakLerpSpeed = 10;
            public float maxSpeed = 10;
            [HideInInspector] internal float velocity_multiplier = 1;

            [HideInInspector] float _direction;
            public float Direction => _direction;
            public float DirectionRad => _direction * Mathf.Deg2Rad;
            [HideInInspector] public float velocity;
            public void ApplyVelocity()
            {
                Debug.Log($"{velocity} * {velocity_multiplier} * {Time.deltaTime}");
                _direction += velocity * velocity_multiplier * Time.deltaTime;
            }
        }
        [SerializeField] private RotationSettings m_rotationSettings;

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

            if (value == 0) StartCoroutine( Lose() );
            onHealthChanged?.Invoke( value, prevHealth );
        }

        public void SetMaxHealth(float maxhealth)
        {
            Debug.Log( $"Max health is set to {maxhealth}" );

            m_stats.maxHealth = maxhealth;
            m_stats.m_health = m_stats.maxHealth;
        }
        #endregion

        #region helper functions
        public void SetBreakLerpSpeed(float value) => m_rotationSettings.breakLerpSpeed = value;
        public float GetBreakLerpSpeed(float value) => m_rotationSettings.breakLerpSpeed;
        public void SetVelocityMultiplier(float value) => m_rotationSettings.velocity_multiplier = value;
        public Vector2 Direction => new Vector2( -Mathf.Sin( m_rotationSettings.DirectionRad ), Mathf.Cos( m_rotationSettings.DirectionRad ) );
        public uint TrinonCount => m_shootings.trinonCount;
        public bool CanAddTrinon() => m_shootings.canModifyTrinonCount && m_shootings.trinonCount < m_shootings.trinonMaxCount;
        public bool CanRemoveTrinon() => m_shootings.canModifyTrinonCount && m_shootings.trinonCount > 1;

        /// <returns>between 0 and 1, and it won't take too much time</returns>
        public float GetNormalCharge() => m_shootings.EvaluateShootPower01( pressT );

        /// <returns>the maximum charge time</returns>
        public float GetMaxChargeTime() => m_shootings.maxChargeTime;
        public Shootings GetShootings() => m_shootings;
        public bool CanAfford(uint cost) => moneyManager.Coins >= cost;

        #endregion


        private float pressT;

        private void Awake()
        {
            m_stats.m_health = m_stats.maxHealth;

            // load money
            moneyManager.Load();

            // assign main single trinon as all there is so far
            parts.trinons.Add( parts.mainTrinon );
        }

        private void Update()
        {
            m_rotationSettings.ApplyVelocity();
            MoveTrinons();
        }

        #region on press shit
        public void OnPressDown(float duration) { }
        public void OnPressUp(float duration)
        {
            Shoot();
        }

        public void OnPressDownUpdate()
        {
            pressT += Time.deltaTime;
            if (pressT > GetMaxChargeTime()) pressT = GetMaxChargeTime();
            // apply break
            m_rotationSettings.velocity = Mathf.Lerp( m_rotationSettings.velocity, 0, m_rotationSettings.breakLerpSpeed * Time.deltaTime );
        }

        public void OnPressUpUpdate()
        {
            pressT = Mathf.Lerp( pressT, 0, Time.deltaTime * m_shootings.chargeDownLerp );
            if (pressT < 0.001) pressT = 0;

            // apply speed up
            m_rotationSettings.velocity += m_rotationSettings.speedUpAcceleration * Time.deltaTime;
            if (m_rotationSettings.velocity > m_rotationSettings.maxSpeed) m_rotationSettings.velocity = m_rotationSettings.maxSpeed;
        }
        #endregion

        public void TakeDamage(EnemyDamageInfo damageinfo)
        {
            SetHealth( GetHealth() - damageinfo.damage );
            Debug.Log( $"taking {damageinfo.damage} damage to player. player health now {GetHealth()}" );
        }


        #region trinon shit
        void MoveTrinons()
        {
            // rotate the shared trinons parents
            parts.trinonsParent.up = new Vector2( -Mathf.Sin( m_rotationSettings.DirectionRad ), Mathf.Cos( m_rotationSettings.DirectionRad ) );

            // rotate individual trinons
            for (int i = 0; i < m_shootings.trinonCount; i++)
            {
                // if not enabled, let it be
                if (!parts.trinons[i].enabled) continue;

                var dir = GetLocalDirectionForTrinonOfIndex( i );
                var rot = parts.trinons[i].transform.localEulerAngles;
                rot.z = dir;
                parts.trinons[i].transform.localEulerAngles = rot;
            }
        }
        protected float GetLocalDirectionForTrinonOfIndex(int index)
        {
            if (m_shootings.trinonCount == 1) return 0;
            return index * m_shootings.trinonDistance.Evaluate( GetNormalCharge() );
        }
        public void RemoveLastTrinon()
        {
            if (!CanRemoveTrinon()) return;

            var trinon = parts.trinons[parts.trinons.Count - 1];

            // fix functionalies
            trinon.enabled = false;
            m_shootings.trinonCount--;
            parts.trinons.RemoveAt( parts.trinons.Count - 1 );
            m_shootings.canModifyTrinonCount = false;
            UpdateTrinonPowerMultiplers();

            // make animation and effects ready
            OnRemoveLastTrinon( trinon, () =>
             {
                 Debug.Log( $"Finished removing a trion" );
                 m_shootings.canModifyTrinonCount = true;
                 Destroy( trinon.gameObject );
             } );
        }
        protected abstract void OnRemoveLastTrinon(Trinon trinon, Action onMethodEnd);
        public void InstantiacteNewTrinon()
        {
            if (!CanAddTrinon()) return;

            var new_trinon = Instantiate(
                original: parts.mainTrinon,
                position: parts.mainTrinon.transform.position,
                rotation: parts.mainTrinon.transform.rotation,
                parent: parts.trinonsParent );

            m_shootings.trinonCount++;
            parts.trinons.Add( new_trinon );

            // disable trinon functionalities. for incomming animation
            new_trinon.enabled = false;
            Debug.Log( "Trinon adding..." );
            m_shootings.canModifyTrinonCount = false;
            UpdateTrinonPowerMultiplers();

            OnAddingNewTrinon( new_trinon, parts.trinons.Count - 1, () =>
             {
                 new_trinon.enabled = true;
                 m_shootings.canModifyTrinonCount = true;
                 Debug.Log( $"A trinon added! total of {parts.trinons.Count} of those creeps now exists" );
             } );
        }
        protected abstract void OnAddingNewTrinon(Trinon trinon, int index, Action onMethodEnd);
        private void UpdateTrinonPowerMultiplers()
        {
            foreach (var trinon in parts.trinons)
                trinon.bulletDamageMultiplier = 1f / parts.trinons.Count;
        }
        #endregion

        #region shooting shit
        private void Shoot()
        {
            // all trinons should shoot 
            parts.trinons.ForEach( trinon => trinon.Shoot( m_shootings.playerNormalBulletPrefab ) );
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

        public virtual void OnGameplayEnd()
        {
            // reset health
            m_stats.m_health = m_stats.maxHealth;
            onHealthChanged?.Invoke( m_stats.m_health, m_stats.m_health ); // for effects

            // trinon rotation
            m_rotationSettings.velocity = 0;
            parts.trinonsParent.localRotation = Quaternion.identity;

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
}
