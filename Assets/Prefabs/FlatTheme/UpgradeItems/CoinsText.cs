using UnityEngine;

namespace FlatTheme.UpgradeItems
{
    public class CoinsText : MonoBehaviour, CanvasSystem.IOnCanvasEnabled, CanvasSystem.IOnCanvasDisabled
    {
        public Gameplay.Player.PlayerInfo playerInfo;
        public TMPro.TMP_Text text;
        public UpgradeSystem.UpgradeManager upgradeManager;

        [ContextMenu("Auto Resolve")]
        public void AutoResolve()
        {
            playerInfo = FindObjectOfType<Gameplay.Player.PlayerInfo>();
            text = GetComponent<TMPro.TMP_Text>();
            upgradeManager = FindObjectOfType<UpgradeSystem.UpgradeManager>();
        }

        public void OnCanvasDisable()
        {
            upgradeManager.onUpgrade -= UpdateText;
        }

        public void OnCanvasEnable()
        {
            upgradeManager.onUpgrade += UpdateText;
            UpdateText();
        }

        private void UpdateText()
        {
            text.text = playerInfo.moneyManager.Coins.ToString();
        }
    }
}