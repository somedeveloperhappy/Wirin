using System.Collections.Generic;
using UnityEngine;

namespace UpgradeSystem
{
        public abstract class UpgradeItemButton : MonoBehaviour
        {
                static public List<UpgradeItemButton> instances = new List<UpgradeItemButton>();

                public abstract void UpdateVisuals();
        }
        public abstract class UpgradeItemButton<T> : UpgradeItemButton, CanvasSystem.IOnCanvasEnabled
            where T : UpgradeItem
        {
                public T upgradeItem;
                public AudioClip popInSound;
                private void Awake() => instances.Add(this);
                private void OnDestroy() => instances.Remove(this);
                public virtual void OnCanvasEnable() => UpdateVisuals();
                protected virtual void OnEnable() => References.menu_sfx.Play(popInSound);
                public void Upgrade()
                {
                        OnUpgrade();
                        //update visuals for all live instances / buttons
                        foreach (var instance in instances) instance.UpdateVisuals();
                }
                protected abstract void OnUpgrade();
        }
}