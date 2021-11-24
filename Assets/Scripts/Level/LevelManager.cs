using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace LevelManaging
{
    public class LevelManager : MonoBehaviour
    {
        public LevelContaining levelContaining;
        public LineSegments lineSegments;

        public float next_spawn_time;

        #region events

        public event Action onStartLevel;
        public event Action<Enemy> onEnemyDestroy;

        #endregion

        public int level = 0;

        private void Awake() {
            LoadLevel();
        }

        private void LoadLevel() => level = PlayerPrefs.GetInt("lvl", 1);
        private void SaveLevel() => PlayerPrefs.SetInt("lvl", level);

        public void Init() {
            StartLevel();
            // let an enemy be spawned right at the first frame
            next_spawn_time = Time.timeSinceLevelLoad;
        }

        public void Tick() {
            CheckForSpawn();
        }

        public void StartLevel() {
            LevelsMaker.UpdateLevelValues(ref levelContaining, level);
            onStartLevel?.Invoke();
        }

        public void WinLevel() {
            level++;
            SaveLevel();
            References.gameController.WinGame();
        }

        public void OnEnemyDestroy(Enemy enemy) {
            levelContaining.pointsTaken += enemy.points;
            Debug.Log($"enemy died. checking for win...");
            CheckForWin(enemy);
            onEnemyDestroy?.Invoke(enemy);
        }

        public void CheckForWin(Enemy enemy) {
            if ((Enemy.instances.Count < 1 || (Enemy.instances.Count == 1 && Enemy.instances[0] == enemy)) &&
                levelContaining.pointsTaken >= levelContaining.goalPoints) {
                WinLevel();
            }
            else {
                Debug.Log(
                    $"did not win yet. points remaining : {levelContaining.goalPoints - levelContaining.pointsTaken} and {Enemy.instances.Count} enemies left");
            }
        }


        private void CheckForSpawn() {
            if (!CanSpawn()) return;

            Spawn();
            updateSpawnTime();
            Debug.Log($"spawned an enemy. next spawn at {next_spawn_time}");
        }

        private void updateSpawnTime() => next_spawn_time = Time.timeSinceLevelLoad + levelContaining.GetSpawnTime();

        private bool CanSpawn() {
            float sum_of_enemies_points = 0;
            foreach (var enem in Enemy.instances) sum_of_enemies_points += enem.points;

            bool time_is_ok() => Time.timeSinceLevelLoad >= next_spawn_time;
            bool points_ok() => levelContaining.goalPoints > (levelContaining.pointsTaken + sum_of_enemies_points);
            return time_is_ok() && points_ok();
        }


        private void Spawn() {
            int enem_index = UnityEngine.Random.Range(0, References.enemies.Length);
            Vector2 position = lineSegments.GetPoint(UnityEngine.Random.Range(0f, lineSegments.maximumX));
            var enem = Instantiate<Enemy>(References.enemies[enem_index], position, Quaternion.identity);
            enem.Init(levelContaining.GetSpawningPoint());
        }
    }
}
