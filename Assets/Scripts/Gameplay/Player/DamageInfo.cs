using System;

namespace Gameplay.Player
{
    [Serializable]
    public struct EnemyDamageInfo
    {
        public float damage;

        public EnemyDamageInfo(float damage)
        {
            this.damage = damage;
        }
    }
}