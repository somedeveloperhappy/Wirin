using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
#region main settings

	[SerializeField, Tooltip("T between 0 and 1")] AnimationCurve damageCurve;
	[SerializeField, Tooltip("T between 0 and 1")] AnimationCurve speedCurve;
	[SerializeField, Tooltip("T between 0 and 1")] AnimationCurve stunCurve;

#endregion

#region refs

	public PlayerInfo playerInfo;

#endregion

	public float damage;
	public float speed;
	public float stunDuration;
	
#region events
	public delegate void OnInit(float normalizedT);
	/// <summary>
	/// executes on initializing it, T normalized
	/// </summary>
	public OnInit onInit_fine;
	/// <summary>
	/// executes on initializing it, T not normalized
	/// </summary>
	public OnInit onInit_raw;
	public delegate void OnHit(float damage);
	/// <summary>
	/// happens on damage
	/// </summary>
	public OnHit onHit;
	
#endregion

	public void Init(PlayerInfo playerInfo)
	{
		this.playerInfo = playerInfo;
		var raw_charge = playerInfo.GetRawCharge();
		var normal_charge = raw_charge / playerInfo.GetMaxPossibleCharge ();
		

		damage = damageCurve.Evaluate (normal_charge);
		speed = speedCurve.Evaluate (normal_charge);
		stunDuration = stunCurve.Evaluate (normal_charge);
		
		onInit_fine?.Invoke(normal_charge);
		onInit_raw?.Invoke(raw_charge);
	}

	#region editor settings

	[SerializeField] float boundryRange = 2;
	const int CHECK_FOR_SCREEN_BOUND_T = 1;
	float last_screen_bound_check = 0;

	#endregion

	Rigidbody2D rigid;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D> ();
	}

	private void FixedUpdate()
	{
		transform.position += transform.up * speed * Time.fixedDeltaTime;

		checkForScreenBound ();
	}

	private void checkForScreenBound()
	{
		if (Time.timeSinceLevelLoad - last_screen_bound_check > CHECK_FOR_SCREEN_BOUND_T)
		{
			last_screen_bound_check = Time.timeSinceLevelLoad;

			Vector2 pos_in_screen = References.currentCamera.WorldToScreenPoint (transform.position);

			if (pos_in_screen.x + boundryRange < 0 ||
				pos_in_screen.x - boundryRange > Screen.width ||
				pos_in_screen.y + boundryRange < 0 ||
				pos_in_screen.y - boundryRange > Screen.height)
			{
				// it's out of screen
				Destroy (gameObject);
			}
		}
	}


	public void OnCollide(Collision2D other)
	{
		Debug.Log ($"collided with {other.gameObject.name}");
		var enemy = other.gameObject.GetComponent<Enemy> ();

		if (enemy != null)
		{
			Debug.Log ($"Damaging enemy {enemy.name} , damage : {damage}");
			enemy.TakeDamage (new PlayerBulletDamageInfo (damage, stunDuration));

			DestroyBullet ();
		}
	}

	private void DestroyBullet()
	{
		Destroy (gameObject);
		onHit?.Invoke(damage);
	}

}
