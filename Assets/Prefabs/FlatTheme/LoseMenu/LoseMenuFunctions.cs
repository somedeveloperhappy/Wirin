using System.Collections;
using UnityEngine;

namespace FlatTheme.LoseMenu
{
    public class LoseMenuFunctions : MonoBehaviour
    {
        public CanvasSystem.CanvasBase canvasBase;
        public UnityEngine.UI.GraphicRaycaster raycaster;
        public UnityEngine.UI.Image overlayObject;

        [System.Serializable]
        public class FadeOutSettings
        {
            public float overlayFadeInSpeed = 1;
        }
        [System.Serializable]
        public struct OnRetrySettings
        {
            public Animator animator;
            public string animName;
            public float animDuration;
            public float timeUpSpeed;
        }
        public OnRetrySettings onRetrySettings;
        public FadeOutSettings fadeOutSettings;

        [ContextMenu("Auto Resolve")]
        public void AutoResolve()
        {
            overlayObject = GetComponent<UnityEngine.UI.Image>();
            canvasBase = GetComponent<CanvasSystem.CanvasBase>();
            raycaster = GetComponent<UnityEngine.UI.GraphicRaycaster>();
            onRetrySettings.animator = GetComponent<Animator>();
        }

        public void ExitToMainMenu() => StartCoroutine(ExitToMenuAsync());
        private IEnumerator ExitToMenuAsync()
        {
            Debug.Log("Exiting to main menu...");

            // saving highscore
            References.gameController.SaveGameScoreAndReset();

            // disable functionlities of the canvas
            raycaster.enabled = false;
            overlayObject.gameObject.SetActive(true);

            // next scene should start off showing submit menu
            Management.GameController.shouldShowSubmitScore = true;

            // loading scene in background
            var current_scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            int current_build_index = current_scene.buildIndex;
            var reload_scene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
                sceneBuildIndex: UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex,
                UnityEngine.SceneManagement.LoadSceneMode.Additive);

            reload_scene.completed += (op) =>
            {
                var operation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(current_scene);
                operation.completed += _ =>
                {
                    UnityEngine.SceneManagement.SceneManager.SetActiveScene(
                        UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(current_build_index));
                };
                Debug.Log("Finished exitToMenuAsync shit");
            };

            // waiting for the reload scene to completely load up
            do
            {
                var col = overlayObject.color;
                col.a += Time.unscaledDeltaTime * fadeOutSettings.overlayFadeInSpeed;
                overlayObject.color = col;
                // Debug.Log($"reload scene : {reload_scene.progress}%");
                yield return null;
            } while (!reload_scene.isDone);

        }




        public void Retry() => StartCoroutine(RetryAsync());

        private IEnumerator RetryAsync()
        {
            // disable functionalities
            raycaster.enabled = false;

            // time animation
            Time.timeScale = 0;

            // play anim and wait till it's finished
            onRetrySettings.animator.updateMode = AnimatorUpdateMode.UnscaledTime; // just in case
            onRetrySettings.animator.Play(onRetrySettings.animName);
            yield return new WaitForSecondsRealtime(onRetrySettings.animDuration);

            // call retry to gamecontroller
            References.gameController.Retry();

            // time from 0 to 1
            while (Time.timeScale < 1)
            {
                Time.timeScale += onRetrySettings.timeUpSpeed * Time.unscaledDeltaTime;
                yield return null;
            }

            // absolute
            Time.timeScale = 1;
            canvasBase.enabled = false;

            // restore defaults
            raycaster.enabled = true;
        }
    }
}