using Gameplay.EnemyNamespace.Types;
using Management;
using UnityEngine;
using UnityEngine.UI;

namespace InGameFX
{
    public class PointsTextFx : MonoBehaviour
    {

        private float showingValue;

        [Tooltip(
            "the showing points will take this long to reach the actual points, recalculated each time we got a new points")]
        public float showInSeconds = 1;

        private float speed;
        public Text text;

        private void Start()
        {
            levelManager.onEnemyDestroy += updateStats;
            Show();
        }

        private void updateStats(EnemyBase enemy)
        {
            speed = (pointsTaken - showingValue) / showInSeconds;
        }

        private void Update()
        {
            // check if should update
            if (showingValue != pointsTaken)
            {
                // updating showing value
                showingValue += speed * Time.deltaTime;
                if (showingValue > pointsTaken) showingValue = pointsTaken;

                // show
                Show();
            }
        }

        private void Show()
        {
            text.text = (int)showingValue + " / " + levelManager.levelStats.goalPoints;
        }


        #region handy refs

        private LevelManager levelManager => References.levelManager;
        private int pointsTaken => levelManager.levelStats.pointsTaken;

        #endregion

    }
}