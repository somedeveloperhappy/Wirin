using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatVFX
{
	public class Bullet_StartingExplosionFX : MonoBehaviour
	{
		[System.Serializable]
		public class Settings
		{
			public MinMax startingSize;
			public MinMax startingSpeed;
			public MinMax<Color> startingColor;
			public MinMax lifeTime;
			public MinMax emissionCount;
		}

		public Settings settings;

		public new ParticleSystem particleSystem;
		Bullet parentBullet;


		private void Awake()
		{
			parentBullet = transform.parent.GetComponent<Bullet> ();
			// if parent not bullet, something is wrong
			if (!parentBullet)
			{
				Debug.LogError ("Parent of object is not Bullet. Deleting self...");
				Destroy (gameObject);
			}
            parentBullet.onInit += Init;
		}
		
		private void Init(float normalizedT)
		{
			Apply(normalizedT);
			particleSystem.Play();
		}

		/// <summary>
		/// set up the fx system. 
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
		
		[ContextMenu("Apply min")] void ApplyMin() => Apply(0);
		[ContextMenu("Apply max")] void ApplyMax() => Apply(1);

	}
}
