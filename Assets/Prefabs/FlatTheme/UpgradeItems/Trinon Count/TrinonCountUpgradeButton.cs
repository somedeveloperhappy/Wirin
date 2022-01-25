using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FlatTheme.UpgradeItems.TrinonCount
{
    public class TrinonCountUpgradeButton : UpgradeSystem.UpgradeItemButton<UpgradeSystem.UpgradeItems.TrinonCount>
    {
        #region refs
        public Button button;
        public TMPro.TMP_Text costText;
        public string fullUpgradeTxt = "Full";
        public UpgradeSystem.UpgradeManager upgradeManager;
        public GameObject disableOverlay;
        #endregion

        [System.Serializable]
        public class Settings
        {
            public Color costTxtDisabledColor;
            public Color costTxtEnabledColor;
        }
        public Settings settings;

        [Space(10)]
        public Animator m_animator;
        public string inAnimName = "in_anim";

        [Space(10)]
        public Image bulletIcon;
        public RectTransform bulletIconPanel;
        [System.Serializable]
        public class BulletIconMatAnim
        {
            public float speed = 1;
            public string propName = "_CutOut";
        }
        public BulletIconMatAnim bulletIconMatAnim;


        [ContextMenu("Auto Resolve")]
        public void AutoResolve()
        {
            foreach (var item in GetComponentsInChildren<TMPro.TMP_Text>())
            {
                if (costText == null && item.name.ToLower().Contains("cost"))
                {
                    costText = item;
                }
            }
            foreach (Image img in GetComponentsInChildren<Image>())
            {
                if (disableOverlay == null && img.name.ToLower().Contains("disable"))
                {
                    disableOverlay = img.gameObject;
                }
                else if (bulletIcon == null && (img.name.ToLower().Contains("icon") || img.name.ToLower().Contains("bullet")))
                {
                    bulletIcon = img;
                }
            }
            foreach (Transform trans in transform)
            {
                if (bulletIconPanel == null && (trans.name.ToLower().Contains("pnl") || trans.name.ToLower().Contains("panel")))
                {
                    bulletIconPanel = trans as RectTransform;
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

            Debug.Log($"On enable");
            m_animator.enabled = true;
            m_animator.Play(inAnimName);

            StopAllCoroutines();
            StartCoroutine(AnimateBulletIcons());
        }
        private void OnDisable()
        {
            Debug.Log($"On disable");
            m_animator.StopPlayback();
            m_animator.enabled = false;

            StopAllCoroutines();
        }

        private IEnumerator AnimateBulletIcons()
        {
            var mat = bulletIcon.material;
            float t = 0;

            do
            {
                t += bulletIconMatAnim.speed * Time.unscaledDeltaTime;
                mat.SetFloat(bulletIconMatAnim.propName, t);
                yield return null;
            }
            while (t < 1);
        }


        public override void UpdateVisuals()
        {
            bool canUpgrade = upgradeItem.CanBeUpgraded();
            bool fullyUpgraded = upgradeItem.IsFullyUpgraded();

            costText.text = fullyUpgraded ? string.Empty : upgradeItem.GetNextCost().ToString();

            if (canUpgrade)
            {
                costText.color = settings.costTxtEnabledColor;
                button.interactable = true;
                disableOverlay.SetActive(false);
            }
            else
            {
                costText.color = settings.costTxtDisabledColor;
                button.interactable = false;
                disableOverlay.SetActive(true);
            }

            SetBullets(upgradeItem.GetTrinoncCount());
        }

        private void SetBullets(uint bulletCount)
        {
            // deleting all icons except the ref one
            foreach (var img in bulletIconPanel.GetComponentsInChildren<Image>())
                if (img != bulletIcon) Destroy(img);

            // instanciating copies of refs for bullet icons
            for (int i = 0; i < bulletCount - 1; i++)
            {
                var img = Instantiate(bulletIcon, bulletIconPanel);
            }
        }



        protected override void OnUpgrade()
        {
            if (upgradeItem.CanBeUpgraded())
            {
                Debug.Log($"Upgrade item {upgradeItem} could be upgraded. upgrading...");
                upgradeManager.Upgrade(upgradeItem);
            }
        }
    }
}