using Gameplay.EnemyNamespace.Types;
using Management;
using System.Collections;
using UnityEngine;

namespace FlatTheme.IngameMenu
{
    public class ProgressBar : MonoBehaviour, CanvasSystem.IOnCanvasEnabled
    {
        public LevelManager levelManager;

        #region references
        public RectTransform fill, fillBleed;
        #endregion

        [System.Serializable]
        public class Settings
        {
            public float fillSpeed = 1;
            public float delay = 0.25f;
        }
        public Settings settings;

        [ContextMenu("Auto Resolve")]
        private void AutoResolve()
        {
            var rects = GetComponentsInChildren<RectTransform>();
            foreach (var t in rects)
            {
                if (t.name.Contains("fill") && !t.name.Contains("bleed")) fill = t;
                if (t.name.Contains("fill") && t.name.Contains("bleed")) fillBleed = t;
            }
        }

        private void Awake()
        {
            if (!fill || !fillBleed)
                Debug.LogError("You should assign the fields fill and fillBleed in the inspector!");
        }

        private void OnEnable()
        {
            levelManager.onEnemyDestroy += OnEnemyDestroy;
        }
        private void OnDisable()
        {
            levelManager.onEnemyDestroy -= OnEnemyDestroy;
        }

        private void OnEnemyDestroy(EnemyBase obj)
        {
            StopAllCoroutines();
            StartCoroutine(Fill(Mathf.Min(1, (float)levelManager.levelStats.pointsTaken / (float)levelManager.levelStats.goalPoints)));
        }

        private IEnumerator Fill(float target)
        {
            var scale = fillBleed.localScale;
            scale.x = target;
            fillBleed.localScale = scale;

            scale = fill.localScale;

            // wait for delay
            yield return new WaitForSecondsRealtime(0.25f);

            // fill
            do
            {
                scale.x += settings.fillSpeed * Time.deltaTime;
                fill.localScale = scale;
                yield return null;
            }
            while (fill.localScale.x < target);

            // absolute assign 
            scale.x = target;
            fill.localScale = scale;
        }

        public void OnCanvasEnable()
        {
            // set it unfilled
            var scale = fill.localScale;
            scale.x = 0;
            fill.localScale = scale;

            scale = fillBleed.localScale;
            scale.x = 0;
            fillBleed.localScale = scale;
        }
    }
}
