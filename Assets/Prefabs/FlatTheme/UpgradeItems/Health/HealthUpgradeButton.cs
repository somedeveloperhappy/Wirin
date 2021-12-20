using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlatTheme.UpgradeItems.Health
{
    public class HealthUpgradeButton : UpgradeSystem.UpgradeItemButton<UpgradeSystem.UpgradeItems.PivotHealth>
    {
        #region refs
        public Button button;
        public Text costText, levelText;
        public string fullUpgradeTxt = "Full";
        public UpgradeSystem.UpgradeManager upgradeManager;
        #endregion

        [System.Serializable]
        public class Settings
        {
            public Color costTxtDisabledColor;
            public Color costTxtEnabledColor;
        }
        public Settings settings;

        [ContextMenu( "Auto Resolve" )]
        public void AutoResolve()
        {
            foreach (var item in GetComponentsInChildren<Text>())
            {
                if (costText == null && item.name.ToLower().Contains( "cost" ))
                {
                    costText = item;
                }
                else if (levelText == null && item.name.ToLower().Contains( "lvl" ) || item.name.ToLower().Contains( "level" ))
                {
                    levelText = item;
                }
            }
            upgradeManager = FindObjectOfType<UpgradeSystem.UpgradeManager>();
            if (button == null)
                button = GetComponentInChildren<Button>();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener( Upgrade );
        }
        public override void OnCanvasEnable()
        {
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            bool canUpgrade = upgradeItem.CanBeUpgraded();
            bool fullyUpgraded = upgradeItem.IsFullyUpgraded();

            costText.text = fullyUpgraded ? "" : upgradeItem.GetNextCost().ToString();

            if (canUpgrade)
            {
                costText.color = settings.costTxtEnabledColor;
                button.interactable = true;
            }
            else
            {
                costText.color = settings.costTxtDisabledColor;
                button.interactable = false;
            }

            levelText.text = upgradeItem.IsFullyUpgraded() ? fullUpgradeTxt : upgradeItem.GetUpgradeLevel().ToString();
        }

        public override void OnCanvasDisable() { }


        public override void Upgrade()
        {
            if (upgradeItem.CanBeUpgraded())
            {
                upgradeManager.Upgrade( upgradeItem );
                UpdateVisuals();
            }
        }
    }
}