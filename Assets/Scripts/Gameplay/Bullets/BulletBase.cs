using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Bullets
{
    public abstract class BulletBase : MonoBehaviour, IOnGameplayEnd
    {
        static public List<BulletBase> instances = new List<BulletBase>();

        public void OnGameplayEnd()
        {
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            instances.Add(this);
            this.ResettableInit();
        }
        private void OnDisable()
        {
            this.ResettableDestroy();
            instances.Remove(this);
        }
    }
}
