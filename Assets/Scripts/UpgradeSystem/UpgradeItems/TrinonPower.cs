using UnityEngine;

namespace UpgradeSystem.UpgradeItems
{
    [CreateAssetMenu(menuName = "Wirin/Trinon Power")]
    public class TrinonPower : UpgradeItem
    {
        public ThemeSystem.ThemeBased<GameObject> buttonPrefab;
        public string keyName;

        /// <summary>
        /// the index below this would be activated	
        /// </summary>
        uint upgradeLevel = 0;

        [System.Serializable]
        public struct UpgradeSteps
        {
            public uint cost;
            public float power;
        }
        public UpgradeSteps[] upgradeSteps;

        public override void LoadState()
        {
            Debug.Log($"loading state of {name}...");
            var val = PlayerPrefs.GetString(keyName, "0");
            var vals = val.Split('_');
            // assigning values
            Debug.Log($"loaded val : {val}");
            upgradeLevel = uint.Parse(vals[0]);
        }

        public override void SaveState()
        {
            // "name1_name2_name3_..."
            string val = $"{upgradeLevel}_";
            PlayerPrefs.SetString(keyName, val);
        }

        public override void Activate()
        {
            if (upgradeLevel == 0) return;

            // applying
            var pow = upgradeSteps[upgradeLevel - 1].power;
            playerInfo.parts.trinon.playerNormalBulletPrefab.damageMultiplier = pow;
        }

        public override bool CanBeUpgraded() => upgradeLevel < upgradeSteps.Length;


        public override GameObject GetButtonPrefab()
        {
            Debug.Log($"theme index : {ThemeSystem.ThemeManager.instance.enabledIndex}");
            Debug.Log($" len : {buttonPrefab.values.Length}");
            return buttonPrefab;
        }

        public override uint GetNextCost() => upgradeSteps[upgradeLevel].cost;

        public override bool ShouldBeActive() => upgradeLevel > 0;

        public override void Upgrade()
        {
            if (CanBeUpgraded() && References.pivot.playerInfo.moneyManager.Coins >= GetNextCost())
            {
                upgradeLevel++;
                SaveState();
                Activate();
                Debug.Log($"{name} upgraded to level {upgradeLevel}, saved and activated successfuly");
            }
            Debug.LogWarning($"{name} can not be upgaded");
        }

        public uint GetUpgradeLevel() => upgradeLevel;

    }
}