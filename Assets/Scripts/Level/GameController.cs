using System;
using LevelManaging;
using UnityEngine;

namespace PlayManagement
{
    public class GameController : MonoBehaviour
    {
        public CanvasSystem.CanvasSystemOperator mainMenuCanvas;
        public CanvasSystem.CanvasSystemOperator ingameCanvas;
        public CanvasSystem.CanvasSystemOperator winMenuCanvas;
        
        public bool isPlaying = false;

        #region handy refs

        LevelManaging.LevelManager levelManager => References.levelManager;
        PlayerPressManager playerPressManager => References.playerPressManager;

        #endregion

        private void Start() {
            OpenMainMenu();
        }

        public void StartGame() {
            // set data
            isPlaying = true;

            // canvas enable/disable
            mainMenuCanvas.enabled = false;
            ingameCanvas.enabled = true;
            winMenuCanvas.enabled = false;

            // start gameplay
            Time.timeScale = 1;
            levelManager.Init();
            playerPressManager.enabled = true;
        }

        private void Update() {
            if (isPlaying) {
                levelManager.Tick();
            }
        }

        /// <summary>
        /// opens main menu, disables gameplay and handles timescale. it's safe to use anywhere.
        /// </summary>
        public void OpenMainMenu() {
            
            Time.timeScale = 0;
            playerPressManager.enabled = false;
            isPlaying = false;
            
            // canvas enable/disable
            ingameCanvas.enabled = false;
            mainMenuCanvas.enabled = true;
            winMenuCanvas.enabled = false;
            
        }
        
        public void WinGame() {
            
            // disable functionalities
            Time.timeScale = 0;
            playerPressManager.enabled = false;
            isPlaying = false;
            
            // canvas enable/disable
            ingameCanvas.enabled = false;
            mainMenuCanvas.enabled = false;
            winMenuCanvas.enabled = true;
            
            
        }

        public void StarNextGame() {
            
            // set data
            isPlaying = true;

            // canvas enable/disable
            mainMenuCanvas.enabled = false;
            ingameCanvas.enabled = true;
            winMenuCanvas.enabled = false;
            
            // start game functionalities
            Time.timeScale = 1;
            levelManager.Init();
            playerPressManager.enabled = true;
            
        }
    }
}