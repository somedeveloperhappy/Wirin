using System.Collections;
using System.Collections.Generic;
using CanvasSystem;
using UnityEngine;

namespace FlatTheme.HighScoreMenu
{
    public class GlobalButton : MonoBehaviour, IOnCanvasEnabled, IOnCanvasDisabled
    {
        public UnityEngine.UI.Button button;

        [System.Serializable]
        public struct Settings
        {
            public Color selectedCol, deselectedStartingCol;
            public float hueShiftSpeed;
            public float scaleYOnSelect, scaleYOnDeselect;
        }
        public Settings settings;

        void IOnCanvasEnabled.OnCanvasEnable()
        {
            References.highScoreManager.onLocalHighscoreGet += EnableButton;
            this.enabled = true;
        }
        void IOnCanvasDisabled.OnCanvasDisable()
        {
            References.highScoreManager.onLocalHighscoreGet -= EnableButton;
            this.enabled = false;
        }
        public void OnButtonPress()
        {
            button.interactable = false;
            button.image.color = settings.selectedCol;

            var scale = button.image.rectTransform.sizeDelta;
            scale.y = settings.scaleYOnSelect;
            button.image.rectTransform.sizeDelta = scale;

            // actually do the highscore thing
            References.highScoreManager.GetGlobalHighscore();
        }
        private void EnableButton()
        {
            button.interactable = true;
            button.image.color = settings.deselectedStartingCol;

            var scale = button.image.rectTransform.sizeDelta;
            scale.y = settings.scaleYOnDeselect;
            button.image.rectTransform.sizeDelta = scale;
        }
        private void Update()
        {
            if (!button.isActiveAndEnabled) return;

            // hue shift button color
            float h, s, v;
            Color.RGBToHSV(button.image.color, out h, out s, out v);
            h += settings.hueShiftSpeed * Time.unscaledDeltaTime;
            button.image.color = Color.HSVToRGB(h, s, v);
        }
    }
}