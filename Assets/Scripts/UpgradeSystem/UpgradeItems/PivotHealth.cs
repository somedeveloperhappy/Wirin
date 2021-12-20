using System.Collections;
using UnityEngine;

namespace UpgradeSystem.UpgradeItems
{
    [CreateAssetMenu( menuName = "Upgrade/Pivot Health" )]
    public class PivotHealth : UpgradeItem
    {
        public string keyName = "pivot_hp";

        /// <summary>
        /// the index below this will be activated
        /// </summary>
        uint upgradeLevel = 0;

        [System.Serializable]
        public class UpgradeStep
        {
            public float health;
            public uint cost;
        }
        public UpgradeStep[] upgradeSteps;

        public override void Activate()
        {
            if (upgradeLevel == 0) return;
            // apply health
            playerInfo.SetMaxHealth( upgradeSteps[upgradeLevel - 1].health );
            playerInfo.OnGameplayEnd(); // just in case
        }

        public override bool CanBeUpgraded() => !IsFullyUpgraded() && playerInfo.CanAfford( GetNextCost() );

        public override uint GetNextCost() => IsFullyUpgraded() ? 0 : upgradeSteps[upgradeLevel].cost;

        public override bool IsFullyUpgraded()
        {
            return upgradeLevel >= upgradeSteps.Length;
        }

        public override void LoadState()
        {
            Debug.Log( $"loading the upgrade state {name}" );
            var value = PlayerPrefs.GetString( keyName, "0_" );
            var upgradeVal = value.Split( '_' )[0];
            upgradeLevel = uint.Parse( upgradeVal );
            Debug.Log( $"upgrade state {name} was loaded" );
        }

        public override void SaveState()
        {
            PlayerPrefs.SetString( keyName, upgradeLevel.ToString() + "_" );
        }

        internal override void Upgrade()
        {
            if (CanBeUpgraded())
            {
                upgradeLevel++;
                SaveState();
                Activate();
                Debug.Log( $"Upgraded {name} to level {upgradeLevel}" );
            }
            else
            {
                Debug.LogWarning( $"Upgrade Item {name} could not be upgraded!" );
            }
        }

        public uint GetUpgradeLevel() => upgradeLevel;
    }
}