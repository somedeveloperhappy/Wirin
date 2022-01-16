using Gameplay.PressSystem;
using SimpleScripts;
using System;
using UnityEngine;

namespace FlatTheme
{
    public class CameraEffectsOnPress : MonoBehaviour, IOnPressFx
    {
        public CameraEffect cameraEffect;

        public ShaderEffect shaderEffect;

        public void Apply(float normalizedT)
        {
            References.postPro.SetChromIntensity(
                Mathf.Lerp(shaderEffect.chromaticIntensity.min, shaderEffect.chromaticIntensity.max, normalizedT));

            References.postPro.SetLensDistortion(
                Mathf.Lerp(shaderEffect.lensDistortion.min, shaderEffect.lensDistortion.max, normalizedT));

            References.currentCamera.orthographicSize = Mathf.Lerp(
                cameraEffect.cameraSize.min, cameraEffect.cameraSize.max, normalizedT);
        }

        private void OnEnable() => this.DefaultInitialize();
        private void OnDisable() => this.DefaultDestroy();
        public void Initialize() => OnEnable();

        [Serializable]
        public class ShaderEffect
        {
            public MinMax chromaticIntensity, lensDistortion;
        }

        [Serializable]
        public class CameraEffect
        {
            public MinMax cameraSize;
        }
    }
}
