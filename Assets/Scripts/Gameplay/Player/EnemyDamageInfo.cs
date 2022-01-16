using System;

namespace Gameplay.Player
{
    [Serializable]
    public struct PlayerBulletDamageInfo
    {
        public float damage;
        public PlayerBulletDamageInfo(float damage)
        {
            this.damage = damage;
        }
    }
}