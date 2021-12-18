using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace FlatTheme.WinMenu
{
    public class WinCanvasFunctions: MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public CanvasSystem.CanvasBase canvasSystem;
        public UnityEngine.UI.GraphicRaycaster graphicRaycaster;

        [ContextMenu( "Auto Resolve" )]
        private void AutoResolve()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasSystem = GetComponent<CanvasSystem.CanvasBase>();
            graphicRaycaster = GetComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        [System.Serializable]
        public class CanvasFadeSettings
        {
            public float fadeSpeed = 1;
        }
        public CanvasFadeSettings fadeSettings;

        [System.Serializable]
        public class HideMovingSubject
        {
            public enum Direction { Up, Down }
            public Direction direction = Direction.Up;
            public RectTransform[] transforms;
            public float moveSpeed = 100;
            public float upscaleSpeed = 0.1f;
        }
        public HideMovingSubject[] hideMovingSubjects;


        public void Play()
        {
            AsyncPlay();
        }

        async void AsyncPlay()
        {
            Time.timeScale = 0;

            // disable menu functions
            graphicRaycaster.enabled = false;

            // start game, but timescale is still zero
            References.gameController.StartGame( canvasSystem, false );

            // fade the canvas as well as rise timescale
            do
            {
                // main shit
                float t = Time.unscaledDeltaTime * fadeSettings.fadeSpeed;
                canvasGroup.alpha -= t;
                Time.timeScale += t;

                // sub shit
                foreach (var subject in hideMovingSubjects)
                {
                    // move
                    var moveSpd = (subject.direction == HideMovingSubject.Direction.Up ? Vector3.up : Vector3.down) * subject.moveSpeed * Time.unscaledDeltaTime;
                    // scale
                    var scaleSpd = Vector3.one * subject.upscaleSpeed * Time.unscaledDeltaTime;

                    foreach (var trans in subject.transforms)
                    {
                        trans.localPosition += moveSpd;
                        trans.localScale += scaleSpd;
                    }
                }

                // wait for next frame
                //yield return null;
                await Task.Yield();

            } while (canvasGroup.alpha > 0);

            // absolutes
            canvasGroup.alpha = 0;
            Time.timeScale = 1;

            // disable this canvas
            canvasSystem.enabled = false;

            // things back to normal
            foreach (var subject in hideMovingSubjects)
            {
                foreach (var trans in subject.transforms)
                {
                    trans.localPosition = Vector3.zero;
                    trans.localScale = Vector3.one;
                }
            }
            canvasGroup.alpha = 1;
            graphicRaycaster.enabled = true;

        }
    }
}