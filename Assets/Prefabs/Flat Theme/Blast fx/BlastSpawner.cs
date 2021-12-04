using System;
using Gameplay;
using Gameplay.Bullets;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace FlatVFX
{
	public class BlastSpawner : MonoBehaviour
	{
		public BlastFX[] blasts;
		[FormerlySerializedAs("bullet")] public PlayerNormalBullet playerNormalBullet;

		private float t;

		private void Awake()
		{
			playerNormalBullet.onInit_fine += Init;
			playerNormalBullet.onHit += onBulletHit;
		}

		private void Init(float t)
		{
			this.t = t;

			// sorting blasts
			// SortBlastsBasedOnMaxT ();
		}

		[ContextMenu("Sort")]
		public void SortBlastsBasedOnMaxT()
		{
			for (var i = 0; i < blasts.Length - 1; i++)
			for (var j = i + 1; j < blasts.Length; j++)
				if (blasts[i].maxT > blasts[j].maxT)
				{
					// swap
					var tmp = blasts[i];
					blasts[i] = blasts[j];
					blasts[j] = tmp;
				}
		}

		private void onBulletHit(float damage)
		{
			// checkig the blast from first to last
			for (var i = 0; i < blasts.Length; i++)
				if (blasts[i].maxT >= t)
				{
					// blast!
					var rot = playerNormalBullet.transform.eulerAngles;
					rot.z = -rot.z;
					Instantiate(blasts[i].gamebject, transform.position, Quaternion.Euler(rot));
					Debug.Log($"blast {i + 1}. T was {t}");
					return;
				}
		}

		[Serializable]
		public class BlastFX
		{
			public GameObject gamebject;
			[Range(0f, 1f)] public float maxT;
		}
	}
}