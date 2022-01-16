using Gameplay.PressSystem;
using SimpleScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatTheme.Player
{
    public class CoverImgFx : MonoBehaviour, IOnPressFx
    {
        [System.Serializable]
        public struct Settings
        {
            public SpriteRenderer spriteRenderer;
            public MinMax center, range, add;
            [HideInInspector] public int center_id, range_id, add_id;
            [HideInInspector] public Material mat;
        }
        public Settings settings;

        private void Awake() => this.DefaultInitialize();
        private void OnDestroy() => this.DefaultDestroy();
        private void Start()
        {
            settings.mat = settings.spriteRenderer.sharedMaterial;
            settings.center_id = Shader.PropertyToID("_Center");
            settings.range_id = Shader.PropertyToID("_Range");
            settings.add_id = Shader.PropertyToID("_Add");
        }
        void IOnPressFx.Apply(float normalizedT)
        {
            settings.mat.SetFloat(settings.center_id, settings.center.Evaluate(normalizedT));
            settings.mat.SetFloat(settings.range_id, settings.range.Evaluate(normalizedT));
            settings.mat.SetFloat(settings.add_id, settings.add.Evaluate(normalizedT));
        }
    }
}
