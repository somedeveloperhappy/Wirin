using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Player;
using System;
using System.Threading.Tasks;

namespace FlatTheme.Player
{
    public class FlatPlayerInfo : Gameplay.Player.PlayerInfo
    {
        [System.Serializable]
        public class LoseSettings
        {
            public float timeDownSpeed = 1;
        }
        public LoseSettings loseSettings;


        protected override IEnumerator OnLose()
        {
            do
            {
                Time.timeScale = Mathf.Max( 0, Time.timeScale - loseSettings.timeDownSpeed * Time.unscaledDeltaTime );
                yield return null;

            } while (Time.timeScale > 0);

            // absolutes
            Time.timeScale = 0;
        }
    }
}