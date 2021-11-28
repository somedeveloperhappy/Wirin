using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatVFX
{
    public class OnPress_FrontVFX : MonoBehaviour
    {
		[System.Serializable]
		public class Settings
		{
			public MinMax emissionRate;
            public MinMax shapeEdgeSizeX;
            public MinMax startSizeY;
            public MinMax startLifeTime;
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


		private void OnPressEnd(float duration) {
			particleSystem.Stop();
		}

		private void OnPressStart() 
		{
			particleSystem.Play();
            
            ApplyEffect(0);

		}

		[ContextMenu ("Apply Min")]
		public void ApplyMin() => ApplyEffect (0);

		[ContextMenu ("Apply Max")]
		public void ApplyMax() => ApplyEffect (1);
        

		private void ApplyEffect(float evaluationTime) {
			// set emission rate
			ParticleSystem.EmissionModule emission = particleSystem.emission;
			emission.rateOverTime = Mathf.Lerp (settings.emissionRate.min, settings.emissionRate.max, evaluationTime);

			// set shape scale x
			var shape = particleSystem.shape;
			shape.scale = new Vector3 (
				Mathf.Lerp (settings.shapeEdgeSizeX.min, settings.shapeEdgeSizeX.max, evaluationTime), shape.scale.y, shape.scale.z);

			// set main size y
			var main = particleSystem.main;
			main.startSizeY = Mathf.Lerp (settings.startSizeY.min, settings.startSizeY.max, evaluationTime);

			// set start ligetime
			main.startLifetime = Mathf.Lerp (settings.startLifeTime.min, settings.startLifeTime.max, evaluationTime);
            
            // set alpha
            ParticleSystem.MinMaxGradient startCol = main.startColor;
            var col = startCol.color;
            col.a = Mathf.Lerp(settings.alpha.min, settings.alpha.max, evaluationTime);
            startCol.color = col;
            main.startColor = startCol;
		}

        
        
    }
}
