using System;
using SimpleScripts;
using UnityEngine;

namespace FlatTheme.Player
{
        public class PlayerSFX : MonoBehaviour
        {
                public Gameplay.Player.PlayerInfo playerInfo;

                [System.Serializable]
                public struct Shoot
                {
                        public Sound shoot_mini, shoot_big;
                }
                public Shoot shoot;

                [System.Serializable] public struct TrinonChange
                {
                        public Sound addSound, removeSound;
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

                private void onTrinonRemove() => References.ingame_sfx.Play(trinonChange.removeSound.clip, trinonChange.removeSound.volume);
                private void onTrinonAdd() => References.ingame_sfx.Play(trinonChange.addSound.clip, trinonChange.addSound.volume);


                private void OnShoot()
                {
                        float charge = playerInfo.GetNormalCharge();

                        References.ingame_sfx.Play(shoot.shoot_mini.clip, shoot.shoot_mini.volume * (1 - charge));
                        References.ingame_sfx.Play(shoot.shoot_big.clip, shoot.shoot_big.volume * charge);
                }
        }
}