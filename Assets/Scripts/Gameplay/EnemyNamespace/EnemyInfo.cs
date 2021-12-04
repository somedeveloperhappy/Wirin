using Gameplay.EnemyNamespace.Types;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.EnemyNamespace
{
	[CreateAssetMenu]
	public class EnemyInfo : ScriptableObject
	{
		public EnemyBase enemyBase;
		
		[Tooltip("The chance to spawn, when is requested to. x being the level number")]
		public AnimationCurve acceptingChance;

		public EnemyBase prefab;

		[Tooltip("This enemyBase will not be spawned in a level lower than this")]
		public int startingLevel;

		public bool CanSpawn(int levelNumber)
		{
			return Random.Range(0f, 1f) <= acceptingChance.Evaluate(levelNumber);
		}
	}
}