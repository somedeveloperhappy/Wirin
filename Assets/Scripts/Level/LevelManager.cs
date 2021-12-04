using System;
using System.Collections;
using Gameplay.EnemyNamespace.Types;
using PlayManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelManaging
{
	public class LevelManager : MonoBehaviour
	{

		public int levelNumber;


		public LevelStats levelStats;
		public LineSegments lineSegments;

		public float next_spawn_time;

#region handy refs

		public GameController gameController => References.gameController;

#endregion

		public bool WaitingForWin { get; } = false;

		private void Awake()
		{
			LoadLevelNumberFromPrefs();
		}

		public void LoadLevelNumberFromPrefs()
		{
			levelNumber = PlayerPrefs.GetInt("lvl", 1);
		}

		private void SaveLevelNumberToPrefs()
		{
			PlayerPrefs.SetInt("lvl", levelNumber);
		}

		public void Tick()
		{
			if (WaitingForWin) return;
			CheckForSpawn();
		}

		public void StartLevel()
		{
			levelStats = new LevelStats(levelNumber);
			// let an enemy be spawned right at the first frame
			next_spawn_time = Time.timeSinceLevelLoad;
			onStartLevel?.Invoke();
		}

		public IEnumerator WinLevel()
		{
			Debug.Log($"won level {levelNumber} ... ");
			levelNumber++;
			SaveLevelNumberToPrefs();

			gameController.DisableAllGameplayMechanics();

			// timescale towards 0
			while (Time.timeScale > 0.01f)
			{
				Time.timeScale = Mathf.Lerp(Time.timeScale, 0, WIN_DELAY_WAIT * Time.unscaledDeltaTime);
				yield return null; // wait for next frame
			}

			Time.timeScale = 0;

			References.gameController.OnWinLevel();
		}

		public IEnumerator LostLevel()
		{
			Debug.Log($"lost on level {levelNumber}");
			SaveLevelNumberToPrefs(); // for making sure

			// wait and lose
			gameController.DisableAllGameplayMechanics(stopInputingAbruptly: false);
			foreach (var enem in EnemyBase.instances) enem.enabled = false;

			// timescale towards 0
			while (Time.timeScale > 0.01f)
			{
				Time.timeScale = Mathf.Lerp(Time.timeScale, 0, WIN_DELAY_WAIT * Time.unscaledDeltaTime);
				yield return null; // wait for next frame
			}

			Time.timeScale = 0;

			gameController.OnLoseLevel();
		}

		public void OnEnemyDestroy(EnemyBase enemy)
		{
			if (WaitingForWin) return;

			levelStats.pointsTaken += enemy.points;
			Debug.Log("enemy died. checking for win...");
			CheckForWin(enemy);
			onEnemyDestroy?.Invoke(enemy);
		}

		public void CheckForWin(EnemyBase enemy)
		{

			if (WaitingForWin) return;

			if ((EnemyBase.instances.Count < 1 || EnemyBase.instances.Count == 1 && EnemyBase.instances[0] == enemy) &&
			    levelStats.pointsTaken >= levelStats.goalPoints)
				StartCoroutine(WinLevel());
			else
				Debug.Log(
					$"did not win yet. points remaining : {levelStats.goalPoints - levelStats.pointsTaken} and {EnemyBase.instances.Count} enemies left");
		}


		private void CheckForSpawn()
		{
			if (!CanSpawn()) return;

			SpawnEnemy();
			updateSpawnTime();
			Debug.Log($"spawned an enemy. next spawn at {next_spawn_time}");
		}

		private void updateSpawnTime()
		{
			next_spawn_time = Time.timeSinceLevelLoad + levelStats.GetSpawnTime();
		}

		private bool CanSpawn()
		{
			float sum_of_enemies_points = 0;
			foreach (var enem in EnemyBase.instances) sum_of_enemies_points += enem.points;

			bool time_is_ok()
			{
				return Time.timeSinceLevelLoad >= next_spawn_time;
			}

			bool points_ok()
			{
				return levelStats.goalPoints > levelStats.pointsTaken + sum_of_enemies_points;
			}

			return time_is_ok() && points_ok();
		}


		private void SpawnEnemy()
		{
			var enem_index = Random.Range(0, References.enemies.Length);
			var position = lineSegments.GetPoint(Random.Range(0f, lineSegments.maximumX));
			var enem = Instantiate(References.enemies[enem_index], position, Quaternion.identity);
			enem.Init(levelStats.GetSpawningPoint());
		}

#region consts

		private const float WIN_DELAY_WAIT = 2;
		private const float LOSE_DELAY_WAIT = 2;
		private const float CLOSE_DELAY_WAIT = 2;

#endregion

#region events

		public event Action onStartLevel;
		public event Action<EnemyBase> onEnemyDestroy;

#endregion

	}
}