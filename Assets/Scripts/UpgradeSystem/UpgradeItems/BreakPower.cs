using UnityEngine;

namespace UpgradeSystem.UpgradeItems
{
    [CreateAssetMenu(menuName = "Upgrade/Break Power")]
    public class BreakPower : UpgradeItem
    {

        public string key = "breakpow";
        struct UpgradeStep
        {
            static public float EvaluateBreakPower(int value) => (float)System.Math.Exp((value) / 20d);
            static public uint EvaluateCost(float value, float maxValue)
            {
                float p = 10f * value * value * value * value;
                float q = 100 * value;
                float val = Mathf.Lerp(q, p, value / maxValue);
                return (uint)val;

            }
        }

        // the index below this will be activated
        ushort upgradeLevel = 0;
        // the maximum level
        ushort maxUpgradeLevel = 60;


        public override void Activate()
        {
            playerInfo.SetBreakLerpSpeed(UpgradeStep.EvaluateBreakPower(upgradeLevel));
        }

        public override bool CanBeUpgraded() => !IsFullyUpgraded() && playerInfo.CanAfford(GetNextCost());

        public override uint GetNextCost() => !IsFullyUpgraded() ? UpgradeStep.EvaluateCost(upgradeLevel + 1, maxUpgradeLevel) : uint.MaxValue;

        public override bool IsFullyUpgraded() => upgradeLevel >= maxUpgradeLevel;

        public override void LoadState()
        {
            string prefVal = PlayerPrefs.GetString(key, "0");
            upgradeLevel = ushort.Parse(prefVal);
            Debug.Log("Break Power upgrade item loaded");
        }

        public override void SaveState()
        {
            string prefVal = upgradeLevel.ToString();
            PlayerPrefs.SetString(key, prefVal);
        }

        internal override void Upgrade()
        {
            if (CanBeUpgraded())
            {
                upgradeLevel++;
                SaveState();
                Activate();
                Debug.Log($"Upgraded {name} to {upgradeLevel}");
            }
            else
            {
                Debug.Log($"could not upgrade {name}");
            }
        }
        public ushort GetUpgradeLevel() => upgradeLevel;
    }
}