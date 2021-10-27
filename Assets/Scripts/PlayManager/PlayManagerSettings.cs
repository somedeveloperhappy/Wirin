using UnityEngine;

namespace PlayManagement
{
    [System.Serializable]
    public class PivotScaleSettings
    {
        [SerializeField] AnimationCurve scale;
        [SerializeField] float downSpeed, upSpeed;
        float time = 0;
        
        public float IncreaseAndGet(ref float deltaTime) {
            time += deltaTime * upSpeed;
            if(time > scale.keys[scale.keys.Length-1].time) time = scale.keys[scale.keys.Length-1].time;
            return scale.Evaluate(time);
        }
        
        public float DecreaseAndGet(ref float deltaTime) {
            time -= deltaTime * downSpeed;
            if (time < 0) time = 0;
            return scale.Evaluate(time);
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
            if(time > speedMultiplier.keys[speedMultiplier.keys.Length-1].time) time = speedMultiplier.keys[speedMultiplier.keys.Length-1].time;
            return speedMultiplier.Evaluate(time);
        }
        public float DecreaseAndGet(ref float deltaTime) {
            time -= deltaTime * downSpeed;
            if (time < 0) time = 0;
            return speedMultiplier.Evaluate(time);
        }
    }
    
    [System.Serializable]
    public class BulletSpeedSettings
    {
        [SerializeField] AnimationCurve bulletSpeedMultiplier;
        [SerializeField] float curveSpeed = 1;
        
        public float GetSpeedByTime(float time) {
            return bulletSpeedMultiplier.Evaluate(time * curveSpeed);
        }
        
    }
    
    [System.Serializable]
    public class BlurOnPressSettings
    {
        [SerializeField] float intensity;
        [SerializeField] AnimationCurve intensityMultiplier;
        [SerializeField] float intensity_down_speed, intensity_up_speed;
        float time = 0;
        
        public float IncreaseAndGet(ref float deltaTime) {
            time += deltaTime * intensity_up_speed;
            if(time > intensityMultiplier.keys[intensityMultiplier.keys.Length-1].time) time = intensityMultiplier.keys[intensityMultiplier.keys.Length-1].time;
            return intensityMultiplier.Evaluate(time) * intensity;
        }
        
        public float DecreaseAndGet(ref float deltaTime) {
            time -= deltaTime * intensity_down_speed;
            if (time < 0) time = 0;
            return intensityMultiplier.Evaluate(time) * intensity;
        }
    }
}