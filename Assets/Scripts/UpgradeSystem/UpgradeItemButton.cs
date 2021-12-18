using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeSystem
{
    public abstract class UpgradeItemButton<T>
        : MonoBehaviour, CanvasSystem.IOnCanvasEnabled, CanvasSystem.IOnCanvasDisabled
        where T : UpgradeItem
    {
        public T upgradeItem;

        public abstract void OnCanvasDisable();
        public abstract void OnCanvasEnable();
        public abstract void Upgrade();
    }
}