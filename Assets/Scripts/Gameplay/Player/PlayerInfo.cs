using UnityEngine;

public class PlayerInfo : MonoBehaviour, IOnPlayerPress
{
	[System.Serializable]
	public class Stats
	{
		public float health;
	}

	[System.Serializable]
	public class Shootings
	{
		public AnimationCurve shootCharge;
        [Tooltip("x where Y is 1")]
		public float maxFineT;
		public float chargeDownLerp = 5;
		public Bullet bulletPrefab;
	}

	[SerializeField]
	[InspectorName ("Stats")]
	Stats m_stats;
	public Stats GetStats() => m_stats;

	[SerializeField]
	[InspectorName ("Shootings")]
	Shootings m_shootings;
	public Shootings GetShootings() => m_shootings;

	public void TakeDamage(EnemyDamageInfo damageinfo)
	{
		Debug.Log ($"taking damage to player...");
		m_stats.health -= damageinfo.damage;
	}

	float pressT;
    float maxfineCharge;   // the maximun charge in the shootCharge where T is fine
    float maxpressT; // the maximun t in the shootCharge
    float maxcharge; // maximum possible charge


	private void Awake()
	{
		this.Initialize ();
		maxpressT = m_shootings.shootCharge.keys[m_shootings.shootCharge.keys.Length - 1].time;
        maxcharge = m_shootings.shootCharge.Evaluate(maxpressT);
		maxfineCharge = m_shootings.shootCharge.Evaluate (m_shootings.maxFineT);
	}

	public void OnPressDown(float duration) { }
	public void OnPressUp(float duration) { }

	public void OnPressDownUpdate()
	{
		pressT += Time.deltaTime;
        if(pressT > maxpressT) pressT = maxpressT;
	}

	public void OnPressUpUpdate()
	{
		pressT = Mathf.Lerp(pressT, 0, Time.deltaTime * m_shootings.chargeDownLerp);
		if (pressT < 0.001) pressT = 0;
	}


#region helper functions
	/// <returns>the current charge. not guaranteed to be between any two numbers</returns>
	public float GetRawCharge() => m_shootings.shootCharge.Evaluate (pressT);
    
    /// <returns>between 0 and 1, and it won't take too much time</returns>
    public float GetNormalCharge()
    {
		return m_shootings.shootCharge.Evaluate (
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
