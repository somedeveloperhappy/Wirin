using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradeSystem
{
    [CreateAssetMenu()]
	public class MoneyManager : ScriptableObject
	{
		[SerializeField] uint _coins;

		public uint Coins
		{
			get => _coins;
			set
			{
				if (value < 0) value = 0;
				_coins = value;
			}
		}


		public void Load()
		{
			if (uint.TryParse (PlayerPrefs.GetString ("coins", "0"), out uint result))
			{
				_coins = result;
			}
			else
			{
				_coins = 0;
			}
		}

		public void Save()
		{
			PlayerPrefs.SetString ("coins", _coins.ToString ());
		}
	}
}
