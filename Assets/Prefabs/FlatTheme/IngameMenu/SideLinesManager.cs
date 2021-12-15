using Gameplay.EnemyNamespace.Types;
using UnityEngine;
using UnityEngine.UI;

namespace FlatTheme.IngameMenu
{
    public class SideLinesManager : MonoBehaviour, CanvasSystem.IOnCanvasEnabled, CanvasSystem.IOnCanvasDisabled
    {
        [System.Serializable]
        public class ImageCase
        {
            public Image img;
            [HideInInspector] public bool isON = false;
            [HideInInspector] public Vector2 position;
        }

        #region references
        public ImageCase lineRight, lineLeft, lineTop, lineBottom;
        public Management.LevelManager levelManager;
        #endregion

        [System.Serializable]
        public class Settings
        {
            public Color warningColor = Color.red;
            public float warningScale = 1.2f;
            public float turnOnSpeed = 3;
            public float turnOffSpeed = 0.1f;
        }
        public Settings settings;
        public float updateRate = 1;

        private float last_update = 0;


        public void OnCanvasEnable() => shouldUpadte = true;
        public void OnCanvasDisable() => shouldUpadte = false;

        [ContextMenu("Auto Resolve")]
        private void AutoResolve()
        {
            levelManager = FindObjectOfType<Management.LevelManager>();

            // for times for sake of being sure
            IterateAndResolveLines();
            IterateAndResolveLines();
            IterateAndResolveLines();
            IterateAndResolveLines();

            void IterateAndResolveLines()
            {
                foreach (Transform t in transform)
                {
                    if (t.TryGetComponent<Image>(out Image img))
                    {
                        if (!lineRight.img && t.name.Contains("r")) lineRight.img = img;
                        else if (!lineLeft.img && t.name.Contains("l")) lineLeft.img = img;
                        else if (!lineTop.img && t.name.Contains("t") || t.name.Contains("u")) lineTop.img = img;
                        else if (!lineBottom.img && t.name.Contains("b") || t.name.Contains("d")) lineBottom.img = img;
                    }
                }
            }
        }



        private void Awake()
        {
            var cam = References.currentCamera;

            lineRight.position = cam.ScreenToWorldPoint(lineRight.img.transform.position);
            lineLeft.position = cam.ScreenToWorldPoint(lineLeft.img.transform.position);
            lineTop.position = cam.ScreenToWorldPoint(lineTop.img.transform.position);
            lineBottom.position = cam.ScreenToWorldPoint(lineBottom.img.transform.position);
        }

        bool shouldUpadte = false;
        private void Update()
        {
            if (!shouldUpadte) return;

            UpdateEffects();

            if (Time.realtimeSinceStartup - last_update >= updateRate)
            {
                UpdateStates();
                last_update = Time.realtimeSinceStartup;
            }

        }

        private void UpdateEffects()
        {
            UpdateImgCase(lineRight);
            UpdateImgCase(lineLeft);
            UpdateImgCase(lineTop);
            UpdateImgCase(lineBottom);

            void UpdateImgCase(ImageCase imageCase)
            {
                imageCase.img.color = Color.Lerp(
                    imageCase.img.color,
                    imageCase.isON ? settings.warningColor : Color.white,
                    (imageCase.isON ? settings.turnOnSpeed : settings.turnOffSpeed) * Time.unscaledDeltaTime);

                imageCase.img.transform.localScale = Vector3.Lerp(
                    imageCase.img.transform.localScale,
                    imageCase.isON ? Vector3.one * settings.warningScale : Vector3.one,
                    (imageCase.isON ? settings.turnOnSpeed : settings.turnOffSpeed) * Time.unscaledDeltaTime);
            }
        }

        private void UpdateStates()
        {
            Debug.Log("updating side lines ");
            // all are off
            lineRight.isON = lineLeft.isON = lineTop.isON = lineBottom.isON = false;

            // iterate for all enemies
            foreach (var enemy in EnemyBase.instances)
            {
                var pos = enemy.transform.position;
                // right
                if (!lineRight.isON && pos.x > lineRight.position.x)
                    lineRight.isON = true;
                // left
                else if (!lineLeft.isON && pos.x < lineLeft.position.x)
                    lineLeft.isON = true;
                // top
                else if (!lineTop.isON && pos.y > lineTop.position.y)
                    lineTop.isON = true;
                // bottom
                else if (!lineBottom.isON && pos.y < lineBottom.position.y)
                    lineBottom.isON = true;
            }
        }


    }
}