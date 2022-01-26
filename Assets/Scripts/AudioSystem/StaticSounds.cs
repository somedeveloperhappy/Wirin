using System;
using UnityEngine;

namespace AudioSystem
{
        public class StaticSounds : MonoBehaviour
        {
                AudioSource m_audioSource;
                public UnityEngine.Audio.AudioMixer mixer;
                [SerializeField] string mixerVolName;

                private void Awake()
                {
                        m_audioSource = GetComponent<AudioSource>();
                        if (!m_audioSource)
                                m_audioSource = gameObject.AddComponent<AudioSource>();
                }

                public void Play(AudioClip clip, float volume = 1)
                {
                        m_audioSource.PlayOneShot(clip, volume);
                }

                public void mixerFadeOut(float fadespeed = 80)
                {
                        StopAllCoroutines();
                        StartCoroutine(fadeAsync(-Mathf.Abs(fadespeed)));
                        Debug.Log("fading out");
                }
                public void mixerFadeIn(float fadespeed = 80)
                {
                        StopAllCoroutines();
                        StartCoroutine(fadeAsync(Mathf.Abs(fadespeed)));
                        Debug.Log("fading in");
                }
                private System.Collections.IEnumerator fadeAsync(float fadeSpeed)
                {
                        float vol;
                        if (mixer.GetFloat(mixerVolName, out vol) == false)
                                throw new Exception("volume name was wrong!");

                        float target = fadeSpeed < 0 ? -80 : 0;
                        Debug.Log($"target is {target}. fade sped : {fadeSpeed}");
                        fadeSpeed = Mathf.Abs(fadeSpeed);

                        do
                        {
                                vol = Mathf.MoveTowards(vol, target, fadeSpeed * Time.unscaledDeltaTime);
                                mixer.SetFloat(mixerVolName, vol);
                                yield return null;
                                if (target == -80) Debug.Log("fading out");
                                else Debug.Log("fading in");
                        } while (m_audioSource.volume != target);
                }
        }
}