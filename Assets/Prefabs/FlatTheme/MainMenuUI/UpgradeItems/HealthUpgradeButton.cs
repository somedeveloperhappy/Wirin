using UnityEngine;
using UnityEngine.UI;

namespace FlatTheme.MainMenuUI.UpgradeItems
{
    public class HealthUpgradeButton : MonoBehaviour
    {
        public UpgradeSystem.UpgradeItems.TrinonPower upgradeItem;
        // public string itemName;
        // public UpgradeSystem.UpgradeItems.TrinonPower trinonPowerItem;
        public Button button;
        public Text costText, levelText;
        public Image disableOverlay;

        public void Start()
        {
            Debug.Log($"{name} enabled!");
            UpdateValues();
        }

        public void UpdateValues()
        {
            Debug.Log($"upgrade item {upgradeItem.name} button setting up . lvl : {upgradeItem.GetUpgradeLevel()}");

            if (upgradeItem.CanBeUpgraded())
            {
                costText.text = upgradeItem.GetNextCost().ToString();
                levelText.text = upgradeItem.GetUpgradeLevel().ToString();

                if (upgradeItem.CanBeUpgraded())
                {
                    button.interactable = true;
                    disableOverlay.enabled = false;
                }
                else
                {
                    button.interactable = false;
                    disableOverlay.enabled = true;
                }
            }
            else
            {
                button.interactable = false;
                costText.text = "FULL";
                levelText.text = "FULL";
                disableOverlay.enabled = true;
            }

        }



    }
}
