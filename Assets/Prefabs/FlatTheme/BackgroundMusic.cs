using UnityEngine;

namespace FlatTheme
{
        public class BackgroundMusic : MonoBehaviour
        {
                public AudioClip[] musics;
                public float fade = 1;

                private void OnEnable()
                {
                        References.gameController.onStartGame += playMusic;
                        References.gameController.onRetry += playMusic;
                }
                private void OnDisable()
                {
                        References.gameController.onStartGame -= playMusic;
                        References.gameController.onRetry -= playMusic;
                }

                private void playMusic()
                {
                        int ind = Random.Range(0, musics.Length);
                        References.backgroundMusic.Play(musics[ind], crossfadeSpeed: fade);
                }
        }
}
