using UnityEngine;

namespace pivotSettings
{
    [System.Serializable]
    public struct FillingEffect
    {
        [SerializeField] ParticleSystem particleSystem;
        [SerializeField] float min, max, value;



        [System.Serializable]
        struct ColorChangers
        {
            public SpriteRenderer colorChagingSprite;
            public Gradient colorOverPress;
        }
        [SerializeField] ColorChangers[] colorChangers;

        float last;

        /// <param name="t">normalized</param>
        public void Update(float t) {
            if (t > 1) t = 1;
            else if (t < 0) t = 0;

            if (t == last) return;
            last = t;


            // applyign color for sprite renderer
            foreach (var c in colorChangers) {
                c.colorChagingSprite.color = c.colorOverPress.Evaluate (t);
            }

            // applying emission rate for particle system
            var emission = particleSystem.emission;
            emission.rateOverTimeMultiplier = min + (max - min) * t;
        }

    }
}
