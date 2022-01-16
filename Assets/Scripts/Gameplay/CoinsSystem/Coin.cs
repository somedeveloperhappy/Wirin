using UnityEngine;


namespace Gameplay.CoinsSystem
{
    public class Coin : MonoBehaviour
    {
        [HideInInspector] public Gameplay.Player.PlayerInfo playerInfo;
        public uint coinsWorth;

        [System.Serializable]
        public class Settings
        {
            public float speed = 2;
        }
        public Settings settings;

        public System.Action onApply;

        public void Init(Player.PlayerInfo playerInfo, uint coinsWorth)
        {
            this.playerInfo = playerInfo;
            this.coinsWorth = coinsWorth;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, playerInfo.transform.position, settings.speed * Time.unscaledDeltaTime);
            if (Vector3.Distance(transform.position, playerInfo.transform.position) <= 0.5f)
            {
                ApplyCoin();
            }
        }

        public void ApplyCoin()
        {
            Debug.Log($"Adding {coinsWorth} coins...");

            playerInfo.moneyManager.Coins += coinsWorth;
            Destroy(gameObject);
            onApply?.Invoke();
        }

    }
}