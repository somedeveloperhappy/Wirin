using UnityEngine;

namespace Gameplay.EnemyNamespace.Types.Trig
{
	public class EnemyBaseTrig : EnemyBase
	{
		private Vector3 targetPosition;

		#region caches

		private new Transform transform;

		#endregion

		protected override void Awake()
		{
			base.Awake ();

			// caching
			transform = gameObject.transform;

		}

		protected override void OnInit()
		{
			// set up values
			Health = (int) (points / 10);

			targetPosition = FindObjectOfType<Player.PlayerInfo> (true).parts.pivot.transform.position;

			RoateTowrardsTarget ();
		}

		protected override void OnSetHealth(ref float m_health)
		{
			if (m_health <= 0)
			{
				m_health = 0;
				DestroyEnemy ();
			}
		}

		private void RoateTowrardsTarget()
		{
			transform.up = targetPosition - transform.position;
		}

		private void Update()
		{
			MoveTowardsTarget ();
		}

		private void MoveTowardsTarget()
		{
			transform.position += transform.up * speed * Time.deltaTime;
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			Debug.Log($"colliding with {other.gameObject.name}");
			if (other.gameObject.TryGetComponent<Player.PlayerInfo> (out var playerInfo))
			{
				var damageInfo = new Player.EnemyDamageInfo (
					damage);
				playerInfo.TakeDamage (damageInfo);

				DestroyEnemy ();
			}
		}

		#region settings

		public float speed = 0.2f;
		public float damage = 1;

		#endregion

	}

}
