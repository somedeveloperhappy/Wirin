using Gameplay.EnemyNamespace.Types;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace FlatTheme
{
    public class EnemyLifeShow : MonoBehaviour
    {
        [FormerlySerializedAs("enemy")] public EnemyBase enemyBase;

        [SerializeField] private OnDamageFx onDamageFx;

        private float previousHealth;
        private float speed;

        private float t; // timer for lero effects
        public TMPro.TextMeshPro textMesh;


        private void Start()
        {
            normal_col = textMesh.color;
            normal_scale = transform.localScale;

            enemyBase.onTakeDamage += onTakeDamage;
        }
        private void OnDestroy()
        {
            enemyBase.onTakeDamage -= onTakeDamage;
        }

        private void onTakeDamage(float previousHealth, PlayerBulletDamageInfo damageInfo)
        {
            this.previousHealth = previousHealth;

            t = 0;
            ApplyEffectBasedOnT();
        }

        private void Update()
        {
            if (t >= 1) return;

            t += Time.deltaTime;

            if (t >= 1)
            {
                t = 1;
                backToNormal();
            }
            else
            {
                ApplyEffectBasedOnT();
            }
        }

        private void ApplyEffectBasedOnT()
        {
            textMesh.text = Mathf.CeilToInt(Mathf.Lerp(previousHealth, enemyBase.Health, t)).ToString();
            textMesh.color = Color.Lerp(onDamageFx.color, normal_col, t);
            transform.localScale = Vector3.Lerp(onDamageFx.scale, normal_scale, t);
        }

        private void backToNormal()
        {
            textMesh.text = Mathf.CeilToInt(enemyBase.Health).ToString();
            textMesh.color = normal_col;
            transform.localScale = normal_scale;
        }

        #region normals

        private Color normal_col;
        private Vector3 normal_scale;

        #endregion

    }
}
