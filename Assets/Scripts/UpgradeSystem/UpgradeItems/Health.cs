using UnityEngine;

namespace UpgradeSystem.UpgradeItems
{
    [CreateAssetMenu(menuName = "Upgrade/Health")]
    public class Health : UpgradeItem
    {
        public string keyName = "pivot_hp";


        [System.Serializable]
        public class UpgradeStep
        {
            static public float EvaluateHealth(float value, float maxValue)
            {
                float f = (float)System.Math.Exp(value / 14f);
                float g = (100f * value) / 60f;
                return Mathf.Lerp(f, g, value / maxValue) + 1;
            }
            static public uint EvaluateCost(float value, float maxValue)
            {
                float p = 10f * value * value * value * value;
                float q = 100 * value;
                float val = Mathf.Lerp(q, p, value / maxValue);
                return (uint)val;
            }
        }
        ushort upgradeLevel = 0;
        readonly ushort maxUpgradeLevel = 60;

        public override void Activate()
        {
            // apply health
            playerInfo.SetMaxHealth(UpgradeStep.EvaluateHealth(upgradeLevel, maxUpgradeLevel));
            playerInfo.OnGameplayEnd(); // just in case
        }

        public override bool CanBeUpgraded() => !IsFullyUpgraded() && playerInfo.CanAfford(GetNextCost());

        public override uint GetNextCost() => !IsFullyUpgraded() ? UpgradeStep.EvaluateCost(upgradeLevel + 1, maxUpgradeLevel) : uint.MaxValue;

        public override bool IsFullyUpgraded()
        {
            Debug.Log($"isfully upgraded ? {upgradeLevel} : {maxUpgradeLevel}");
            return upgradeLevel >= maxUpgradeLevel;
        }

        public override void LoadState()
        {
            var value = PlayerPrefs.GetString(keyName, "0");
            upgradeLevel = ushort.Parse(value);
            Debug.Log($"upgrade state {name} was loaded ( lvl {upgradeLevel} )");
        }

        public override void SaveState()
        {
            PlayerPrefs.SetString(keyName, upgradeLevel.ToString());
        }

        internal override void Upgrade()
        {
            if (CanBeUpgraded())
            {
                upgradeLevel++;
                SaveState();
                Activate();
                Debug.Log($"Upgraded {name} to level {upgradeLevel}");
            }
            else
            {
                Debug.LogWarning($"Upgrade Item {name} could not be upgraded!");
            }
        }

        public uint GetUpgradeLevel() => upgradeLevel;
    }
}