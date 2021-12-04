using CanvasSystem;
using Gameplay.Player;
using LevelManaging;
using UnityEngine;

namespace PlayManagement
{
	public class GameController : MonoBehaviour
	{

		// list of all of the canvases
		private CanvasSystemOperator[] canvases;

		public bool gameplayMechanicsActive;
		public CanvasSystemOperator ingameCanvas;
		public CanvasSystemOperator mainMenuCanvas;
		public CanvasSystemOperator winMenuCanvas;

		private void Awake()
		{
			// make canvases ready
			canvases = new[]
			{
				mainMenuCanvas,
				ingameCanvas,
				winMenuCanvas
			};
		}

		private void Start()
		{
			OpenMainMenu();
		}


		private void Update()
		{
			if (gameplayMechanicsActive) levelManager.Tick();
		}

		/// <summary>
		///     opens main menu, disables gameplay and handles timescale. it's safe to use anywhere.
		/// </summary>
		public void OpenMainMenu()
		{
			//  enable/disable
			DisableAllCanvasesExceptFor(mainMenuCanvas);

			// gameplay stuff disabled
			DisableAllGameplayMechanics(true, true);
		}


		public void OnWinLevel()
		{
			Debug.Log("here");
			// enable/disable
			DisableAllCanvasesExceptFor(winMenuCanvas);

			Debug.Log("then here");
			// disable functionalities
			DisableAllGameplayMechanics(true, true);
			Debug.Log("won the game fully");
			Debug.Log($"win canvas active {winMenuCanvas.enabled}");
		}

		public void OnLoseLevel()
		{
			OpenMainMenu();
		}

		public void StartGame()
		{
			// enable/disable
			DisableAllCanvasesExceptFor(ingameCanvas);

			// set data
			gameplayMechanicsActive = true;

			// start gameplay functionalities
			EnableAllGameplayMechanics(true);
			levelManager.StartLevel();
		}

		private void DisableAllCanvasesExceptFor(params CanvasSystemOperator[] exceptionCanvases)
		{
			foreach (var canvas in canvases)
			{
				foreach (var exceptioncanvas in exceptionCanvases)
					if (canvas == exceptioncanvas)
					{
						// enable this and go for next canvas
						canvas.enabled = true;
						goto aftersearch;
					}

				// if reached here, it should be disabled
				canvas.enabled = false;
				// go for next canvas
				aftersearch: ;
			}
		}

		public void DisableAllGameplayMechanics(bool timeScale_zero = false, bool stopInputingAbruptly = false)
		{
			if (stopInputingAbruptly)
				References.playerPressManager.enabled = false;
			else
				References.playerPressManager.canGetInputs = false;

			if (timeScale_zero) Time.timeScale = 0;
			gameplayMechanicsActive = false;
		}

		public void EnableAllGameplayMechanics(bool timeScale_one = false)
		{
			References.playerPressManager.enabled = true;
			References.playerPressManager.canGetInputs = true;

			if (timeScale_one) Time.timeScale = 1;
			foreach (var pi in PlayerInfo.instances)
				pi.enabled = true;
			gameplayMechanicsActive = true;

		}

#region handy refs

		private LevelManager levelManager => References.levelManager;
		private PlayerPressManager playerPressManager => References.playerPressManager;

#endregion

	}
}