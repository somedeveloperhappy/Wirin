using UnityEngine;

namespace Gameplay.EnemyNamespace.Types.FireBlaster
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] Vector2 shootPosOffset;
        [SerializeField] float rotation = 0; // based on degrees

        [HideInInspector] public bool canAim = false;

        #region refs
        public EnemyBaseFireBlast enemy;
        #endregion

        public float rotationSpeed = 10;

        [System.Serializable]
        public class BulletSettings
        {
            public FireBlastBullet bullet;
            public float damage = 0.5f;
            public float buletSpeed = 2;
        }
        public BulletSettings bulletSettings;


        public void Shoot()
        {
            Debug.Log($"shooting from {name}");
            Instantiate<FireBlastBullet>(
                bulletSettings.bullet,
                GetShootPosition(),
                transform.rotation
            ).Init(this);
        }

        private void Update()
        {
            if (canAim)
            {
                // aim at player
                transform.up = Vector3.MoveTowards(
                    transform.up,
                    enemy.target - (Vector3)GetShootPosition(),
                    Time.deltaTime * rotationSpeed);
            }
        }

        public Vector2 GetShootPosition() =>
            transform.position + transform.up * shootPosOffset.y + transform.right * shootPosOffset.x;

        public Vector2 GetShootDirection()
        {
            float rad = rotation * Mathf.Deg2Rad;
            return transform.up * Mathf.Sin(rad) + transform.right * Mathf.Cos(rad);
        }
    }
}
