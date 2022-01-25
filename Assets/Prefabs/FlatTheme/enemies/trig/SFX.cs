using System;
using UnityEngine;

namespace FlatTheme.enemies.trig
{
        public class SFX : MonoBehaviour
        {
                public Gameplay.EnemyNamespace.Types.Trig.EnemyBaseTrig enemy;
                public AudioClip destructionSound;
                public float volume = 1;

                [ContextMenu("Auto Resolve")]
                public void AutoResolve()
                {
                        enemy = FindObjectOfType<Gameplay.EnemyNamespace.Types.Trig.EnemyBaseTrig>();
                }
                private void OnEnable()
                {
                        enemy.onDestroy += onDesruction;
                }

                private void onDesruction()
                {
                        References.ingame_sfx.Play(destructionSound, volume);
                        enemy.onDestroy -= onDesruction;
                }
        }
}