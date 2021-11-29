using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PressFX;

namespace FlatVFX
{
	public class Background_FX : MonoBehaviour, IOnPressFx
	{
		[System.Serializable]
		public class Overlay
		{
			public MeshRenderer meshRenderer;
			[HideInInspector] public Material material;
			[HideInInspector] public float hueChangeSpeed = 0;
			[HideInInspector] public Color overlayBaseColor;
			[HideInInspector] public float current_hue;

			[System.Serializable]
			public class Settings
			{
				public MinMax hueChangeSpeed;
				public MinMax alpha;
			}
			public Settings settings;
		}
		[System.Serializable]
		public class Models
		{
			public MeshRenderer meshRenderer;
			[HideInInspector] public Material mateiral;

			[System.Serializable]
			public class Settings
			{
				public MinMax steps;
			}
			public Settings settings;
		}

		public Overlay overlay;
		public Models[] models;

#region handy refs
		public Trinon trinon => References.trinon;
		public Bullet bullet => References.trinon.bulletPrefab;
#endregion

		private float bulletMaxPower = 0;

		private void Start()
		{
			// overlay stuff
			overlay.material = overlay.meshRenderer.material;
			overlay.overlayBaseColor = overlay.material.GetColor ("_MainColor");
			Color.RGBToHSV (overlay.overlayBaseColor, out overlay.current_hue, out _, out _); // getting the first hue

			// models stuff
			foreach (var model in models)
			{
				model.mateiral = model.meshRenderer.material;
			}

			this.Initialize ();
		}

		private void Update()
		{
			// overlay stuff
			ChangeHue (Time.deltaTime * overlay.hueChangeSpeed);
		}

		private void ChangeHue(float delta)
		{
			// getting HSV
			float s, v, a;
			Color.RGBToHSV (overlay.overlayBaseColor, out _, out s, out v);
			// Changing Hue
			overlay.current_hue = (overlay.current_hue + delta) % 1;
			var col = Color.HSVToRGB (overlay.current_hue, s, v);
			col.a = overlay.overlayBaseColor.a;
			// Applying to material
			overlay.material.SetColor ("_MainColor", col);
		}

		public void Apply(float normalizedT)
		{
			// overlay
			overlay.hueChangeSpeed = Mathf.Lerp (overlay.settings.hueChangeSpeed.min, overlay.settings.hueChangeSpeed.max, normalizedT);
			overlay.overlayBaseColor.a = Mathf.Lerp (overlay.settings.alpha.min, overlay.settings.alpha.max, normalizedT);

			// models
			foreach (var model in models)
			{
				model.mateiral.SetFloat (
					"_Steps",
					Mathf.Lerp (model.settings.steps.min, model.settings.steps.max, normalizedT));
			}
		}
		public void Initialize() => this.DefaultInitialize ();
	}
}
