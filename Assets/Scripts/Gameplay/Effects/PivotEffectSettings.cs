using UnityEngine;

namespace OnPressFxSettings
{
    internal interface IOnPressFxSettings
    {
        /// <param name="t">normalized</param>
        public void Update(float t);
    }

    [System.Serializable]
    public class FillingEffect : IOnPressFxSettings
    {
        [SerializeField] ParticleSystem particleSystem;
        [SerializeField] float min, max;


        [System.Serializable]
        struct ColorChangers
        {
            public SpriteRenderer colorChagingSprite;
            public Gradient colorOverPress;
        }

        [SerializeField] ColorChangers[] colorChangers;

        float last;

        public void Update(float t) {
            if (t == last) return;
            last = t;


            // applyign color for sprite renderer
            foreach (var c in colorChangers) {
                c.colorChagingSprite.color = c.colorOverPress.Evaluate(t);
            }

            // applying emission rate for particle system
            var emission = particleSystem.emission;
            emission.rateOverTimeMultiplier = Mathf.Lerp(min, max, t);
        }
    }

    [System.Serializable]
    public class ShaderEffect : IOnPressFxSettings
    {
        [SerializeField] MinMax chromaticIntensity;
        [SerializeField] MinMax lensDistortion;

        float last;

        public void Update(float t) {
            if (t == last) return;
            last = t;

            References.postPro.SetChromIntensity(Mathf.Lerp(chromaticIntensity.min, chromaticIntensity.max, t));
            References.postPro.SetLensDistortion(Mathf.Lerp(lensDistortion.min, lensDistortion.max, t));
        }
    }

    [System.Serializable]
    public class CameraEffect : IOnPressFxSettings
    {
        [SerializeField] MinMax cameraSize;

        float last;

        public void Update(float t) {
            if (t == last) return;
            last = t;

            References.currentCamera.orthographicSize = Mathf.Lerp(cameraSize.min, cameraSize.max, t);
        }
    }
}