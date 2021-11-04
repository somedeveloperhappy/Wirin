using UnityEngine;

namespace level_making
{
    [System.Serializable]
    public class LevelContaining
    {
        public float spawningPoints;

        public float goalPoints;

        public float pointsTaken;

        internal float spawnTimeRate;
        
        internal float spawnTimeRateSpread;
        
        public float GetSpawnTime() => spawnTimeRate + Random.Range(-spawnTimeRateSpread, spawnTimeRateSpread);
        
    }
}
