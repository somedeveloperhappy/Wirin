using System;
using System.Collections.Generic;
using LevelManaging;
using UnityEngine;

namespace Enemies
{
	public abstract class Enemy : MonoBehaviour
	{
		/// <returns>the current health for this enemy</returns>
		public abstract float Health { get; set; }

		public int points;


		public virtual void Init(int points)
		{
			this.points = points;
		}


		static public List<Enemy> instances = new List<Enemy> ();

		protected virtual void Awake()
		{
			instances.Add (this);
		}

		#region events
		public event System.Action<float, PlayerBulletDamageInfo> onTakeDamage;
		public event System.Action onDestroy;
		#endregion


		protected virtual void OnDestroy() => instances.Remove (this);

		public void DestroyEnemy()
		{
			BeforeDestruction ();
			Destroy (gameObject);
			References.levelManager.OnEnemyDestroy (this);
			AfterDestruction ();
			onDestroy?.Invoke ();
		}

		protected virtual void BeforeDestruction() { }
		protected virtual void AfterDestruction() { }

		public void TakeDamage(PlayerBulletDamageInfo damageInfo)
		{
			var health_before = Health;
			Health -= damageInfo.damage;
			OnTakeDamage ();
			onTakeDamage?.Invoke (health_before, damageInfo);
		}

		protected virtual void OnTakeDamage() { }

	}
}
