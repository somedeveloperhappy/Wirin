using UnityEngine;
using UnityEngine.Serialization;

namespace PlayManagement
{
	[System.Serializable]
	public class PivotScaleSettings
	{
		[SerializeField] AnimationCurve scale;
		[SerializeField] float downSpeed, upSpeed;
		float time = 0;

		public float IncreaseAndGet(ref float deltaTime)
		{
			time += deltaTime * upSpeed;
			if (time > scale.keys[scale.keys.Length - 1].time) time = scale.keys[scale.keys.Length - 1].time;
			return scale.Evaluate (time);
		}

		public float DecreaseAndGet(ref float deltaTime)
		{
			time -= deltaTime * downSpeed;
			if (time < 0) time = 0;
			return scale.Evaluate (time);
		}
	}

	[System.Serializable]
	public class TrinonRotateSettings
	{
		[SerializeField] AnimationCurve speedMultiplier;
		[SerializeField] float downSpeed, upSpeed;
		float time = 0;

		public float IncreaseAndGet(ref float deltaTime)
		{
			time += deltaTime * upSpeed;
			if (time > speedMultiplier.keys[speedMultiplier.keys.Length - 1].time)
				time = speedMultiplier.keys[speedMultiplier.keys.Length - 1].time;
			return speedMultiplier.Evaluate (time);
		}

		public float DecreaseAndGet(ref float deltaTime)
		{
			time -= deltaTime * downSpeed;
			if (time < 0) time = 0;
			return speedMultiplier.Evaluate (time);
		}
	}

	[System.Serializable]
	public class BulletSpeedSettings
	{
		[SerializeField] AnimationCurve bulletSpeedMultiplier;
		[SerializeField] float curveSpeed = 1;

		public float GetSpeedByTime(float time)
		{
			return bulletSpeedMultiplier.Evaluate (time * curveSpeed);
		}
	}

	[System.Serializable]
	public class BlurOnPressSettings
	{
		[SerializeField] float intensity;
		[SerializeField] AnimationCurve intensityMultiplier;

		[FormerlySerializedAs ("intensity_down_speed")]
		[SerializeField]
		float intensityDownSpeed;

		[FormerlySerializedAs ("intensity_up_speed")]
		[SerializeField]
		float intensityUpSpeed;

		float _time = 0;

		public float IncreaseAndGet(ref float deltaTime)
		{
			_time += deltaTime * intensityUpSpeed;
			if (_time > intensityMultiplier.keys[intensityMultiplier.keys.Length - 1].time)
				_time = intensityMultiplier.keys[intensityMultiplier.keys.Length - 1].time;
			return intensityMultiplier.Evaluate (_time) * intensity;
		}

		public float DecreaseAndGet(ref float deltaTime)
		{
			_time -= deltaTime * intensityDownSpeed;
			if (_time < 0) _time = 0;
			return intensityMultiplier.Evaluate (_time) * intensity;
		}
	}
}
