using Gameplay.Player;
using Gameplay.PressSystem;
using System;
using UnityEngine;

namespace FlatTheme
{
    public class OnPress_SideVFX : MonoBehaviour, IOnPressFx
    {
        public ParticleSystem m_particleSystem;

        public Settings settings;

        void IOnPressFx.Apply(float normalizedT)
        {
            // set emisison rate
            var emission = m_particleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(settings.rateOverTime.min, settings.rateOverTime.max, normalizedT);

            // set alpha
            var main = m_particleSystem.main;
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
            (this as IOnPressFx).Apply(0);
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
            public SimpleScripts.MinMax rateOverTime;
        }

        #region quick refs

        public Trinon trinon => References.playerInfo.parts.mainTrinon;
        public PlayerNormalBullet PlayerNormalBullet => References.playerInfo.GetShootings().playerNormalBulletPrefab;

        #endregion

    }
}
