using UnityEngine;
using UnityEngine.UI;

namespace FlatTheme.UpgradeItems.Health
{
    public class HealthUpgradeButton : UpgradeSystem.UpgradeItemButton<UpgradeSystem.UpgradeItems.Health>
    {
        #region refs
        public Button button;
        public TMPro.TMP_Text costText, levelText;
        public string fullUpgradeTxt = "Full";
        public UpgradeSystem.UpgradeManager upgradeManager;
        public GameObject disableOverlay;

        #endregion

        [System.Serializable]
        public class Settings
        {
            public Color costTxtDisabledColor;
            public Color costTxtEnabledColor;
            public Color levelTxtDisabledColor;
            public Color levelTxtEnabledColor;
        }
        public Settings settings;

        [Space(10)]
        public Animator m_animator;
        public string inAnimName = "in_anim";

        [ContextMenu("Auto Resolve")]
        public void AutoResolve()
        {
            foreach (var item in GetComponentsInChildren<TMPro.TMP_Text>())
            {
                if (costText == null && item.name.ToLower().Contains("cost"))
                {
                    costText = item;
                }
                else if (levelText == null && item.name.ToLower().Contains("lvl") || item.name.ToLower().Contains("level"))
                {
                    levelText = item;
                }
            }

            foreach (Transform trans in transform)
            {
                if (disableOverlay == null && trans.name.ToLower().Contains("disable"))
                {
                    disableOverlay = trans.gameObject;
                }
            }
            upgradeManager = FindObjectOfType<UpgradeSystem.UpgradeManager>();
            if (button == null)
                button = GetComponentInChildren<Button>();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Upgrade);
            m_animator = GetComponent<Animator>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_animator.enabled = true;
            m_animator.Play(inAnimName);
        }
        private void OnDisable()
        {
            m_animator.StopPlayback();
            m_animator.enabled = false;
        }

        public override void UpdateVisuals()
        {
            bool canUpgrade = upgradeItem.CanBeUpgraded();
            bool fullyUpgraded = upgradeItem.IsFullyUpgraded();

            uint cost = upgradeItem.GetNextCost();

            costText.text = fullyUpgraded ? "" : cost.ToString();

            if (canUpgrade)
            {
                costText.color = settings.costTxtEnabledColor;
                levelText.color = settings.levelTxtEnabledColor;
                button.interactable = true;
                disableOverlay.SetActive(false);
            }
            else
            {
                costText.color = settings.costTxtDisabledColor;
                levelText.color = settings.levelTxtDisabledColor;
                button.interactable = false;
                disableOverlay.SetActive(true);
            }

            levelText.text = upgradeItem.IsFullyUpgraded() ? fullUpgradeTxt : upgradeItem.GetUpgradeLevel().ToString();
        }

        protected override void OnUpgrade()
        {
            if (upgradeItem.CanBeUpgraded())
            {
                upgradeManager.Upgrade(upgradeItem);
            }
        }
    }
}