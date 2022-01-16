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
            Debug.Log($"Loading money");

            _coins = uint.Parse(PlayerPrefs.GetString("coins", "0"));
        }

        public void Save()
        {
            Debug.Log($"Saving Coins");
            PlayerPrefs.SetString("coins", _coins.ToString());
        }
    }
}
