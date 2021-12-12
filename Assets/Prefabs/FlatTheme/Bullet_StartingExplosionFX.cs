using System;
using Gameplay;
using Gameplay.Bullets;
using Gameplay.Player;
using UnityEngine;

namespace FlatTheme
{
	public class Bullet_StartingExplosionFX : MonoBehaviour
	{
		private PlayerNormalBullet _parentPlayerNormalBullet;

		public new ParticleSystem particleSystem;

		public Settings settings;


		private void Awake()
		{
			_parentPlayerNormalBullet = transform.parent.GetComponent<PlayerNormalBullet>();
			// if parent not PlayerNormalBullet, something is wrong
			if (!_parentPlayerNormalBullet)
			{
				Debug.LogError("Parent of object is not PlayerNormalBullet. Deleting self...");
				Destroy(gameObject);
			}

			_parentPlayerNormalBullet.onInit_fine += Init;
		}

		private void Init(float normalizedT)
		{
			Apply(normalizedT);
			particleSystem.Play();
		}

		/// <summary>
		///     set up the fx system.
		/// </summary>
		/// <param name="normalizedT">normalized T value for minmax evaluation</param>
		public void Apply(float normalizedT)
		{
			var main = particleSystem.main;
			main.startSize = Mathf.Lerp(settings.startingSize.min, settings.startingSize.max, normalizedT);
			main.startColor = Color.Lerp(settings.startingColor.min, settings.startingColor.max, normalizedT);
			main.startSpeed = Mathf.Lerp(settings.startingSpeed.min, settings.startingSpeed.max, normalizedT);
			main.startLifetime = Mathf.Lerp(settings.lifeTime.min, settings.lifeTime.max, normalizedT);

			var emission = particleSystem.emission;
			emission.rateOverTime = Mathf.Lerp(settings.emissionCount.min, settings.emissionCount.max, normalizedT);
		}

		[ContextMenu("Apply min")]
		private void ApplyMin()
		{
			Apply(0);
		}

		[ContextMenu("Apply max")]
		private void ApplyMax()
		{
			Apply(1);
		}

		[Serializable]
		public class Settings
		{
			public SimpleScripts.MinMax emissionCount;
			public SimpleScripts.MinMax lifeTime;
			public SimpleScripts.MinMax<Color> startingColor;
			public SimpleScripts.MinMax startingSize;
			public SimpleScripts.MinMax startingSpeed;
		}
	}
}
