using UnityEngine;

namespace AudioSystem
{
    public class BackgroundMusic : MonoBehaviour
    {
        public AudioSource as1, as2;
        bool is_as1_on = true;
        float m_crossfadeSpeed = 0;
        float m_target_volume = 1;

        public void Play(AudioClip audio, float volume = 1, float crossfadeSpeed = 0.5f)
        {
            if (is_as1_on) { as2.clip = audio; as2.Play(); }
            else { as1.clip = audio; as1.Play(); }
            is_as1_on = !is_as1_on;

            m_crossfadeSpeed = crossfadeSpeed;
            m_target_volume = volume;
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