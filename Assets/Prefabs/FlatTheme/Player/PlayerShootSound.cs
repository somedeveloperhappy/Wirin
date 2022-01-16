using Gameplay;
using Gameplay.PressSystem;
using SimpleScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatTheme.Player
{
    public class PlayerShootSound : MonoBehaviour, Gameplay.IOnPlayerPress
    {
        #region quick refs
        Gameplay.Player.PlayerInfo playerInfo => References.playerInfo;
        UnityEngine.Audio.AudioMixerGroup ingameAudioGroup => References.ingameMixerGroup;
        UnityEngine.Audio.AudioMixer audioMixer => References.audioMixer;
        #endregion

        [System.Serializable]
        public struct ChargeSoundSettings
        {
            public AudioSource source;
            public AudioClip[] clips;
            public MinMax pitch;
            public AnimationCurve volumeCurve;
            [Range(0f, 1f)] public float volumeMultiplier;
        }
        public ChargeSoundSettings m_chargeSoundSettings;

        [System.Serializable]
        public struct ReverbSettings
        {
            public MinMax amount;
            public string parameterName;
        }
        public ReverbSettings m_reverbSettings;

        private void Awake()
        {
            //this.enabled = false;
            //m_settings.source.enabled = false;
            m_chargeSoundSettings.source.Stop();
            this.OnPlayerPressInit();
        }

        private void OnDestroy()
        {
            this.OnPlayerPressDestroy();
        }

        void Apply(float normalizedT)
        {
            // charge sound
            m_chargeSoundSettings.source.volume = m_chargeSoundSettings.volumeCurve.Evaluate(normalizedT);
            m_chargeSoundSettings.source.pitch = m_chargeSoundSettings.pitch.Evaluate(normalizedT) * m_chargeSoundSettings.volumeMultiplier;

            // reverb settings
            audioMixer.SetFloat(m_reverbSettings.parameterName, m_reverbSettings.amount.Evaluate(normalizedT));
        }

        public void OnPressDown(float duration)
        {
            // on start
            m_chargeSoundSettings.source.clip = m_chargeSoundSettings.clips[Random.Range(0, m_chargeSoundSettings.clips.Length)];
            m_chargeSoundSettings.source.Play();
        }

        public void OnPressUp(float duration)
        {

        }

        public void OnPressDownUpdate()
        {
            Apply(playerInfo.GetNormalCharge());
        }

        public void OnPressUpUpdate()
        {
            if (!m_chargeSoundSettings.source.isPlaying) return;

            Apply(playerInfo.GetNormalCharge());
            if (playerInfo.GetNormalCharge() <= 0)
            {
                // on stop
                m_chargeSoundSettings.source.Stop();
            }

        }
    }
}
