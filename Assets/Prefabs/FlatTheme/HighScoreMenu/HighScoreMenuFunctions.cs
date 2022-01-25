using UnityEngine;

namespace FlatTheme.HighScoreMenu
{
    public class HighScoreMenuFunctions : MonoBehaviour
    {

        public CanvasSystem.CanvasBase canvasBase;
        public UnityEngine.UI.Button localRankButton;

        [System.Serializable]
        public struct FadeOutSettings
        {
            [HideInInspector] public Animator animator;
            public string animOutName;
            public float animDuration;
        }
        public FadeOutSettings fadeOutSettings;

        private void Start()
        {
            fadeOutSettings.animator = GetComponent<Animator>();
        }

        public void FadeOut()
        {
            StartCoroutine(FadeOutAsync());
        }

        private System.Collections.IEnumerator FadeOutAsync()
        {
            // click local rank button
            localRankButton.onClick.Invoke();
            
            fadeOutSettings.animator.Play(fadeOutSettings.animOutName);
            yield return new WaitForSecondsRealtime(fadeOutSettings.animDuration);
            canvasBase.enabled = false;
        }
    }
}
