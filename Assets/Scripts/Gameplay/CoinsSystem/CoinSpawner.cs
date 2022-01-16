using UnityEngine;

namespace Gameplay.CoinsSystem
{
    [CreateAssetMenu(menuName = "CoinSpawner")]
    public class CoinSpawner : ScriptableObject
    {
        [System.Serializable]
        public class SpawnCase
        {
            public Coin coinPrefab;
            public uint minimunAccepting = 1;
        }

        public SpawnCase[] spawnCases;


        public (Coin coin, int amoun)[] GetCoins(uint coinsAmount)
        {
            System.Collections.Generic.List<(Coin coin, int amount)> r = new System.Collections.Generic.List<(Coin coin, int amount)>();


            for (int i = spawnCases.Length - 1; i >= 0; i--)
            {
                int amount = (int)(coinsAmount / spawnCases[i].minimunAccepting);
                if (amount != 0)
                {
                    r.Add((spawnCases[i].coinPrefab, amount));
                    coinsAmount -= (uint)amount * (uint)spawnCases[i].minimunAccepting;
                }
            }

            return r.ToArray();
        }

    }
}