using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Gameplay.Player
{
	public class PlayerInfo : MonoBehaviour, IOnPlayerPress
	{
		public static List<PlayerInfo> instances =
			new List<PlayerInfo>();

		[SerializeField] [InspectorName("Shootings")]
		private Shootings m_shootings;

		[SerializeField] [InspectorName("Stats")]
		private Stats m_stats;

		private float maxcharge; // maximum possible charge
		private float maxfineCharge; // the maximun charge in the shootCharge where T is fine
		private float maxpressT; // the maximun t in the shootCharge

		public Parts parts;


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
			pressT = Mathf.Lerp(pressT, 0, Time.deltaTime * m_shootings.chargeDownLerp);
			if (pressT < 0.001) pressT = 0;
		}

		public Stats GetStats()
		{
			return m_stats;
		}

		public Shootings GetShootings()
		{
			return m_shootings;
		}

		public void TakeDamage(EnemyDamageInfo damageinfo)
		{
			m_stats.Health -= damageinfo.damage;
			Debug.Log($"taking {damageinfo.damage} damage to player. player health now {m_stats.Health}");
		}


		private void Awake()
		{
			instances.Add(this);
			this.Initialize();
			maxpressT = m_shootings.shootCharge.keys[m_shootings.shootCharge.keys.Length - 1].time;
			maxcharge = m_shootings.shootCharge.Evaluate(maxpressT);
			maxfineCharge = m_shootings.shootCharge.Evaluate(m_shootings.maxFineT);

			m_stats.Health = m_stats.maxHealth;

			// prepairing a lost situation 
			m_stats.onHealthLessThanZero += () => StartCoroutine(References.levelManager.LostLevel());
		}

		[Serializable]
		public class Stats
		{

			public delegate void OnHealthChanged(float newHealth, HealthChangedType type);

			public enum HealthChangedType
			{
				Increase,
				Decrease
			}

			private float m_health;

			public float maxHealth;

			/// <summary>
			///     called after health changed
			/// </summary>
			public OnHealthChanged onHealthChanged;

			public Action onHealthLessThanZero;

			public float Health
			{
				get => m_health;
				set
				{
					if (m_health == value) return;
					var wasLess = value < m_health;
					m_health = value;
					if (m_health < 0) onHealthLessThanZero?.Invoke();
					onHealthChanged?.Invoke(m_health, wasLess ? HealthChangedType.Decrease : HealthChangedType.Increase);
				}
			}
		}

		[Serializable]
		public class Shootings
		{
			[FormerlySerializedAs("bulletPrefab")] public PlayerNormalBullet playerNormalBulletPrefab;
			public float chargeDownLerp = 5;
			[Tooltip("x where Y is 1")] public float maxFineT;
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
			return m_shootings.shootCharge.Evaluate(pressT);
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

#endregion

	}

	public interface IPlayerPart
	{
		public PlayerInfo GetPlayerInfo();
	}
}