using System;
using System.Collections;
using System.Collections.Generic;
using LevelManaging;
using UnityEngine;

namespace Enemies
{
	public class EnemyTrig : Enemy
	{
		private Vector3 targetPosition;

#region caches

		private new Transform transform;
#endregion

#region settings

		public float speed = 0.2f;
		public float damage = 1;

#endregion

		protected override void OnInit()
		{
			// set up values
			Health = (int)(points / 10);
			
			targetPosition = FindObjectOfType<PlayerInfo>(includeInactive: true).parts.pivot.transform.position;
			// caching
			transform = gameObject.transform;

			RoateTowrardsTarget();
		}

		protected override void OnSetHealth()
		{
			if (Health <= 0)
			{
				Health = 0;
				DestroyEnemy();
			}
		}

		private void RoateTowrardsTarget()
		{
			transform.up = targetPosition - transform.position;
		}

		private void Update()
		{
			MoveTowardsTarget();
		}

		private void MoveTowardsTarget()
		{
			transform.position += transform.up * speed * Time.deltaTime;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent<PlayerInfo>(out PlayerInfo playerInfo))
			{
				EnemyDamageInfo damageInfo = new EnemyDamageInfo(
					damage: damage);
				playerInfo.TakeDamage(damageInfo);
				
				DestroyEnemy();
			}
		}
	}

}