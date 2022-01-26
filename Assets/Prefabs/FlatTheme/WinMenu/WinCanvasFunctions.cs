using System;
using System.Collections;
using CanvasSystem;
using SimpleScripts;
using UnityEngine;

namespace FlatTheme.WinMenu
{
        public class WinCanvasFunctions : MonoBehaviour, CanvasSystem.IOnCanvasEnabled, CanvasSystem.IOnCanvasDisabled
        {
                public CanvasGroup canvasGroup;
                public CanvasSystem.CanvasBase canvasSystem;
                public UnityEngine.UI.GraphicRaycaster graphicRaycaster;
                public Sound startingMusic, loopMusic;
                public float startingMusicDuration = 10;

                [ContextMenu("Auto Resolve")]
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
                        StartCoroutine(AsyncPlay());
                }

                IEnumerator AsyncPlay()
                {
                        Time.timeScale = 0;

                        // disable menu functions
                        graphicRaycaster.enabled = false;

                        // disabling animation
                        if (TryGetComponent<Animator>(out Animator animator)) animator.enabled = false;

                        // start game, but timescale is still zero
                        References.gameController.StartGame(canvasSystem, false);

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
                                yield return null;

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
                        if (TryGetComponent<Animator>(out animator)) animator.enabled = true;

                }

                void IOnCanvasEnabled.OnCanvasEnable()
                {
                        References.backgroundMusic.Play(startingMusic.clip, AudioSystem.BackgroundMusic.Source.menu, startingMusic.volume, loop: false);
                        StartCoroutine(playLoopAfterDelay());
                }

                private IEnumerator playLoopAfterDelay()
                {
                        yield return new WaitForSecondsRealtime(startingMusicDuration);

                        // play the loop music
                        References.backgroundMusic.Play(loopMusic.clip, AudioSystem.BackgroundMusic.Source.menu, loopMusic.volume, loop: true);
                }

                void IOnCanvasDisabled.OnCanvasDisable()
                {
                        StopAllCoroutines();
                }
        }
}