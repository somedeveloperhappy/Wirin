using CanvasSystem;
using UnityEngine;

namespace FlatTheme.HighScoreMenu
{
    public class LocalButton : MonoBehaviour, IOnCanvasDisabled, IOnCanvasEnabled
    {
        public UnityEngine.UI.Button button;
        public HighScoreMenu.ScoreItem[] scoreItems;

        [System.Serializable]
        public struct Settings
        {
            public Color selectedCol, deselectedCol;
            public float scaleYOnSelect, scaleYOnDeselect;
        }
        public Settings settings;

        void IOnCanvasEnabled.OnCanvasEnable()
        {
            References.highScoreManager.onGlobalHighscoreGet += EnableButton;
            this.enabled = true;

            // press this button at first
            button.onClick.Invoke();
        }
        void IOnCanvasDisabled.OnCanvasDisable()
        {
            References.highScoreManager.onGlobalHighscoreGet -= EnableButton;
            this.enabled = false;
        }
        public void OnButtonPress()
        {
            button.interactable = false;
            button.image.color = settings.selectedCol;

            var scale = button.image.rectTransform.sizeDelta;
            scale.y = settings.scaleYOnSelect;
            button.image.rectTransform.sizeDelta = scale;

            // actually get highscores
            var items = References.highScoreManager.GetLocalHighscore();

            // deleting old items
            // creating new items
            for (int i = 0; i < scoreItems.Length; i++)
            {
                if (items != null && i < items.Count)
                {
                    scoreItems[i].enabled = true;
                    scoreItems[i].Init(items[i].name, i+1, items[i].score);
                }
                else
                {
                    scoreItems[i].enabled = false;
                }
            }
        }
        private void EnableButton()
        {
            button.interactable = true;
            button.image.color = settings.deselectedCol;

            var scale = button.image.rectTransform.sizeDelta;
            scale.y = settings.scaleYOnDeselect;
            button.image.rectTransform.sizeDelta = scale;
        }
    }
}
