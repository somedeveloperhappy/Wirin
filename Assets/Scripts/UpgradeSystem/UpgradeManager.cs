using System;
using UnityEngine;

namespace UpgradeSystem
{
    public class UpgradeManager : MonoBehaviour
    {
        public Gameplay.Player.PlayerInfo playerInfo;
        public UpgradeItem[] upgradeItems;

        public Action onUpgrade;

        private void OnEnable() => SetupUpgradeItems();
        public void SetupUpgradeItems()
        {
            foreach (var up in upgradeItems)
                up.LoadState();
            foreach (var ui in upgradeItems)
            {
                ui.Init(playerInfo);
                ui.Activate();
            }
        }


        public UpgradeItem GetUpgradeItemByName(string itemName)
        {
            foreach (var item in upgradeItems)
                if (item.name.ToLower() == itemName.ToLower())
                    return item;

            Debug.Log($"Item {itemName} does not exist");
            return null;
        }

        public bool CanUpgrade(String itemName)
        {
            var item = GetUpgradeItemByName(itemName);
            return CanUpgrade(item);
        }

        public bool CanUpgrade(UpgradeItem item) => (!item) ? false : item.CanBeUpgraded();

        public uint GetUpgradeItemCostByName(string itemName)
        {
            var item = GetUpgradeItemByName(itemName);
            if (!item) return 0;
            return item.GetNextCost();
        }

        public void Upgrade(UpgradeItem upgradeItem)
        {
            if (upgradeItem.CanBeUpgraded())
            {
                uint cost = upgradeItem.GetNextCost();
                upgradeItem.Upgrade();
                playerInfo.moneyManager.Coins -= cost;
                playerInfo.moneyManager.Save();
                onUpgrade?.Invoke();
            }
        }

    }
}
