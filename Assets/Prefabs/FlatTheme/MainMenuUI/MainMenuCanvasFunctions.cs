using System.Collections;
using SimpleScripts;
using UnityEngine;

namespace FlatTheme.MainMenuUI
{
        public class MainMenuCanvasFunctions : MonoBehaviour, CanvasSystem.IOnCanvasEnabled
        {
                public CanvasSystem.CanvasBase canvasBase;
                public RectTransform[] affectedTransforms;
                public UnityEngine.UI.GraphicRaycaster graphicRaycaster;
                public CanvasGroup canvasGroup;
                public AudioClip gameBackgroundMusic;
                public UI.SwipeButton swipeButton;

                public Sound backgroundMusic;


                [System.Serializable]
                public class FadeSettings
                {
                        public float fadeSpeed = 5;
                        public float moveDownSpeed = 5;
                        public float scaleUpSpeed = 5;
                }
                public FadeSettings fadeSettings;

                public void HideCanvasAndStartGame()
                {
                        Debug.Log($"HideCanvasAndStartGame");
                        StartCoroutine(HideCanvasAndStartGameAsync());
                }
                private IEnumerator HideCanvasAndStartGameAsync()
                {

                        // saving state
                        var _posY = new float[affectedTransforms.Length];
                        for (int i = 0; i < _posY.Length; i++)
                                _posY[i] = affectedTransforms[i].localPosition.y;
                        var _scale = new Vector3[affectedTransforms.Length];
                        for (int i = 0; i < _scale.Length; i++)
                                _scale[i] = affectedTransforms[i].localScale;


                        // disable further inputs
                        graphicRaycaster.enabled = false;
                        swipeButton.enabled = false;

                        // starting game behind the scenes
                        References.gameController.StartGame(canvasBase, false);


                        Time.timeScale = 0;

                        // fade out 
                        float scaleSpeed, moveSpeed;
                        do
                        {
                                canvasGroup.alpha -= fadeSettings.fadeSpeed * Time.unscaledDeltaTime;
                                Time.timeScale += fadeSettings.fadeSpeed * Time.unscaledDeltaTime;

                                scaleSpeed = fadeSettings.scaleUpSpeed * Time.unscaledDeltaTime;
                                moveSpeed = fadeSettings.moveDownSpeed * Time.unscaledDeltaTime;
                                foreach (var t in affectedTransforms)
                                {
                                        t.localPosition += Vector3.down * moveSpeed;
                                        t.localScale += Vector3.one * scaleSpeed;
                                }
                                yield return null;
                        }
                        while (canvasGroup.alpha > 0);

                        // absolute values
                        canvasGroup.alpha = 0;
                        Time.timeScale = 1;

                        // disable fully
                        canvasBase.enabled = false;

                        // restore defaults
                        graphicRaycaster.enabled = true;
                        for (int i = 0; i < _posY.Length; i++)
                        {
                                var tmp = affectedTransforms[i].localPosition;
                                affectedTransforms[i].localPosition = new Vector3(tmp.x, _posY[i], tmp.z);
                                tmp = affectedTransforms[i].localScale;
                                affectedTransforms[i].localScale = _scale[i];
                        }
                        canvasGroup.alpha = 1;



                }

                public void OnCanvasEnable()
                {
                        swipeButton.enabled = true;

                        // hide gameplay objects
                        References.gameplayObjects.SetActive(false);

                        References.backgroundMusic.Play(backgroundMusic.clip, AudioSystem.BackgroundMusic.Source.menu, backgroundMusic.volume);
                }
        }

}
