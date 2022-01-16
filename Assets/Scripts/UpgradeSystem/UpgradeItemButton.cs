using System.Collections.Generic;
using UnityEngine;

namespace UpgradeSystem
{
    public abstract class UpgradeItemButton<T>
        : MonoBehaviour, CanvasSystem.IOnCanvasEnabled, CanvasSystem.IOnCanvasDisabled
        where T : UpgradeItem
    {
        public T upgradeItem;
        public AudioClip popInSound;


        static protected List<UpgradeItemButton<T>> liveInstances = new List<UpgradeSystem.UpgradeItemButton<T>>();
        public virtual void OnCanvasDisable()
        {
            liveInstances.Remove(this);
        }
        public virtual void OnCanvasEnable()
        {
            UpdateVisuals();
            liveInstances.Add(this);
        }
        protected virtual void OnEnable()
        {
            //AudioSource.PlayClipAtPoint(popInSound, Vector3.zero);
            References.staticSounds.Play(popInSound);
        }
        public abstract void UpdateVisuals();
        public void Upgrade()
        {
            OnUpgrade();
            // update visuals for all live instances/buttons
            liveInstances.ForEach(instance => instance.UpdateVisuals());
        }
        protected abstract void OnUpgrade();
    }
}