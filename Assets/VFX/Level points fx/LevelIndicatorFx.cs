using System;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasSystem
{
    public class LevelIndicatorFx : MonoBehaviour, IOnCanvasEnabled
    {
        public Text pointsText;
        public UnityEngine.UI.Image fillImage;
        public Text levelText;

        #region handy refs

        LevelManaging.LevelManager levelManager => References.levelManager;
        int pointsTaken => levelManager.levelStats.pointsTaken;

        #endregion

        [Serializable]
        public struct Settings
        {
            [Tooltip(
                "the showing points will take this long to reach the actual points, recalculated each time we got a new points")]
            public float reachInSeconds;
        }

        public Settings settings;

        float showingValue = 0;
        float speed;

        private void Start() {
            References.levelManager.onStartLevel += onLevelStart;
            References.levelManager.onEnemyDestroy += onEnemyDestroy;
        }

        public void OnCanvasEnable() { }

        private void onEnemyDestroy(Enemy enemy) {
            // recalculating speed
            speed = (pointsTaken - showingValue) / settings.reachInSeconds;
        }

        private void onLevelStart() {
            // level number display
            levelText.text = string.Concat("Level ", levelManager.levelNumber);
            Show();
        }

        public void Update() {
            if (IsNotPlaying()) return;

            // check if should update
            if (showingValue != pointsTaken) {
                // updating showing value
                showingValue += speed * Time.deltaTime;
                if (showingValue > pointsTaken) showingValue = pointsTaken;

                // show
                Show();
            }
        }

        private void Show() {
            pointsText.text = (int) showingValue + " / " + levelManager.levelStats.goalPoints;
            fillImage.material.SetFloat("_value", showingValue / levelManager.levelStats.goalPoints);
        }

        private bool IsNotPlaying() => !References.gameController.isPlaying;
    }
}
