using System;
using Gameplay.PressSystem;
using UnityEngine;

namespace FlatTheme
{
	public class OnPress_FrontVFX : MonoBehaviour, IOnPressFx
	{
		public new ParticleSystem particleSystem;

		public Settings settings;

		public void Apply(float normalizedT)
		{
			// set emission rate
			var emission = particleSystem.emission;
			emission.rateOverTime = Mathf.Lerp(settings.emissionRate.min, settings.emissionRate.max, normalizedT);

			// set shape scale x
			var shape = particleSystem.shape;
			shape.scale = new Vector3(
				Mathf.Lerp(settings.shapeEdgeSizeX.min, settings.shapeEdgeSizeX.max, normalizedT), shape.scale.y,
				shape.scale.z);

			// set main size y
			var main = particleSystem.main;
			main.startSizeY = Mathf.Lerp(settings.startSizeY.min, settings.startSizeY.max, normalizedT);

			// set start ligetime
			main.startLifetime = Mathf.Lerp(settings.startLifeTime.min, settings.startLifeTime.max, normalizedT);

			// set alpha
			var startCol = main.startColor;
			var col = startCol.color;
			col.a = Mathf.Lerp(settings.alpha.min, settings.alpha.max, normalizedT);
			startCol.color = col;
			main.startColor = startCol;
		}

		public void Initialize()
		{
			this.DefaultInitialize();
		}

		private void Start()
		{

			particleSystem ??= GetComponent<ParticleSystem>();
			Initialize();
		}

		[Serializable]
		public class Settings
		{
			public SimpleScripts.MinMax alpha;
			public SimpleScripts.MinMax emissionRate;
			public SimpleScripts.MinMax shapeEdgeSizeX;
			public SimpleScripts.MinMax startLifeTime;
			public SimpleScripts.MinMax startSizeY;
		}
	}
}
