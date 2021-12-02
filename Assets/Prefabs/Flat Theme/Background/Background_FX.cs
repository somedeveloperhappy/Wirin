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
		public class PostProcess
		{
			public FlatBackgroundPP main;
			
			[System.Serializable]
			public class Settings
			{
				public MinMax fogIntensity, saturation;
			}
			public Settings settings;
		}
		[System.Serializable]
		public class Models
		{
			public MeshRenderer meshRenderer;
			[HideInInspector] public Material mateiral;
			public Vector2 rotatingPivot;
			
			[HideInInspector] public float speed;
			
			[System.Serializable]
			public class Settings
			{
				public MinMax steps;	
				public MinMax rotatingSpeed;
			}
			public Settings settings;
		}
		
		[System.Serializable]
		public class Lightfx
		{
			public Light light;
			public MinMax speed;
			[HideInInspector] public float m_speed;
		}

		public PostProcess postPro;
		public Models[] models;
		public Lightfx lightfx;

#region handy refs
		public Trinon trinon => References.trinon;
		public Bullet bullet => References.trinon.bulletPrefab;
#endregion

		private void Start()
		{
			// models stuff
			foreach (var model in models)
			{
				model.mateiral = model.meshRenderer.material;
			}

			this.Initialize ();
		}
		
		public void Apply(float normalizedT)
		{
			// overlay
			postPro.main.SetFogIntensity (postPro.settings.fogIntensity.Evaluate (normalizedT));
			postPro.main.SetSaturation (postPro.settings.saturation.Evaluate (normalizedT));

			// models
			foreach (var model in models)
			{
				model.mateiral.SetFloat ("_Steps", model.settings.steps.Evaluate (normalizedT));
				model.speed = model.settings.rotatingSpeed.Evaluate (normalizedT);
			}
			
			// light
			lightfx.m_speed = lightfx.speed.Evaluate (normalizedT);
		}
		public void Initialize() => this.DefaultInitialize ();
		
		private void Update() 
		{
			// models
			foreach (var model in models)
			{
				model.meshRenderer.transform.RotateAround (
					model.rotatingPivot, Vector3.back, Time.deltaTime * model.speed);
			}

			// light
			lightfx.light.transform.Rotate (Vector3.right * Time.deltaTime * lightfx.m_speed);

		}
	}
}
