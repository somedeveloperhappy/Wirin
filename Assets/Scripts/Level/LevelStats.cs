using UnityEngine;

namespace LevelManaging
{
	[System.Serializable]
	public class LevelStats
	{
		/// <summary>
		/// the points the new spawned enemies will have
		/// </summary>
		[SerializeField] internal int spawningPoints;

		/// <summary>
		/// the randomizer in addition to spawningPoints
		/// </summary>
		[SerializeField] internal int spawningPointsSpread;

		/// <summary>
		/// the points that when reached, this level is won
		/// </summary>
		[SerializeField] internal int goalPoints;

		/// <summary>
		/// the points taken so far
		/// </summary>
		[SerializeField] internal int pointsTaken;

		/// <summary>
		/// time delay for spawning next enemy
		/// </summary>
		[SerializeField] internal float spawnTimeRate;

		/// <summary>
		/// a randomizer in addition to spawnTimeRate
		/// </summary>
		[SerializeField] internal float spawnTimeRateSpread;

		public float GetSpawnTime() => spawnTimeRate + Random.Range (-spawnTimeRateSpread, spawnTimeRateSpread);

		public int GetSpawningPoint() => spawningPoints + Random.Range (-spawningPointsSpread, spawningPointsSpread);



		public LevelStats(int level)
		{
			// a normalized value of the current level. if level is 0, this value is 1. if level is infinite, this value is 1
			float normalized_level = 1 - Mathf.Exp (-0.005f * level);


			this.goalPoints = (int) (3000f * (Mathf.Sin (0.3f * level) + 0.3f * level) - 1000);

			this.spawningPoints =
				(int) ((1f / Mathf.Pow (level, 0.5f) *
						(Mathf.Pow (this.goalPoints, 1.15f) * ((1f - (normalized_level / 2f)) / 10f)) +
						(10 * level)));
			this.spawningPointsSpread =
				(int) (this.spawningPoints / (10f - (normalized_level * 5f)));

			// the total predicted game time
			float gameTime = 6 * Mathf.Sqrt (level) + 30f + (level / 2f);

			// almost the number of enemies
			float enemyNumbers = (float) this.goalPoints / (float) this.spawningPoints;

			this.spawnTimeRate = gameTime / enemyNumbers;

			this.spawnTimeRateSpread = this.spawnTimeRate / (5 + normalized_level * 3f);

			// reseting the points taken
			this.pointsTaken = 0;
		}
	}
}