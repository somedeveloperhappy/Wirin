using System;
using UnityEngine;

namespace FlatTheme.Player
{
        public class PlayerSFX : MonoBehaviour
        {
                public Gameplay.Player.PlayerInfo playerInfo;

                [System.Serializable]
                public struct Shoot
                {
                        public AudioClip shoot_mini, shoot_big;
                        public float volume;
                }
                public Shoot shoot;

                [System.Serializable] public struct TrinonChange
                {
                        public AudioClip addSound;
                        public float addSoundVolume;
                        public AudioClip removeSound;
                        public float removeSoundVolume;
                }
                public TrinonChange trinonChange;

                [ContextMenu("Auto resolve")]
                public void AutoResolve() 
                {
                        playerInfo = FindObjectOfType<Gameplay.Player.PlayerInfo>();
                }

                private void OnEnable()
                {
                        playerInfo.onShoot += OnShoot;
                        playerInfo.onTrinonAdd += onTrinonAdd;
                        playerInfo.onTrinonRemove += onTrinonRemove;
                }
                private void OnDisable()
                {
                        playerInfo.onShoot -= OnShoot;
                        playerInfo.onTrinonRemove -= onTrinonRemove;
                }

                private void onTrinonRemove() => References.ingame_sfx.Play(trinonChange.removeSound, trinonChange.removeSoundVolume);
                private void onTrinonAdd() => References.ingame_sfx.Play(trinonChange.addSound, trinonChange.addSoundVolume);


                private void OnShoot()
                {
                        float charge = playerInfo.GetNormalCharge();

                        References.ingame_sfx.Play(shoot.shoot_mini, shoot.volume * (1 - charge));
                        References.ingame_sfx.Play(shoot.shoot_big, shoot.volume * charge);
                }
        }
}