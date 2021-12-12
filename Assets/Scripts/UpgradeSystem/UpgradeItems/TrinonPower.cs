using Gameplay.Player;
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
			var val = PlayerPrefs.GetString (keyName, string.Empty);
			if (val == string.Empty) return;
			var vals = val.Split ('_');
			// assigning values
			upgradeLevel = uint.Parse (vals[0]);
		}

		public override void SaveState()
		{
			// "name1_name2_name3_..."
			string val = $"{upgradeLevel}_";
			PlayerPrefs.SetString (keyName, val);
		}

		public override void Activate()
		{
			if (upgradeLevel == 0) return;

			// applying
			var pow = upgradeSteps[upgradeLevel - 1].power;
			playerInfo.parts.trinon.playerNormalBulletPrefab.damageMultiplier = pow;
		}

		public override bool CanBeUpgraded() => upgradeLevel < upgradeSteps.Length;


		public override GameObject GetButtonPrefab() => buttonPrefab;

		public override uint GetNextCost() => upgradeSteps[upgradeLevel].cost;

		public override bool ShouldBeActive() => upgradeLevel > 0;

		public override void Upgrade()
		{
			upgradeLevel ++;
		}
		
		public uint GetUpgradeLevel() => upgradeLevel;

	}
}
