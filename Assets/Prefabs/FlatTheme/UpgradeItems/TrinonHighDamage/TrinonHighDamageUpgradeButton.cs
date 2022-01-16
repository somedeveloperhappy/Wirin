using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;

namespace FlatTheme.MainMenuUI.UpgradeItems
{
    public class TrinonHighDamageUpgradeButton : UpgradeItemButton<UpgradeSystem.UpgradeItems.TrinonHighDamage>
    {
        #region refs
        public Button button;
        public TMPro.TMP_Text costTxt, lvlText;
        public string fullUpgradeText = "Full";
        public UpgradeManager upgradeManager;
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
                if (costTxt == null && item.name.ToLower().Contains("cost"))
                {
                    costTxt = item;
                }
                else if (lvlText == null && item.name.ToLower().Contains("lvl") || item.name.ToLower().Contains("level"))
                {
                    lvlText = item;
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

            costTxt.text = fullyUpgraded ? "" : upgradeItem.GetNextCost().ToString();

            if (canUpgrade)
            {
                costTxt.color = settings.costTxtEnabledColor;
                lvlText.color = settings.levelTxtEnabledColor;
                button.interactable = true;
                disableOverlay.SetActive(false);
            }
            else
            {
                costTxt.color = settings.costTxtDisabledColor;
                lvlText.color = settings.levelTxtDisabledColor;
                button.interactable = false;
                disableOverlay.SetActive(true);
            }

            lvlText.text = upgradeItem.IsFullyUpgraded() ? fullUpgradeText : upgradeItem.GetUpgradeLevel().ToString();
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
