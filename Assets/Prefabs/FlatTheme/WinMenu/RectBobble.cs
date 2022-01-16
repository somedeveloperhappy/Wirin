using CanvasSystem;
using SimpleScripts;
using UnityEngine;
using UnityEngine.UI;

namespace FlatTheme.WinMenu
{
    [RequireComponent(typeof(Image))]
    public class RectBobble : MonoBehaviour, IOnCanvasEnabled, IOnCanvasDisabled
    {
        #region refs
        public Image image;
        #endregion

        [System.Serializable]
        public class Settings
        {
            public float movingDistance = 10;
            public float movingSpeed = 10;
            public float movingFrequency = 0.75f;
            public MinMax<Color> color;
            public float colorChangeSpeed;
        }
        public Settings settings;

        private Vector2 currentTarget = Vector2.zero;
        private float lastT = 0;

        #region caches
        private RectTransform m_rectTransform;
        private Vector3 init_pos;
        #endregion


        public void OnCanvasDisable()
        {
            m_rectTransform.position = init_pos;
            this.enabled = false;
        }
        public void OnCanvasEnable()
        {
            init_pos = m_rectTransform.position;
            this.enabled = true;
        }


        [ContextMenu("Auto Resolve")]
        private void AutoResolve()
        {
            image = GetComponent<Image>();
        }

        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
            this.enabled = false;
        }

        private void Update()
        {
            var t = Mathf.Sin(Time.realtimeSinceStartup * settings.colorChangeSpeed);

            image.color = Color.Lerp(settings.color.min, settings.color.max, t);


            // moving system
            if (Time.realtimeSinceStartup - lastT >= settings.movingFrequency)
            {
                lastT = Time.realtimeSinceStartup;
                ChangeMoveTarget();
            }

            m_rectTransform.position = Vector3.MoveTowards(m_rectTransform.position, currentTarget, Time.unscaledDeltaTime * settings.movingSpeed);
        }

        private void ChangeMoveTarget()
        {
            currentTarget = init_pos + new Vector3(
                Random.Range(-settings.movingDistance / 2, settings.movingDistance / 2),
                Random.Range(-settings.movingDistance / 2, settings.movingDistance / 2),
                0);
        }
    }
}