using System;

namespace Gameplay.Player
{
	[Serializable]
	public struct PlayerBulletDamageInfo
	{
		public float damage;
		public float stunDuration;

		public PlayerBulletDamageInfo(float damage, float stunDuration)
		{
			this.damage = damage;
			this.stunDuration = stunDuration;
		}
	}
}