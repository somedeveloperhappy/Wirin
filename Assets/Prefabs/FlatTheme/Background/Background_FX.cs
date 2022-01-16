using Gameplay.Player;
using Gameplay.PressSystem;
using SimpleScripts;
using System;
using UnityEngine;

namespace FlatTheme
{
    public class Background_FX : MonoBehaviour, IOnPressFx
    {
        public Lightfx lightfx;
        public Models[] models;

        public PostProcess postPro;

        void IOnPressFx.Apply(float normalizedT)
        {
            // overlay
            postPro.main.SetFogIntensity(postPro.settings.fogIntensity.Evaluate(normalizedT));
            postPro.main.SetSaturation(postPro.settings.saturation.Evaluate(normalizedT)); 

            // models
            foreach (var model in models)
            {
                model.mateiral.SetFloat("_Steps", model.settings.steps.Evaluate(normalizedT));
                model.speed = model.settings.rotatingSpeed.Evaluate(normalizedT);
            }

            // light
            lightfx.m_speed = lightfx.speed.Evaluate(normalizedT);
        }

        private void Start()
        {
            // models stuff
            foreach (var model in models) model.mateiral = model.meshRenderer.material;
            // set random fog color
            postPro.main.SetFogColor(postPro.settings.fogColors[UnityEngine.Random.Range(0, postPro.settings.fogColors.Length)]);

        }
        private void OnEnable() => this.DefaultInitialize();
        private void OnDisable() => this.DefaultDestroy();

        private void Update()
        {
            // models
            foreach (var model in models)
                model.meshRenderer.transform.RotateAround(
                    model.rotatingPivot, Vector3.back, Time.deltaTime * model.speed);

            // light
            lightfx.light.transform.Rotate(Vector3.right * Time.deltaTime * lightfx.m_speed);

        }

        [Serializable]
        public class PostProcess
        {
            public FlatBackgroundPP main;
            public Settings settings;

            [Serializable]
            public class Settings
            {
                public Color[] fogColors;
                public MinMax fogIntensity, saturation;
            }
        }

        [Serializable]
        public class Models
        {
            [HideInInspector] public Material mateiral;
            public MeshRenderer meshRenderer;
            public Vector2 rotatingPivot;
            public Settings settings;

            [HideInInspector] public float speed;

            [Serializable]
            public class Settings
            {
                public MinMax rotatingSpeed;
                public MinMax steps;
            }
        }

        [Serializable]
        public class Lightfx
        {
            public Light light;
            [HideInInspector] public float m_speed;
            public MinMax speed;
        }

        #region handy refs

        public Trinon trinon => References.playerInfo.parts.mainTrinon;
        public PlayerNormalBullet PlayerNormalBullet => References.playerInfo.GetShootings().playerNormalBulletPrefab;

        #endregion

    }
}
