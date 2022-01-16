using SimpleScripts;
using UnityEngine;

namespace FlatTheme.Common
{
    public class ImageColorPingPong : MonoBehaviour, CanvasSystem.IOnCanvasEnabled, CanvasSystem.IOnCanvasDisabled
    {
        public UnityEngine.UI.Image image;
        public MinMax<Color> color;
        public float speed = 1;

        public void OnCanvasDisable() => this.enabled = false;
        public void OnCanvasEnable() => this.enabled = true;

        [ContextMenu("Auto Resolve")]
        private void AutoResolve()
        {
            image = GetComponent<UnityEngine.UI.Image>();
        }

        private void Update()
        {
            var t = Mathf.Sin(Time.unscaledTime * speed);
            image.color = Color.Lerp(color.min, color.max, t);
        }
    }
}