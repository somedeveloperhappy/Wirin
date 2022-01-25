using CanvasSystem;
using UnityEngine;

namespace Settings
{
    public class QualitySetterButton : MonoBehaviour, CanvasSystem.IOnCanvasEnabled
    {
        public GlobalSettings.Quality qualityValue;
        private UnityEngine.UI.Button button;
        public UnityEngine.UI.Button Button => button;

        static System.Collections.Generic.List<QualitySetterButton> instances = new System.Collections.Generic.List<QualitySetterButton>();
        private void Awake()
        {
            instances.Add(this);
            button = GetComponent<UnityEngine.UI.Button>();
        }
        private void OnDestroy() => instances.Remove(this);
        void IOnCanvasEnabled.OnCanvasEnable() => button.interactable = GlobalSettings.quality != qualityValue;
        public void SetQuality()
        {
            foreach (var item in instances) item.button.interactable = true;
            button.interactable = false;
        }

    }
}