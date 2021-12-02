using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LevelManaging;
using UnityEngine;

namespace PlayManagement
{
	public class GameController : MonoBehaviour
	{
		public CanvasSystem.CanvasSystemOperator mainMenuCanvas;
		public CanvasSystem.CanvasSystemOperator ingameCanvas;
		public CanvasSystem.CanvasSystemOperator winMenuCanvas;

		// list of all of the canvases
		private CanvasSystem.CanvasSystemOperator[] canvases;

		public bool gameplayMechanicsActive = false;

		#region handy refs
		LevelManaging.LevelManager levelManager => References.levelManager;
		PlayerPressManager playerPressManager => References.playerPressManager;
		#endregion

		private void Awake()
		{
			// make canvases ready
			canvases = new CanvasSystem.CanvasSystemOperator[] {
				mainMenuCanvas,
				ingameCanvas,
				winMenuCanvas
			};
		}

		private void Start()
		{
			OpenMainMenu ();
		}



		private void Update()
		{
			if (gameplayMechanicsActive)
			{
				levelManager.Tick ();
			}
		}

		/// <summary>
		/// opens main menu, disables gameplay and handles timescale. it's safe to use anywhere.
		/// </summary>
		public void OpenMainMenu()
		{
			//  enable/disable
			DisableAllCanvasesExceptFor (mainMenuCanvas);

			// gameplay stuff disabled
			DisableAllGameplayMechanics (timeScale_zero: true, stopInputingAbruptly: true);
		}


		public void OnWinLevel()
		{
			Debug.Log ($"here");
			// enable/disable
			DisableAllCanvasesExceptFor (winMenuCanvas);

			Debug.Log ($"then here");
			// disable functionalities
			DisableAllGameplayMechanics (timeScale_zero: true, stopInputingAbruptly: true);
			Debug.Log ($"won the game fully");
			Debug.Log ($"win canvas active {winMenuCanvas.enabled}");
		}

		public void OnLoseLevel()
		{
			OpenMainMenu ();
		}

		public void StartGame()
		{
			// enable/disable
			DisableAllCanvasesExceptFor (ingameCanvas);

			// set data
			gameplayMechanicsActive = true;

			// start gameplay functionalities
			EnableAllGameplayMechanics (timeScale_one: true);
			levelManager.StartLevel ();
		}

		private void DisableAllCanvasesExceptFor(params CanvasSystem.CanvasSystemOperator[] exceptionCanvases)
		{
			foreach (var canvas in this.canvases)
			{
				foreach (var exceptioncanvas in exceptionCanvases)
				{
					if (canvas == exceptioncanvas)
					{
						// enable this and go for next canvas
						canvas.enabled = true;
						goto aftersearch;
					}
				}

				// if reached here, it should be disabled
				canvas.enabled = false;
			// go for next canvas
			aftersearch: continue;
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
	}
}
