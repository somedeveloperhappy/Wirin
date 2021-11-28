using System;
using System.Collections.Generic;
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

		public bool isPlaying = false;

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

		public void StartGame()
		{
			// canvas enable/disable
            DisableAllCanvasesExceptFor (ingameCanvas);

			// set data
			isPlaying = true;

			// start gameplay functionalities
			Time.timeScale = 1;
			levelManager.StartLevel ();
			playerPressManager.enabled = true;
		}

		private void Update()
		{
			if (isPlaying)
			{
				levelManager.Tick ();
			}
		}

		/// <summary>
		/// opens main menu, disables gameplay and handles timescale. it's safe to use anywhere.
		/// </summary>
		public void OpenMainMenu()
		{
			// canvas enable/disable
			DisableAllCanvasesExceptFor (mainMenuCanvas);
			Time.timeScale = 0;
			isPlaying = false;
		}


		public void OnWinLevel()
		{
			// canvas enable/disable
			DisableAllCanvasesExceptFor (winMenuCanvas);

			// disable functionalities
			Time.timeScale = 0;
			isPlaying = false;
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

	}
}
