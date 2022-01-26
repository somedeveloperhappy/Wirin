using UnityEngine;

namespace AudioSystem
{
        public class BackgroundMusic : MonoBehaviour
        {
                public AudioSource as1, as2;
                bool is_as1_on = true;
                float m_crossfadeSpeed = 0;
                float m_target_volume = 1;

                public enum Source
                {
                        Default, ingame, menu
                }
                [SerializeField] private UnityEngine.Audio.AudioMixerGroup ingameMusic, menuMusic;

                public void Play(AudioClip clip, Source source = Source.Default, float volume = 1, float crossfadeSpeed = 0.5f, bool loop = true)
                {
                        if (is_as1_on)
                        {
                                as2.clip = clip;
                                as2.Play();
                                as2.loop = loop;
                                if(source != Source.Default)
                                        as2.outputAudioMixerGroup = source == Source.ingame ? ingameMusic : menuMusic;
                        }
                        else
                        {
                                as1.clip = clip; 
                                as1.Play(); 
                                as1.loop = loop;
                                if(source != Source.Default)
                                        as1.outputAudioMixerGroup = source == Source.ingame ? ingameMusic : menuMusic;
                        }
                        is_as1_on = !is_as1_on;

                        m_crossfadeSpeed = crossfadeSpeed;
                        m_target_volume = volume;
                }
                public void PlayPreviousMusic(float crossfade = 0.5f)
                {
                        if (is_as1_on)
                                Play(as2.clip, crossfadeSpeed: crossfade);
                        else
                                Play(as1.clip, crossfadeSpeed: crossfade);
                }
                private void Update()
                {
                        if (is_as1_on)
                        {
                                if (as2.enabled)
                                {
                                        as2.volume -= m_crossfadeSpeed * Time.unscaledDeltaTime;
                                        if (as2.volume <= 0)
                                        {
                                                as2.volume = 0;
                                                as2.enabled = false;
                                        }
                                }

                                // enable
                                if (!as1.enabled) as1.enabled = true;
                                // volume up
                                if (as1.volume < m_target_volume) as1.volume += m_crossfadeSpeed * Time.unscaledDeltaTime;
                                else if (as1.volume >= m_target_volume) as1.volume = m_target_volume;
                        }
                        else
                        {
                                if (as1.enabled)
                                {
                                        as1.volume -= m_crossfadeSpeed * Time.unscaledDeltaTime;
                                        if (as1.volume <= 0)
                                        {
                                                as1.volume = 0;
                                                as1.enabled = false;
                                        }
                                }

                                // enable
                                if (!as2.enabled) as2.enabled = true;
                                // volume up
                                if (as2.volume < m_target_volume) as2.volume += m_crossfadeSpeed * Time.unscaledDeltaTime;
                                else if (as2.volume >= m_target_volume) as2.volume = m_target_volume;
                        }
                }
        }
}