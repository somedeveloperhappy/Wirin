using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;

namespace FlatTheme.MainMenuUI.UpgradeItems
{
    public class TrinonPowerUpgradeButton : UpgradeItemButton<UpgradeSystem.UpgradeItems.TrinonPower>
    {
        #region refs
        public Button button;
        public Text costTxt, lvlText;
        public string fullUpgradeText = "Full";
        public UpgradeManager upgradeManager;
        #endregion

        [System.Serializable]
        public class Settings
        {
            public Color costTxtDisabledColor;
            public Color costTxtEnabledColor;
        }
        public Settings settings;

        [ContextMenu("Auto Resolve")]
        public void AutoResolve()
        {
            button = GetComponent<Button>();
            button ??= GetComponentInChildren<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener( Upgrade );

            foreach (var t in GetComponentsInChildren<Text>())
                if (lvlText == null && t.name.ToLower().Contains( "lvl" ) || t.name.ToLower().Contains( "level" ))
                    lvlText = t;
                else if (costTxt == null && t.name.ToLower().Contains( "cost" ))
                    costTxt = t;

            upgradeManager = FindObjectOfType<UpgradeManager>();

        }

        public override void OnCanvasDisable() { }

        public override void OnCanvasEnable()
        {
            updateValues();
        }

        private void updateValues()
        {
            bool canupgrade = upgradeItem.CanBeUpgraded();
            bool fullyUpgraded = upgradeItem.IsFullyUpgraded();
            
            costTxt.text = fullyUpgraded ? "" : upgradeItem.GetNextCost().ToString();
            
            if (canupgrade)
            {
                costTxt.color = settings.costTxtEnabledColor;
                button.interactable = true;
            }
            else
            {
                costTxt.color = settings.costTxtDisabledColor;
                button.interactable = false;
            }

            lvlText.text = upgradeItem.IsFullyUpgraded() ? fullUpgradeText : upgradeItem.GetUpgradeLevel().ToString();
        }

        public override void Upgrade()
        {
            if (upgradeItem.CanBeUpgraded())
            {
                upgradeManager.Upgrade( upgradeItem );
                updateValues();
            }
        }
    }
}
