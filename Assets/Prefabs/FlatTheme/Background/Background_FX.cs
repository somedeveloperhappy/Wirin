using System;
using Gameplay;
using Gameplay.PressSystem;
using Gameplay.Player;
using UnityEngine;
using SimpleScripts;
using FlatTheme;

namespace FlatTheme
{
	public class Background_FX : MonoBehaviour, IOnPressFx
	{
		public Lightfx lightfx;
		public Models[] models;

		public PostProcess postPro;

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

		public void Initialize()
		{
			this.DefaultInitialize ();
		}

		private void Start()
		{
			// models stuff
			foreach (var model in models) model.mateiral = model.meshRenderer.material;

			Initialize ();
		}

		private void Update()
		{
			// models
			foreach (var model in models)
				model.meshRenderer.transform.RotateAround (
					model.rotatingPivot, Vector3.back, Time.deltaTime * model.speed);

			// light
			lightfx.light.transform.Rotate (Vector3.right * Time.deltaTime * lightfx.m_speed);

		}

		[Serializable]
		public class PostProcess
		{
			public FlatBackgroundPP main;
			public Settings settings;

			[Serializable]
			public class Settings
			{
				public MinMax fogIntensity, saturation;
			}
		}

		[Serializable]
		public class Models
		{
			[HideInInspector] public Material mateiral;
			public MeshRenderer meshRenderer;
			public Vector2 rotatingPivot;
			public Settings settings;

			[HideInInspector] public float speed;

			[Serializable]
			public class Settings
			{
				public MinMax rotatingSpeed;
				public MinMax steps;
			}
		}

		[Serializable]
		public class Lightfx
		{
			public Light light;
			[HideInInspector] public float m_speed;
			public MinMax speed;
		}

		#region handy refs

		public Trinon trinon => References.trinon;
		public PlayerNormalBullet PlayerNormalBullet => References.trinon.playerNormalBulletPrefab;

		#endregion

	}
}
