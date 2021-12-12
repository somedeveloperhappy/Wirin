using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Management
{
	[Serializable]
	public class LevelStats
	{

		/// <summary>
		///     the points that when reached, this level is won
		/// </summary>
		[SerializeField] internal int goalPoints;

		/// <summary>
		///     the points taken so far
		/// </summary>
		[SerializeField] internal int pointsTaken;

		/// <summary>
		///     the points the new spawned enemies will have
		/// </summary>
		[SerializeField] internal int spawningPoints;

		/// <summary>
		///     the randomizer in addition to spawningPoints
		/// </summary>
		[SerializeField] internal int spawningPointsSpread;

		/// <summary>
		///     time delay for spawning next enemy
		/// </summary>
		[SerializeField] internal float spawnTimeRate;

		/// <summary>
		///     a randomizer in addition to spawnTimeRate
		/// </summary>
		[SerializeField] internal float spawnTimeRateSpread;


		public LevelStats(int level)
		{
			// a normalized value of the current level. if level is 0, this value is 1. if level is infinite, this value is 1
			var normalized_level = 1 - Mathf.Exp(-0.005f * level);


			goalPoints = (int) (3000f * (Mathf.Sin(0.3f * level) + 0.3f * level) - 1000) / 10;

			spawningPoints =
				(int) (1f / Mathf.Pow(level, 0.5f) *
				       (Mathf.Pow(goalPoints, 1.15f) * ((1f - normalized_level / 2f) / 10f)) +
				       10 * level);
			spawningPointsSpread =
				(int) (spawningPoints / (10f - normalized_level * 5f));

			// the total predicted game time
			var gameTime = 6 * Mathf.Sqrt(level) + 30f + level / 2f;

			// almost the number of enemies
			var enemyNumbers = goalPoints / (float) spawningPoints;

			spawnTimeRate = gameTime / enemyNumbers;

			spawnTimeRateSpread = spawnTimeRate / (5 + normalized_level * 3f);

			// reseting the points taken
			pointsTaken = 0;
		}

		public float GetSpawnTime()
		{
			return spawnTimeRate + Random.Range(-spawnTimeRateSpread, spawnTimeRateSpread);
		}

		public int GetSpawningPoint()
		{
			return spawningPoints + Random.Range(-spawningPointsSpread, spawningPointsSpread);
		}
	}
}