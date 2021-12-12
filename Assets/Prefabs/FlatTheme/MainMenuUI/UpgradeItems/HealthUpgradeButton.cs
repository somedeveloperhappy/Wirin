using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleScripts;

namespace FlatTheme.MainMenuUI.UpgradeItems
{
	public class HealthUpgradeButton : MonoBehaviour, CanvasSystem.IOnCanvasEnabled
	{

		public string itemName;
		// public UpgradeSystem.UpgradeItems.TrinonPower trinonPowerItem;
		public Button button;
		public Text costText, levelText;
		public Image disableOverlay;

		public void OnCanvasEnable()
		{
			UpdateValues ();
		}

		public void UpdateValues()
		{
			var upgradeManager = References.pivot.playerInfo.upgradeManager;
			UpgradeSystem.UpgradeItems.TrinonPower upgradeItem =
				(UpgradeSystem.UpgradeItems.TrinonPower) upgradeManager.GetUpgradeItemByName (itemName);
				
			Debug.Log($"{upgradeItem.name}");

			if (upgradeItem.CanBeUpgraded ())
			{
				costText.text = upgradeItem.GetNextCost ().ToString ();
				levelText.text = upgradeItem.GetUpgradeLevel ().ToString ();
				
				if(upgradeManager.CanUpgrade(upgradeItem))
				{
					button.interactable = true;
					disableOverlay.enabled = false;
				}
				else
				{
					button.interactable = false;
					disableOverlay.enabled = true;
				}
			}
			else
			{
				button.interactable = false;
				costText.text = "FULL";
				levelText.text = "FULL";
				disableOverlay.enabled = true;
			}

		}

	}
}
