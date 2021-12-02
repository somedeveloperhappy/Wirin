using Enemies;
using UnityEngine;

namespace InGameFX
{
    public class PointsTextFx : MonoBehaviour
    {
        public UnityEngine.UI.Text text;

        [Tooltip(
            "the showing points will take this long to reach the actual points, recalculated each time we got a new points")]
        public float showInSeconds = 1;


        #region handy refs

        LevelManaging.LevelManager levelManager => References.levelManager;
        int pointsTaken => levelManager.levelStats.pointsTaken;

        #endregion

        private float showingValue = 0;
        private float speed = 0;

        private void Start() {
            levelManager.onEnemyDestroy += updateStats;
            Show();
        }

        private void updateStats(Enemy enemy) {
            speed = (pointsTaken - showingValue) / showInSeconds;
        }

        private void Update() {
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
            text.text = (int) showingValue + " / " + levelManager.levelStats.goalPoints;
        }
    }
}
