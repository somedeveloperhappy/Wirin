using UnityEngine;

namespace FlatTheme.UpgradeItems
{
    public class CoinsText : MonoBehaviour, CanvasSystem.IOnCanvasEnabled, CanvasSystem.IOnCanvasDisabled
    {
        public Gameplay.Player.PlayerInfo playerInfo;
        public UnityEngine.UI.Text text;
        public UpgradeSystem.UpgradeManager upgradeManager;

        [ContextMenu("Auto Resolve")]
        public void AutoResolve()
        {
            playerInfo = FindObjectOfType<Gameplay.Player.PlayerInfo>();
            text = GetComponent<UnityEngine.UI.Text>();
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