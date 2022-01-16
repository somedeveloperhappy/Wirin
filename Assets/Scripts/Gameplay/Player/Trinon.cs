using UnityEngine;

namespace Gameplay.Player
{
    public class Trinon : MonoBehaviour
    {
        #region general settigns
        [System.Serializable]
        public class ShootPosSetting
        {
            public Vector2 bulletPosOffset;
        }
        public ShootPosSetting shootPosSetting;

        [HideInInspector]
        public float bulletDamageMultiplier = 1;
        #endregion

        #region refs
        public PlayerInfo playerInfo;
        #endregion

        public PlayerInfo GetPlayerInfo() => playerInfo;
        public Vector3 GetBulletPositionInWorld() => transform.position + transform.up * shootPosSetting.bulletPosOffset.y + transform.right * shootPosSetting.bulletPosOffset.x;

        public void Shoot(PlayerNormalBullet bulletPrefab)
        {
            var bul = Instantiate(bulletPrefab, GetBulletPositionInWorld(), transform.rotation);
            bul.damageMultiplier = bulletDamageMultiplier;
            bul.Init(playerInfo);
        }
    }
}
