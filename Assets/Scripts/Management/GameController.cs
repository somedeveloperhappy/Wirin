using CanvasSystem;
using Gameplay.Player;
using PlayManager;
using UnityEngine;

namespace Management
{
    public class GameController : MonoBehaviour
    {

        // list of all of the canvases
        private CanvasBase[] canvases;

        public bool gameplayMechanicsActive;
        public CanvasBase ingameCanvas;
        public CanvasBase mainMenuCanvas;
        public CanvasBase winMenuCanvas;
        public CanvasBase loseMenuCanvas;

        private void Awake()
        {
            // make canvases ready
            canvases = new[]
            {
                mainMenuCanvas,
                ingameCanvas,
                winMenuCanvas,
                loseMenuCanvas
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
            DisableAllCanvasesExceptFor( mainMenuCanvas );

            // gameplay stuff disabled
            DisableAllGameplayMechanics( true, true );
        }


        public void OnWinLevel()
        {
            Debug.Log( "here" );
            // enable/disable
            DisableAllCanvasesExceptFor( winMenuCanvas );

            Debug.Log( "then here" );
            // disable functionalities
            DisableAllGameplayMechanics( true, true );
            Debug.Log( "won the game fully" );
            Debug.Log( $"win canvas active {winMenuCanvas.enabled}" );
        }

        public void OnLoseLevel()
        {
            // enable/disable canvas
            DisableAllCanvasesExceptFor( loseMenuCanvas );

            // set data
            gameplayMechanicsActive = false;

            // stop gameplay for good
            DisableAllGameplayMechanics( timeScaleTo0: true, stopInputingAbruptly: true );
        }

        public void StartGame() => StartGame( null, true );
        public void StartGame(CanvasBase ignorecanvas = null, bool setTimeScaleTo1 = true)
        {
            // enable/disable
            DisableAllCanvasesExceptFor( ingameCanvas, ignorecanvas );

            // set data
            gameplayMechanicsActive = true;

            // start gameplay functionalities
            EnableAllGameplayMechanics( setTimeScaleTo1 );
            levelManager.StartLevel();
        }

        public void DisableAllCanvasesExceptFor(params CanvasBase[] exceptionCanvases)
        {
            bool enabled = false;

            foreach (var canvas in canvases)
            {
                enabled = false;
                foreach (var exceptioncanvas in exceptionCanvases)
                    if (canvas == exceptioncanvas)
                    {
                        // enable this and go for next canvas
                        enabled = true;
                    }

                // if reached here, it should be disabled
                canvas.enabled = enabled;
            }
        }

        public void DisableAllGameplayMechanics(bool timeScaleTo0 = false, bool stopInputingAbruptly = false)
        {
            if (stopInputingAbruptly)
                References.playerPressManager.enabled = false;
            else
                References.playerPressManager.canGetInputs = false;

            if (timeScaleTo0) Time.timeScale = 0;
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


        public void EndGameplay(bool timeScaleTo0 = true)
        {
            // resetting all resettables
            for (int i = OnGameplayEnd.instances.Count - 1; i >= 0; i--)
            {
                OnGameplayEnd.instances[i].OnGameplayEnd();
            }

            if (timeScaleTo0) Time.timeScale = 0;

            DisableAllGameplayMechanics( timeScaleTo0: timeScaleTo0, stopInputingAbruptly: true );
        }

        public bool isAnyPlayerAlive()
        {
            foreach (var r in PlayerInfo.instances)
                if (r.GetHealth() > 0) return true;
            return false;
        }
    }
}
