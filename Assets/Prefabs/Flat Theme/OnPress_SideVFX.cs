using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PressFX;

namespace FlatVFX
{
	public class OnPress_SideVFX : MonoBehaviour, IOnPressFx
	{
		[System.Serializable]
		public class Settings
		{
			public MinMax rateOverTime;
			public MinMax alpha;
		}

		public Settings settings;
		public new ParticleSystem particleSystem;

		#region quick refs
		public Trinon trinon => References.trinon;
		public Bullet bullet => References.trinon.bulletPrefab;
		#endregion

		float bulletMaxPower = 0; // bullet's max power

		private void Start()
		{
			particleSystem ??= GetComponent<ParticleSystem> ();
			this.Initialize();
		}

		public void Apply(float normalizedT)
		{
			// set emisison rate
			ParticleSystem.EmissionModule emission = particleSystem.emission;
			emission.rateOverTime = Mathf.Lerp (settings.rateOverTime.min, settings.rateOverTime.max, normalizedT);

			// set alpha
			var main = particleSystem.main;
			ParticleSystem.MinMaxGradient startCol = main.startColor;
			var col = startCol.color;
			col.a = Mathf.Lerp (settings.alpha.min, settings.alpha.max, normalizedT);
			startCol.color = col;
			main.startColor = startCol;

		}
		public void Initialize() => this.DefaultInitialize ();
	}
}
