using UnityEngine;

namespace AudioSystem
{
    public class StaticSounds : MonoBehaviour
    {
        AudioSource m_audioSource;

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
    }
}