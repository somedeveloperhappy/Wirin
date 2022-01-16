using CanvasSystem;
using PlayManager;
using UnityEngine;

namespace Management
{
    public class GameController : MonoBehaviour
    {

        // list of all of the canvases
        private CanvasBase[] canvases;

        public bool gameplayMechanicsActive;
        public CanvasBase ingameCanvas,
            mainMenuCanvas,
            winMenuCanvas,
            loseMenuCanvas,
            upgradeMenuCanvas,
            pauseMenuCanvas;
        [HideInInspector] public bool isPaused = false;

        public AudioClip gameBackgroundMusic;

        private void Awake()
        {
            // make canvases ready
            canvases = new[]
            {
                mainMenuCanvas,
                ingameCanvas,
                winMenuCanvas,
                loseMenuCanvas,
                upgradeMenuCanvas,
                pauseMenuCanvas
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
            // enable/disable
            DisableAllCanvasesExceptFor(winMenuCanvas);

            // disable functionalities
            DisableAllGameplayMechanics(true, true);
            Debug.Log($"win canvas active {winMenuCanvas.enabled}");

            // save
            References.playerInfo.moneyManager.Save();

        }

        public void OnLoseLevel()
        {
            // enable/disable canvas
            DisableAllCanvasesExceptFor(loseMenuCanvas);

            // set data
            gameplayMechanicsActive = false;

            // stop gameplay for good
            DisableAllGameplayMechanics(timeScaleTo0: true, stopInputingAbruptly: true);

            // stop gameplay music
            References.backgroundMusic.Play(null);

            // save
            References.playerInfo.moneyManager.Save();
        }

        public void StartGame() => StartGame(null, true);
        public void StartGame(CanvasBase ignorecanvas = null, bool setTimeScaleTo1 = true)
        {
            // enable/disable
            DisableAllCanvasesExceptFor(ingameCanvas, ignorecanvas);

            // set data
            gameplayMechanicsActive = true;

            // start gameplay functionalities
            EnableAllGameplayMechanics(setTimeScaleTo1);
            levelManager.StartLevel();

            // play the ingame background music
            References.backgroundMusic.Play(gameBackgroundMusic);
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
        public CanvasBase[] GetEnabledCanvases()
        {
            System.Collections.Generic.List<CanvasBase> r = new System.Collections.Generic.List<CanvasBase>();
            for (int i = 0; i < canvases.Length; i++)
                if (canvases[i].enabled) r.Add(canvases[i]);
            return r.ToArray();
        }

        public void DisableAllGameplayMechanics(bool timeScaleTo0 = false, bool stopInputingAbruptly = false)
        {
            if (stopInputingAbruptly)
            {
                References.playerInfo.enabled = false;
                References.playerPressManager.enabled = false;
            }
            else
            {
                References.playerPressManager.canGetInputs = false;
            }

            if (timeScaleTo0) Time.timeScale = 0;
            gameplayMechanicsActive = false;
        }

        public void EnableAllGameplayMechanics(bool timeScale_one = false)
        {
            References.playerInfo.enabled = true;
            References.playerPressManager.enabled = true;
            References.playerPressManager.canGetInputs = true;

            if (timeScale_one) Time.timeScale = 1;
            References.playerInfo.enabled = true;

            gameplayMechanicsActive = true;

            // make sure gameplay objects are enabled
            References.gameplayObjects.SetActive(true);
        }

        #region handy refs

        private LevelManager levelManager => References.levelManager;
        private PlayerPressManager playerPressManager => References.playerPressManager;

        #endregion


        public void EndGameplay(bool timeScaleTo0 = true)
        {
            // resetting all resettables
            for (int i = OnGameplayEnd.instances.Count - 1; i >= 0; i--)
                OnGameplayEnd.instances[i]?.OnGameplayEnd();

            if (timeScaleTo0) Time.timeScale = 0;

            DisableAllGameplayMechanics(timeScaleTo0: timeScaleTo0, stopInputingAbruptly: true);
        }

        public bool isPlayerAlive() => References.playerInfo.GetHealth() > 0;

        public void PauseGame()
        {
            if (isPaused) return;
            isPaused = true;
            Time.timeScale = 0;
            DisableAllCanvasesExceptFor(pauseMenuCanvas);
        }
        public void ResumeGame()
        {
            if (!isPaused) return;
            Time.timeScale = 1;
            DisableAllCanvasesExceptFor(ingameCanvas);

        }
        public void ExitGame()
        {
            Application.Quit(0);
        }
    }
}
