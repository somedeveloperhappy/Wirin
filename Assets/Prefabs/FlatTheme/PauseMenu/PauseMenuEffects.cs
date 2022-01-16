using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlatTheme.PauseMenu
{
    public class PauseMenuEffects : MonoBehaviour
    {
        [System.Serializable]
        public struct OverlayEffect
        {
            public Image overlayImage;
            public float hueShiftSpeed;
        }
        public OverlayEffect m_overlayEffect;

        private void Update()
        {
            UpdateOverlatEffect();
        }

        private void UpdateOverlatEffect()
        {
            float h, s, v;
            Color.RGBToHSV(m_overlayEffect.overlayImage.color, out h, out s, out v);
            h += m_overlayEffect.hueShiftSpeed * Time.unscaledDeltaTime;
            if (h > 1) h %= 1;
            m_overlayEffect.overlayImage.color = Color.HSVToRGB(h, s, v);
        }
    }
}