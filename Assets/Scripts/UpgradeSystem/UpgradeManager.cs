using CanvasSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeSystem
{
    public class UpgradeManager : MonoBehaviour, IOnCanvasEnabled
    {
        public Gameplay.Player.PlayerInfo playerInfo;

        public UpgradeItem[] upgradeItems;

        public UI.ScrollPannel parentPannel;

        public Text coinsText;

        public void OnCanvasEnable()
        {
            Debug.Log($"onCanvasEnabled");
            LoadUpgradeItems();
            SetupUpgradeItems();

            InstantiateButtons();
            SetUpCoins();

        }

        private void SetUpCoins()
        {
            var coins = playerInfo.moneyManager.Coins;
            if (coins >= 1000000000)
                coinsText.text = string.Format("{0:0.##}", coins / 1000000000) + "B";
            if (coins >= 1000000)
                coinsText.text = string.Format("{0:0.##}", coins / 1000000) + "M";
            else if (coins >= 1000)
                coinsText.text = string.Format("{0:0.##}", coins / 1000) + "K";
            else
                coinsText.text = string.Format("{0:0.##}", coins);
        }

        public void SetupUpgradeItems()
        {
            foreach (var ui in upgradeItems)
            {
                if (ui.ShouldBeActive())
                {
                    ui.Init(playerInfo);
                    ui.Activate();
                }
            }
        }

        public void LoadUpgradeItems()
        {
            foreach (var up in upgradeItems)
                up.LoadState();
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
            if (!item) return false;
            return CanUpgrade(item);
        }
        public bool CanUpgrade(UpgradeItem item)
        {
            if (!item) return false;
            return item.CanBeUpgraded() && playerInfo.moneyManager.Coins >= item.GetNextCost();
        }


        public uint GetUpgradeItemCostByName(string itemName)
        {
            var item = GetUpgradeItemByName(itemName);
            if (!item) return 0;
            return item.GetNextCost();
        }

        public void UpgradeItemByName(string itemName)
        {
            var item = GetUpgradeItemByName(itemName);
            if (!item) return;
            item.Upgrade();
        }

        public void InstantiateButtons()
        {
            // first get rid of all the already values on the scrol pannel
            foreach (Transform obj in parentPannel.transform)
                Destroy(obj.gameObject);
            // var objs = new GameObject[upgradeItems.Length];
            for (int i = 0; i < upgradeItems.Length; i++)
            {
                var obj = upgradeItems[i].GetButtonPrefab();
                Instantiate(obj, parentPannel.transform);
                Debug.Log($"put an object there");
            }
            // reorder everything
            parentPannel.ReAutoArrangeAll();
        }
    }
}
