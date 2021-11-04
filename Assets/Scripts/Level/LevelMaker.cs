using UnityEngine;

namespace level_making
{
    static class LevelsMaker
    {
        static public void UpdateLevelValues(ref LevelContaining levelContaining, int level) {
            levelContaining.goalPoints = level * 2 + 10;
            levelContaining.spawningPoints = level * 2;
            levelContaining.spawnTimeRate = (level/10) + 10;
            levelContaining.spawnTimeRateSpread = (level / 10 + 10) / 10;
        }
    }

}
