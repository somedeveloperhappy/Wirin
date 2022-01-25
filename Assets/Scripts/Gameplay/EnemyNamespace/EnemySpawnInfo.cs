using Gameplay.EnemyNamespace.Types;
using UnityEngine;

namespace Gameplay.EnemyNamespace
{
    [CreateAssetMenu]
    public class EnemySpawnInfo : ScriptableObject
    {
        [Tooltip( "The chance to spawn, when is requested to. x being the level number" )]
        public AnimationCurve spawnChance;

        // public ThemeSystem.ThemeBased<EnemyBase> prefab;
        [System.Serializable]
        struct Prefabs
        {
            public EnemyBase flatPrefab;
        }
        [SerializeField] Prefabs prefabs;
        public EnemyBase prefab
        {
            get
            {
                switch (GlobalSettings.theme)
                {
                    case GlobalSettings.Theme.flat:
                        return prefabs.flatPrefab;
                    default:
                        return prefabs.flatPrefab;
                }
            }
        }

        [Tooltip( "This enemyBase will not be spawned in a level lower than this" )]
        public int startingLevel;

        public bool canSpawn(int level) => level >= startingLevel;

    }
}
