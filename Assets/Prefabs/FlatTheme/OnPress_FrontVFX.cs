using Gameplay.PressSystem;
using System;
using UnityEngine;

namespace FlatTheme
{
    public class OnPress_FrontVFX : MonoBehaviour, IOnPressFx
    {
        public ParticleSystem m_particleSystem;

        public Settings settings;

        void IOnPressFx.Apply(float normalizedT)
        {
            // set emission rate
            var emission = m_particleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(settings.emissionRate.min, settings.emissionRate.max, normalizedT);

            // set shape scale x
            var shape = m_particleSystem.shape;
            shape.scale = new Vector3(
                Mathf.Lerp(settings.shapeEdgeSizeX.min, settings.shapeEdgeSizeX.max, normalizedT), shape.scale.y,
                shape.scale.z);

            // set main size y
            var main = m_particleSystem.main;
            main.startSizeY = Mathf.Lerp(settings.startSizeY.min, settings.startSizeY.max, normalizedT);

            // set start ligetime
            main.startLifetime = Mathf.Lerp(settings.startLifeTime.min, settings.startLifeTime.max, normalizedT);

            // set alpha
            var startCol = main.startColor;
            var col = startCol.color;
            col.a = Mathf.Lerp(settings.alpha.min, settings.alpha.max, normalizedT);
            startCol.color = col;
            main.startColor = startCol;
        }

        private void OnEnable()
        {
            this.DefaultInitialize();
            m_particleSystem.Play();
            ((IOnPressFx)this).Apply(0);
        }

        private void OnDisable()
        {
            this.DefaultDestroy();
            m_particleSystem.Stop();
        }

        private void Start()
        {
            m_particleSystem ??= GetComponent<ParticleSystem>();
        }


        [Serializable]
        public class Settings
        {
            public SimpleScripts.MinMax alpha;
            public SimpleScripts.MinMax emissionRate;
            public SimpleScripts.MinMax shapeEdgeSizeX;
            public SimpleScripts.MinMax startLifeTime;
            public SimpleScripts.MinMax startSizeY;
        }
    }
}
