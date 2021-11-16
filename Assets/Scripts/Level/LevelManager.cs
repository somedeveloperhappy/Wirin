using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManaging
{
    public class LevelManager : MonoBehaviour
    {
        public level_making.LevelContaining levelContaining;
        public LineSegments lineSegments;

        public float next_spawn_time;


        public int level = 0;

        private void Awake() {
            LoadLevel ();
        }

        private void LoadLevel() => level = PlayerPrefs.GetInt ("lvl", 1);
        private void SaveLevel() => PlayerPrefs.SetInt ("lvl", level);

        public void Init() {
            StartLevel ();
        }
        public void Tick() {
            CheckForSpawn ();
        }

        public void StartLevel() {
            level_making.LevelsMaker.UpdateLevelValues (ref levelContaining, level);
        }

        public void WinLevel() {
            level++;
            SaveLevel ();
            StartLevel ();
        }

        public void OnEnemyDestroy(Enemy enemy) {
            levelContaining.pointsTaken += enemy.points;
            Debug.Log ($"enemy died. checking for win...");
            CheckForWin (enemy);
        }

        public void CheckForWin(Enemy enemy) {
            if ((Enemy.instances.Count < 1 || (Enemy.instances.Count == 1 && Enemy.instances[ 0 ] == enemy)) && levelContaining.pointsTaken >= levelContaining.goalPoints) {
                WinLevel ();
            } else {
                Debug.Log ($"did not win yet. points remaining : {levelContaining.goalPoints - levelContaining.pointsTaken} and {Enemy.instances.Count} enemies left");
            }
        }


        private void CheckForSpawn() {
            if (CanSpawn ()) {
                Spawn ();
                next_spawn_time += levelContaining.GetSpawnTime ();
                Debug.Log ($"spawned an enemy. next spawn at {next_spawn_time}");
            }
        }

        private bool CanSpawn() {
            return Time.timeSinceLevelLoad >= next_spawn_time && levelContaining.goalPoints > levelContaining.pointsTaken;
        }

        private void Spawn() {

            int enem_index = UnityEngine.Random.Range (0, References.enemies.Length);
            Vector2 position = lineSegments.GetPoint (UnityEngine.Random.Range (0f, lineSegments.maximumX));
            var enem = Instantiate<Enemy> (References.enemies[ enem_index ], position, Quaternion.identity);
            enem.Init (levelContaining.GetSpawningPoint ());
        }

    }
}
