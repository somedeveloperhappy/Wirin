using UnityEngine;

public class PlayerInfo : MonoBehaviour, IOnPlayerPress
{
	static public System.Collections.Generic.List<PlayerInfo> instances =
		new System.Collections.Generic.List<PlayerInfo>();

	[System.Serializable]
	public class Stats
	{
		float m_health;

		public float Health
		{
			get => m_health;
			set
			{
				if (m_health == value) return;
				bool wasLess = value < m_health;
				m_health = value;
				if (m_health < 0) onHealthLessThanZero?.Invoke();
				onHealthChanged?.Invoke(m_health, wasLess ? HealthChangedType.Decrease : HealthChangedType.Increase);
			}
		}

		public enum HealthChangedType
		{
			Increase,
			Decrease
		}

		public delegate void OnHealthChanged(float newHealth, HealthChangedType type);

		/// <summary>
		/// called after health changed
		/// </summary>
		public OnHealthChanged onHealthChanged;

		public float maxHealth;

		public System.Action onHealthLessThanZero;
	}

	[System.Serializable]
	public class Shootings
	{
		public AnimationCurve shootCharge;
		[Tooltip("x where Y is 1")] public float maxFineT;
		public float chargeDownLerp = 5;
		public Bullet bulletPrefab;
	}

	[SerializeField] [InspectorName("Stats")]
	Stats m_stats;

	public Stats GetStats() => m_stats;

	[SerializeField] [InspectorName("Shootings")]
	Shootings m_shootings;

	public Shootings GetShootings() => m_shootings;

	[System.Serializable]
	public class Parts
	{
		public Pivot pivot;
		public Trinon trinon;
	}

	public Parts parts;

	public void TakeDamage(EnemyDamageInfo damageinfo)
	{
		m_stats.Health -= damageinfo.damage;
		Debug.Log($"taking {damageinfo.damage} damage to player. player health now {m_stats.Health}");
	}


	float pressT;
	float maxfineCharge; // the maximun charge in the shootCharge where T is fine
	float maxpressT; // the maximun t in the shootCharge
	float maxcharge; // maximum possible charge


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


#region helper functions

	/// <returns>the current charge. not guaranteed to be between any two numbers</returns>
	public float GetRawCharge() => m_shootings.shootCharge.Evaluate(pressT);

	/// <returns>between 0 and 1, and it won't take too much time</returns>
	public float GetNormalCharge()
	{
		return m_shootings.shootCharge.Evaluate(
			pressT > m_shootings.maxFineT ? m_shootings.maxFineT : pressT
		) / maxfineCharge;
	}

	/// <returns>the maximum charge time</returns>
	public float GetMaxPossibleChargeTime() => maxpressT;

	public float GetMaxPossibleCharge() => maxcharge;

#endregion


}

public interface IPlayerPart
{
	public PlayerInfo GetPlayerInfo();
}