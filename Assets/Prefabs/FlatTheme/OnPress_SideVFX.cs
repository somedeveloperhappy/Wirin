using System;
using Gameplay;
using Gameplay.PressSystem;
using Gameplay.Player;
using UnityEngine;

namespace FlatTheme
{
	public class OnPress_SideVFX : MonoBehaviour, IOnPressFx
	{
		public new ParticleSystem particleSystem;

		public Settings settings;

		public void Apply(float normalizedT)
		{
			// set emisison rate
			var emission = particleSystem.emission;
			emission.rateOverTime = Mathf.Lerp(settings.rateOverTime.min, settings.rateOverTime.max, normalizedT);

			// set alpha
			var main = particleSystem.main;
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
			public SimpleScripts.MinMax rateOverTime;
		}

#region quick refs

		public Trinon trinon => References.trinon;
		public PlayerNormalBullet PlayerNormalBullet => References.trinon.playerNormalBulletPrefab;

#endregion

	}
}
