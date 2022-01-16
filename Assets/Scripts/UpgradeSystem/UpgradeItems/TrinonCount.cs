using UnityEngine;

namespace UpgradeSystem.UpgradeItems
{
    [CreateAssetMenu(menuName = "Upgrade/Trinon Count")]
    public class TrinonCount : UpgradeItem
    {
        [SerializeField] private string key = "_bc";
        [System.Serializable]
        public struct UpgradeStep
        {
            public uint trinonCount;
            public uint cost;
        }
        public UpgradeStep[] upgradeSteps;
        /// <summary>
        /// the index below this will be activated
        /// </summary>
        uint upgradeLevel = 0;

        public override void Activate()
        {
            if (upgradeLevel != 0)
            {
                playerInfo.GetShootings().trinonMaxCount = upgradeSteps[upgradeLevel - 1].trinonCount;
            }
        }

        public override bool CanBeUpgraded() => !IsFullyUpgraded() && playerInfo.CanAfford(GetNextCost());
        public override uint GetNextCost() => IsFullyUpgraded() ? 0 : upgradeSteps[upgradeLevel].cost;
        public override bool IsFullyUpgraded() => upgradeLevel >= upgradeSteps.Length;
        public override void LoadState() => upgradeLevel = uint.Parse(PlayerPrefs.GetString(key, "0"));
        public override void SaveState() => PlayerPrefs.SetString(key, upgradeLevel.ToString());
        internal override void Upgrade()
        {
            if (CanBeUpgraded())
            {
                upgradeLevel++;
                Activate();
                SaveState();
                Debug.Log($"Trinon Count upgraded to level {upgradeLevel}");
            }
            else
            {
                Debug.Log($"Could not upgrade Trinon Count :(");
            }
        }
        public uint GetUpgradeLevel() => upgradeLevel;
        public uint GetTrinoncCount() => upgradeLevel > 0 ? upgradeSteps[upgradeLevel - 1].trinonCount : 1;
    }
}