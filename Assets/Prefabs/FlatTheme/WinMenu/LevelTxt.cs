using CanvasSystem;
using UnityEngine;

namespace FlatTheme.WinMenu
{
    public class LevelTxt : MonoBehaviour, IOnCanvasEnabled, IOnCanvasDisabled
    {
        public Management.LevelManager LevelManager;
        public TMPro.TMP_Text text;

        [ContextMenu("Auto Resolve")]
        private void AutoResolve()
        {
            text = GetComponent<TMPro.TMP_Text>();
            LevelManager = FindObjectOfType<Management.LevelManager>();
        }

        public void OnCanvasEnable()
        {
            this.enabled = true;
            text.text = LevelManager.levelNumber.ToString();
        }

        void IOnCanvasDisabled.OnCanvasDisable() => this.enabled = false;
    }
}