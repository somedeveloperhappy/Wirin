using LevelManaging;
using UnityEngine;

namespace PlayManagement
{
	public class GameController : MonoBehaviour
	{
		public Canvas mainMenuCanvas;
        public Canvas ingameCanvas;
        
		public bool isPlaying = false;

		#region handy refs

		LevelManaging.LevelManager levelManager => References.levelManager;
		PlayerPressManager playerPressManager => References.playerPressManager;

		#endregion

		private void Start() {
			Time.timeScale = 0;

			playerPressManager.enabled = false;
		}

		public void StartGame() {
            
            // set data
			isPlaying = true;

			// canvas
			mainMenuCanvas.enabled = false;
            ingameCanvas.enabled = true;

            // start gameplay
			Time.timeScale = 1;
			levelManager.Init ();
			playerPressManager.enabled = true;
		}

		private void Update() {
			if (isPlaying) {
				levelManager.Tick ();
			}
		}
	}
}
