using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDescription : ScriptableObject
{
	[Tooltip ("This enemy will not be spawned in a level lower than this")]
	public int startingLevel;

	[Tooltip ("The chance to spawn, when is requested to. x being the level number")]
	public AnimationCurve acceptingChance;

	public Enemy prefab;

	public bool CanSpawn(int levelNumber)
	{
		return Random.Range (0f, 1f) <= acceptingChance.Evaluate (levelNumber);
	}
}
