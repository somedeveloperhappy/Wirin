using UnityEngine;

namespace UpgradeSystem.UpgradeItems
{
    [CreateAssetMenu(menuName = "Upgrade/Trinon High Damage")]
    public class TrinonHighDamage : UpgradeItem
    {
        public string keyName = "trinonHdmg";


        [System.Serializable]
        public struct UpgradeSteps
        {
            static public float EvaluatePower(float value, float maxValue)
            {
                value /= maxValue;
                return
                    (3 * value * value - 2 * value * value * value) * 10000 + 50;
            }
            static public uint EvaluateCost(float value, float maxValue)
            {
                float p = 10f * value * value * value * value;
                float q = 100 * value;
                float val = Mathf.Lerp(q, p, value / maxValue);
                return (uint)val;
            }
        }
        uint upgradeLevel = 0;
        uint maxUpgradeLevel = 60;

        public override void LoadState()
        {
            var val = PlayerPrefs.GetString(keyName, "0");
            upgradeLevel = uint.Parse(val);
            Debug.Log($"loaded val : {val} , lvl {upgradeLevel}");
        }

        public override void SaveState()
        {
            PlayerPrefs.SetString(keyName, upgradeLevel.ToString());
        }

        public override void Activate()
        {
            playerInfo.GetShootings().playerNormalBulletPrefab.damageMax = UpgradeSteps.EvaluatePower(upgradeLevel, maxUpgradeLevel);
        }

        public override bool CanBeUpgraded() => !IsFullyUpgraded() && playerInfo.CanAfford(GetNextCost());
        public override uint GetNextCost() => IsFullyUpgraded() ? 0 : UpgradeSteps.EvaluateCost(upgradeLevel + 1, maxUpgradeLevel);
        internal override void Upgrade()
        {
            if (CanBeUpgraded())
            {
                upgradeLevel++;
                SaveState();
                Activate();
                Debug.Log($"{name} upgraded to level {upgradeLevel}, saved and activated successfuly");
            }
            else
            {
                Debug.LogWarning($"{name} can not be upgaded");
            }
        }

        public uint GetUpgradeLevel() => upgradeLevel;
        public override bool IsFullyUpgraded() => upgradeLevel >= maxUpgradeLevel;

    }
}
