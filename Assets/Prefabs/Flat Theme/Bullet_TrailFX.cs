using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatVFX
{
	public class Bullet_TrailFX : MonoBehaviour
	{
		[System.Serializable]
		public class Settings
		{
			public MinMax trailTime;
			public MinMax endWidth;
            public MinMax ColorTransparency;
		}

		public Settings settings;

		public TrailRenderer trailRenderer;
		Bullet parentBullet;


		private void Awake()
		{
			parentBullet = transform.parent.GetComponent<Bullet> ();
			// if parent not bullet, something is wrong
			if (!parentBullet)
			{
				Debug.LogError ("Parent of object is not Bullet. Deleting self...");
				Destroy (gameObject);
			}
            parentBullet.onInit_fine += Setup;
		}

		/// <summary>
		/// set up the fx system. 
		/// </summary>
		/// <param name="normalizedT">normalized T value for minmax evaluation</param>
		public void Setup(float normalizedT)
		{
			trailRenderer.time = Mathf.Lerp (settings.trailTime.min, settings.trailTime.max, normalizedT);
			trailRenderer.endWidth = Mathf.Lerp (settings.endWidth.min, settings.endWidth.max, normalizedT);
            Color color = trailRenderer.startColor;
            color.a = Mathf.Lerp(settings.ColorTransparency.min, settings.ColorTransparency.max, normalizedT);
            trailRenderer.startColor = color;
		}

	}
}
