using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayManager
{
    [Serializable]
    public class PivotScaleSettings
    {
        [SerializeField] private float downSpeed, upSpeed;
        [SerializeField] private AnimationCurve scale;
        private float time;

        public float IncreaseAndGet(ref float deltaTime)
        {
            time += deltaTime * upSpeed;
            if (time > scale.keys[scale.keys.Length - 1].time) time = scale.keys[scale.keys.Length - 1].time;
            return scale.Evaluate(time);
        }

        public float DecreaseAndGet(ref float deltaTime)
        {
            time -= deltaTime * downSpeed;
            if (time < 0) time = 0;
            return scale.Evaluate(time);
        }
    }

    [Serializable]
    public class TrinonRotateSettings
    {
        [SerializeField] private float downSpeed, upSpeed;
        [SerializeField] private AnimationCurve speedMultiplier;
        private float time;

        public float IncreaseAndGet(ref float deltaTime)
        {
            time += deltaTime * upSpeed;
            if (time > speedMultiplier.keys[speedMultiplier.keys.Length - 1].time)
                time = speedMultiplier.keys[speedMultiplier.keys.Length - 1].time;
            return speedMultiplier.Evaluate(time);
        }

        public float DecreaseAndGet(ref float deltaTime)
        {
            time -= deltaTime * downSpeed;
            if (time < 0) time = 0;
            return speedMultiplier.Evaluate(time);
        }
    }

    [Serializable]
    public class BulletSpeedSettings
    {
        [SerializeField] private AnimationCurve bulletSpeedMultiplier;
        [SerializeField] private float curveSpeed = 1;

        public float GetSpeedByTime(float time)
        {
            return bulletSpeedMultiplier.Evaluate(time * curveSpeed);
        }
    }

    [Serializable]
    public class BlurOnPressSettings
    {

        private float _time;
        [SerializeField] private float intensity;

        [FormerlySerializedAs("intensity_down_speed")]
        [SerializeField]
        private float intensityDownSpeed;

        [SerializeField] private AnimationCurve intensityMultiplier;

        [FormerlySerializedAs("intensity_up_speed")]
        [SerializeField]
        private float intensityUpSpeed;

        public float IncreaseAndGet(ref float deltaTime)
        {
            _time += deltaTime * intensityUpSpeed;
            if (_time > intensityMultiplier.keys[intensityMultiplier.keys.Length - 1].time)
                _time = intensityMultiplier.keys[intensityMultiplier.keys.Length - 1].time;
            return intensityMultiplier.Evaluate(_time) * intensity;
        }

        public float DecreaseAndGet(ref float deltaTime)
        {
            _time -= deltaTime * intensityDownSpeed;
            if (_time < 0) _time = 0;
            return intensityMultiplier.Evaluate(_time) * intensity;
        }
    }
}