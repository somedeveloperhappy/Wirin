using System;
using System.Collections.Generic;
using LevelManaging;
using UnityEditor.Compilation;
using UnityEngine;

namespace Enemies
{
	public abstract class Enemy : MonoBehaviour
	{
		private float m_health;

		/// <returns>the current health for this enemy</returns>
		public float Health
		{
			get
			{
				OnGetHealth();
				return m_health;
			}
			set
			{
				OnSetHealth();
				m_health = value;
			}
		}

		protected virtual void OnGetHealth() { }
		protected virtual void OnSetHealth() { }

		public int points;


		public void Init(int points)
		{
			this.points = points;
			OnInit();
		}

		protected abstract void OnInit();


		static public List<Enemy> instances = new List<Enemy>();

		protected virtual void Awake()
		{
			instances.Add(this);
		}

#region events

		public event System.Action<float, PlayerBulletDamageInfo> onTakeDamage;
		public event System.Action onDestroy;

#endregion


		protected virtual void OnDestroy() => instances.Remove(this);

		public void DestroyEnemy()
		{
			Destroy(gameObject);
			References.levelManager.OnEnemyDestroy(this);
			AfterDestruction();
			onDestroy?.Invoke();
		}

		protected virtual void AfterDestruction() { }

		public void TakeDamage(PlayerBulletDamageInfo damageInfo)
		{
			var health_before = Health;
			Health -= damageInfo.damage;
			OnTakeDamage();
			onTakeDamage?.Invoke(health_before, damageInfo);
		}

		protected virtual void OnTakeDamage() { }

	}
}