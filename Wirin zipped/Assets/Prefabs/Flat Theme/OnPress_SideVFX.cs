using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatVFX
{
	public class OnPress_SideVFX : MonoBehaviour
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

		private void Start() {
			
			particleSystem ??= GetComponent<ParticleSystem>();
			
			trinon.onShootingPressStart += OnPressStart;
			trinon.onShootingPressEnd += OnPressEnd;
			trinon.onShootingPressUpdate += OnPressUpdate;

			bulletMaxPower = bullet.damageCurve.keys[ bullet.damageCurve.keys.Length - 1 ].value;
		}

		private void OnPressUpdate(float duration) {

			float evaluationTime = bullet.damageCurve.Evaluate (duration) / bulletMaxPower;

			ApplyEffect (evaluationTime);
		}

		private void ApplyEffect(float evaluationTime) {
			// set emisison rate
			ParticleSystem.EmissionModule emission = particleSystem.emission;
			emission.rateOverTime = Mathf.Lerp (settings.rateOverTime.min, settings.rateOverTime.max, evaluationTime);
			
			// set alpha
			var main = particleSystem.main;
            ParticleSystem.MinMaxGradient startCol = main.startColor;
            var col = startCol.color;
            col.a = Mathf.Lerp(settings.alpha.min, settings.alpha.max, evaluationTime);
            startCol.color = col;
            main.startColor = startCol;		}

		private void OnPressEnd(float duration) {
			particleSystem.Stop();
		}

		private void OnPressStart() 
		{
			particleSystem.Play();
			
			// set emission rate to min
			ParticleSystem.EmissionModule emission = particleSystem.emission;
			emission.rateOverTime = settings.rateOverTime.min;
		}
	}
}
