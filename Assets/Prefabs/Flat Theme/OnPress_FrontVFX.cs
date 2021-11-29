using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PressFX;

namespace FlatVFX
{
    public class OnPress_FrontVFX : MonoBehaviour, IOnPressFx
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
		
		private void Start() 
		{
			
			particleSystem ??= GetComponent<ParticleSystem>();
			this.Initialize();
		}

		public void Apply(float normalizedT)
		{
			// set emission rate
			ParticleSystem.EmissionModule emission = particleSystem.emission;
			emission.rateOverTime = Mathf.Lerp (settings.emissionRate.min, settings.emissionRate.max, normalizedT);

			// set shape scale x
			var shape = particleSystem.shape;
			shape.scale = new Vector3 (
				Mathf.Lerp (settings.shapeEdgeSizeX.min, settings.shapeEdgeSizeX.max, normalizedT), shape.scale.y, shape.scale.z);

			// set main size y
			var main = particleSystem.main;
			main.startSizeY = Mathf.Lerp (settings.startSizeY.min, settings.startSizeY.max, normalizedT);

			// set start ligetime
			main.startLifetime = Mathf.Lerp (settings.startLifeTime.min, settings.startLifeTime.max, normalizedT);
            
            // set alpha
            ParticleSystem.MinMaxGradient startCol = main.startColor;
            var col = startCol.color;
            col.a = Mathf.Lerp(settings.alpha.min, settings.alpha.max, normalizedT);
            startCol.color = col;
            main.startColor = startCol;
		}

		public void Initialize() => this.DefaultInitialize ();
	}
}
