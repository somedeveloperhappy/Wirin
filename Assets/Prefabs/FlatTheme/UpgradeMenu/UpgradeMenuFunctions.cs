using System;
using System.Collections;
using CanvasSystem;
using SimpleScripts;
using UnityEngine;

namespace FlatTheme.MainMenuUI
{
        public class UpgradeMenuFunctions : MonoBehaviour, CanvasSystem.IOnCanvasEnabled, CanvasSystem.IOnCanvasDisabled
        {
                private CanvasSystem.CanvasBase m_canvas;
                Animator animator;
                UnityEngine.UI.GraphicRaycaster m_graphicRaycaster;
                public UI.ScrollPannel m_scrollPannel;
                public Sound backgroundMusic;

                [Serializable]
                public struct AnimSettings
                {
                        public string inAnimName;
                        public float inDuration;
                        public string outAnimName;
                        public float outDuration;
                }
                [SerializeField] AnimSettings m_animSettings;

                [Serializable]
                public class ScrollPanelMovement
                {
                        public float inSpeed = 500;
                        public AnimationCurve inPos;
                        public float outSpeed;
                }
                public ScrollPanelMovement scrollPanelMovement;

                private void Awake()
                {
                        m_canvas = GetComponent<CanvasSystem.CanvasBase>();
                        animator = GetComponent<Animator>();
                        m_graphicRaycaster = GetComponent<UnityEngine.UI.GraphicRaycaster>();
                }

                public void ShowCanvas()
                {
                        StartCoroutine(ShowCanvasAsync());
                }

                private IEnumerator ShowCanvasAsync()
                {
                        m_canvas.enabled = true;

                        // disable scroll pannel
                        m_scrollPannel.enabled = false;

                        // play animation
                        animator.enabled = true;
                        animator.Play(m_animSettings.inAnimName);
                        var start_time = Time.realtimeSinceStartup;

                        Vector3 pos;
                        float t = 0;
                        while (Time.realtimeSinceStartup - start_time < m_animSettings.inDuration)
                        {
                                pos = m_scrollPannel.targetTransform.localPosition;
                                t += scrollPanelMovement.inSpeed * Time.unscaledDeltaTime;
                                pos.x = scrollPanelMovement.inPos.Evaluate(t);
                                m_scrollPannel.targetTransform.localPosition = pos;
                                yield return null;
                        }
                        yield return new WaitForSecondsRealtime(m_animSettings.inDuration);

                        // enabling functionalities
                        m_graphicRaycaster.enabled = true;
                        animator.StopPlayback();
                        animator.enabled = false;
                        m_scrollPannel.enabled = true;

                        // update outside view scrollables
                        m_scrollPannel.DisableOutsideView();
                }

                public void HideCanvas()
                {
                        StartCoroutine(HideCanvasAsync());
                }

                private IEnumerator HideCanvasAsync()
                {
                        // disable functionalities
                        m_scrollPannel.enabled = false;
                        References.eventSystem.enabled = false;

                        // play animation
                        animator.enabled = true;
                        animator.Play(m_animSettings.outAnimName);
                        var start_time = Time.realtimeSinceStartup;

                        Vector3 spd = Vector3.right * scrollPanelMovement.outSpeed;
                        while (Time.realtimeSinceStartup - start_time < m_animSettings.inDuration)
                        {
                                m_scrollPannel.targetTransform.localPosition += spd * Time.unscaledDeltaTime;
                                yield return null;
                        }
                        yield return new WaitForSecondsRealtime(m_animSettings.inDuration);



                        // absolute
                        m_canvas.enabled = false;

                        // restore defaults
                        animator.StopPlayback();
                        animator.enabled = false;
                        References.eventSystem.enabled = true;
                }

                void IOnCanvasDisabled.OnCanvasDisable()
                {
                        References.backgroundMusic.PlayPreviousMusic();
                }

                void IOnCanvasEnabled.OnCanvasEnable()
                {
                        References.backgroundMusic.Play(backgroundMusic.clip, AudioSystem.BackgroundMusic.Source.menu, backgroundMusic.volume);
                }
        }
}