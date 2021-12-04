using System;
using Gameplay.Player;
using UnityEngine;

namespace FlatVFX
{
	public class PivotLifeFX : MonoBehaviour
	{

		private float _fillValueLast = -1;


		private float _smothnessLast = -1;
		private float m_healthtarget = 1;
		private float m_healthValue = 1;
		private Material m_material;
		public PlayerInfo playerInfo;
		public Settings settings;


		private float smoothnessSpeed;
		private float smoothnessValue;
		private float smoothnessValueMax;
		public SpriteRenderer spriteRenderer;

		private void Awake()
		{
			m_material = spriteRenderer.material;
			playerInfo.GetStats().onHealthChanged += onHealthChanged;
		}

		private void Start()
		{
			m_healthValue = settings.startingFillValue * playerInfo.GetStats().maxHealth;
			m_healthtarget = playerInfo.GetStats().maxHealth;
		}

		private void Update()
		{
			// smoothness
			smoothnessValue = Mathf.Sin(Time.timeSinceLevelLoad * smoothnessSpeed) * smoothnessValueMax;
			smoothnessValue *= smoothnessValue; // no minus values now
			SetSmoothness(smoothnessValue);

			// fill value
			m_healthValue = Mathf.Lerp(m_healthValue, m_healthtarget, settings.healthChangeSpeed * Time.deltaTime);
			SetFillValue(m_healthValue / playerInfo.GetStats().maxHealth);

		}

		private void onHealthChanged(float newHealth, PlayerInfo.Stats.HealthChangedType type)
		{
			var t = newHealth / playerInfo.GetStats().maxHealth;
			m_healthtarget = newHealth;
			smoothnessSpeed = settings.smoothnessSpeed.Evaluate(t);
			smoothnessValueMax = settings.smoothnessValueMax.Evaluate(t);
		}

		public void SetSmoothness(float smoothness)
		{
			if (smoothness != _smothnessLast)
			{
				m_material.SetFloat("_Smoothness", smoothness);
				_smothnessLast = smoothness;
			}
		}

		public void SetFillValue(float fillValue)
		{
			if (fillValue != _fillValueLast)
			{
				m_material.SetFloat("_FillValue", fillValue);
				_fillValueLast = fillValue;
			}
		}

		[Serializable]
		public class Settings
		{
			public float healthChangeSpeed;
			public MinMax smoothnessSpeed;
			public MinMax smoothnessValueMax;
			[Range(0f, 1f)] public float startingFillValue;
		}
	}
}