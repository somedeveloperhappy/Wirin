using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatVFX
{
	public class PivotLifeFX : MonoBehaviour
	{
        public PlayerInfo playerInfo;
		public SpriteRenderer spriteRenderer;
		private Material m_material;
        
		[System.Serializable]
		public class Settings
		{
			[Range(0f, 1f)] public float startingFillValue;
            public float healthChangeSpeed;
            public MinMax smoothnessValueMax;
            public MinMax smoothnessSpeed;
		}
		public Settings settings;

        
        float smoothnessSpeed;
        float smoothnessValueMax;
        float smoothnessValue;
        float m_healthValue = 1;
        float m_healthtarget = 1;

		private void Awake()
		{
			m_material = spriteRenderer.material;
            playerInfo.GetStats().onHealthChanged += onHealthChanged;
		}
		
		private void Start()
		{
			m_healthValue = settings.startingFillValue * playerInfo.GetStats ().maxHealth;
			m_healthtarget = playerInfo.GetStats ().maxHealth;
		}
		
		private void Update()
		{
			// smoothness
			smoothnessValue = Mathf.Sin (Time.timeSinceLevelLoad * smoothnessSpeed) * smoothnessValueMax;
			smoothnessValue *= smoothnessValue; // no minus values now
			SetSmoothness (smoothnessValue);

			// fill value
			m_healthValue = Mathf.Lerp (m_healthValue, m_healthtarget, settings.healthChangeSpeed * Time.deltaTime);
			SetFillValue (m_healthValue / playerInfo.GetStats().maxHealth);
			
		}

		private void onHealthChanged(float newHealth, PlayerInfo.Stats.HealthChangedType type)
		{
			float t = newHealth / playerInfo.GetStats ().maxHealth;
			m_healthtarget = newHealth;
			smoothnessSpeed = settings.smoothnessSpeed.Evaluate (t);
			smoothnessValueMax = settings.smoothnessValueMax.Evaluate (t);
		}



		float _smothnessLast = -1;
		public void SetSmoothness(float smoothness)  
		{
			if(smoothness != _smothnessLast)
			{
				m_material.SetFloat ("_Smoothness", smoothness);
				_smothnessLast = smoothness;
			}
		}
		
		float _fillValueLast = -1;
        public void SetFillValue(float fillValue)
		{
			if(fillValue != _fillValueLast)
			{
				m_material.SetFloat("_FillValue", fillValue);
				_fillValueLast = fillValue;
			}
		}

	}
}
