using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatTheme.IngameMenu
{
    public class PauseMenuFunctions : MonoBehaviour
    {
        [System.Serializable]
        public struct Settings
        {
            public CanvasGroup canvasGroup;
            public CanvasSystem.CanvasBase canvasBase;
            public UnityEngine.UI.GraphicRaycaster raycaster;
            public float fadeSpeed;
        }
        public Settings settings;

        public void PauseGame()
        {
            StartCoroutine(PauseGameAsync());
        }
        private IEnumerator PauseGameAsync()
        {
            // disable functionalities
            settings.raycaster.enabled = false;

            // show canavs
            settings.canvasBase.enabled = true;

            // starting values
            settings.canvasGroup.alpha = 0;

            do
            {
                float t = settings.fadeSpeed * Time.unscaledDeltaTime;
                settings.canvasGroup.alpha += t;
                Time.timeScale = Mathf.Max(0, Time.timeScale - t);
                Debug.Log($"during. t : {Time.timeScale}");
                yield return null;
            } while (Time.timeScale > 0);

            // absolutes
            settings.canvasGroup.alpha = 1;

            // actually pause now
            References.gameController.PauseGame();

            // restore functionalities
            settings.raycaster.enabled = true;
        }

        public void ResumeGame()
        {
            StartCoroutine(ResumeGameAsync());
        }

        private IEnumerator ResumeGameAsync()
        {
            // disable functionalities
            settings.raycaster.enabled = false;


            do
            {
                float t = settings.fadeSpeed * Time.unscaledDeltaTime;
                settings.canvasGroup.alpha -= t;
                Time.timeScale = Mathf.Min(1, Time.timeScale + t);
                yield return null;
            } while (Time.timeScale < 1);

            // absolutes
            settings.canvasGroup.alpha = 0;

            // actually pause now
            References.gameController.ResumeGame();
            settings.canvasBase.enabled = false;

            // restore defaults
            settings.raycaster.enabled = true;
            settings.canvasGroup.alpha = 1;
        }
    }
}
