using System;
using Gameplay;
using Gameplay.Bullets;
using Gameplay.Player;
using UnityEngine;

namespace FlatVFX
{
	public class Bullet_TrailFX : MonoBehaviour
	{
		private PlayerNormalBullet _parentPlayerNormalBullet;

		public Settings settings;

		public TrailRenderer trailRenderer;


		private void Awake()
		{
			_parentPlayerNormalBullet = transform.parent.GetComponent<PlayerNormalBullet>();
			// if parent not PlayerNormalBullet, something is wrong
			if (!_parentPlayerNormalBullet)
			{
				Debug.LogError("Parent of object is not PlayerNormalBullet. Deleting self...");
				Destroy(gameObject);
			}

			_parentPlayerNormalBullet.onInit_fine += Setup;
		}

		/// <summary>
		///     set up the fx system.
		/// </summary>
		/// <param name="normalizedT">normalized T value for minmax evaluation</param>
		public void Setup(float normalizedT)
		{
			trailRenderer.time = Mathf.Lerp(settings.trailTime.min, settings.trailTime.max, normalizedT);
			trailRenderer.endWidth = Mathf.Lerp(settings.endWidth.min, settings.endWidth.max, normalizedT);
			var color = trailRenderer.startColor;
			color.a = Mathf.Lerp(settings.ColorTransparency.min, settings.ColorTransparency.max, normalizedT);
			trailRenderer.startColor = color;
		}

		[Serializable]
		public class Settings
		{
			public MinMax ColorTransparency;
			public MinMax endWidth;
			public MinMax trailTime;
		}
	}
}