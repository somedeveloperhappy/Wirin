using Gameplay.EnemyNamespace.Types;
using UnityEngine;

namespace Gameplay.EnemyNamespace
{
	[CreateAssetMenu]
	public class EnemySpawnInfo : ScriptableObject
	{
		[Tooltip ("The chance to spawn, when is requested to. x being the level number")]
		public AnimationCurve spawnChance;

		public EnemyBase prefab;

		[Tooltip ("This enemyBase will not be spawned in a level lower than this")]
		public int startingLevel;

		public bool canSpawn(int level) => level >= startingLevel;

	}
}
