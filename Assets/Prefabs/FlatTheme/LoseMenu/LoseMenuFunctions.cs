using System.Collections;
using UnityEngine;

namespace FlatTheme.LoseMenu
{
    public class LoseMenuFunctions : MonoBehaviour
    {
        public CanvasSystem.CanvasBase canvasBase;
        public CanvasGroup OverlayCanvasGroup;
        public UnityEngine.UI.Image overlayObject;

        [System.Serializable]
        public class FadeOutSettings
        {
            public float overlayFadeInSpeed = 1;
        }
        public FadeOutSettings fadeOutSettings;

        [ContextMenu("Auto Resolve")]
        public void AutoResolve()
        {
            overlayObject = GetComponent<UnityEngine.UI.Image>();
            canvasBase = GetComponent<CanvasSystem.CanvasBase>();
            OverlayCanvasGroup = GetComponent<CanvasGroup>();
        }

        public void ExitToMainMenu()
        {
            StartCoroutine(ExitToMenuAsync());
        }

        private IEnumerator ExitToMenuAsync()
        {
            Debug.Log("Exiting to main menu...");
            // disable functionlities of the canvas
            overlayObject.gameObject.SetActive(true);

            // loading scene in background
            var current_scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var reload_scene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
                sceneBuildIndex: UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex,
                UnityEngine.SceneManagement.LoadSceneMode.Single);
            reload_scene.completed += (op) =>
            {
                Debug.Log("Finished exitToMenuAsync shit");
            };

            // waiting for the reload scene to completely load up
            do
            {
                OverlayCanvasGroup.alpha += Time.unscaledDeltaTime * fadeOutSettings.overlayFadeInSpeed;
                Debug.Log($"reload scene : {reload_scene.progress}%");
                yield return null;
            } while (!reload_scene.isDone);
        }
    }
}