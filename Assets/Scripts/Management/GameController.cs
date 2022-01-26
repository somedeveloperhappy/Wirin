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
                    pauseMenuCanvas,
                    highscoreMenuCanvas,
                    submitscoreMenuCanvas;
                [HideInInspector] public bool isPaused = false;

                #region score
                /// <summary>
                /// the score for the current game
                /// </summary>
                private decimal current_score = 0;
                static public bool shouldShowSubmitScore = false;
                static public decimal lastGameScore = 0;
                #endregion

                #region  events
                public System.Action onRetry;
                public System.Action onStartGame;
                #endregion
                public decimal CurrentScore => current_score;
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
                pauseMenuCanvas,
                highscoreMenuCanvas,
                submitscoreMenuCanvas
            };
                }

                private void Start()
                {
                        OpenMainMenu();
                        if (shouldShowSubmitScore)
                        {
                                Debug.Log("shouldShowSubmitScore is true");
                                submitscoreMenuCanvas.enabled = true;
                        }
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
                        // add to scores
                        current_score += levelManager.levelStats.pointsTaken;

                        // enable/disable
                        DisableAllCanvasesExceptFor(winMenuCanvas);

                        // disable functionalities
                        DisableAllGameplayMechanics(true, true);
                        Debug.Log($"win canvas active {winMenuCanvas.enabled}");

                        // save
                        References.playerInfo.moneyManager.Save();
                }

                public void OpenLoseMenu()
                {
                        // enable/disable canvas
                        DisableAllCanvasesExceptFor(loseMenuCanvas);

                        // set data
                        gameplayMechanicsActive = false;

                        // stop gameplay for good
                        DisableAllGameplayMechanics(timeScaleTo0: true, stopInputingAbruptly: true);

                        // save
                        References.playerInfo.moneyManager.Save();

                        // silencing ingame sfx
                        References.ingame_sfx.mixerFadeOut();
                }
                public void Retry()
                {
                        // set player health full
                        References.playerInfo.SetHealth(References.playerInfo.GetMaxHealth());
                        onRetry?.Invoke();
                        EnableAllGameplayMechanics(timeScale_one: false);
                        DisableAllCanvasesExceptFor(exceptionCanvases: ingameCanvas);

                        gameplayMechanicsActive = true;

                        // resume ingame sfx
                        References.ingame_sfx.mixerFadeIn();
                }

                public void SaveGameScoreAndReset()
                {
                        // score starts from zero
                        lastGameScore = current_score + levelManager.levelStats.pointsTaken;
                        Debug.Log($"lost with {lastGameScore} scores");
                        current_score = 0;
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

                        // disable the show submit menu for the time being
                        shouldShowSubmitScore = false;

                        // fade in sfx sounds for ingame
                        References.ingame_sfx.mixerFadeIn();

                        onStartGame?.Invoke();
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
                                                break;
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

                        // silencing ingame sfx
                        References.ingame_sfx.mixerFadeOut();
                }
                public void ResumeGame()
                {
                        if (!isPaused) return;
                        Time.timeScale = 1;
                        DisableAllCanvasesExceptFor(ingameCanvas);

                        // resuming ingame sfx
                        References.ingame_sfx.mixerFadeIn();
                }
                public void ExitGame()
                {
                        Application.Quit(0);
                }
        }
}
