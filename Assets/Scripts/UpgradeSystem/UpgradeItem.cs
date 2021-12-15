using UnityEngine;

namespace UpgradeSystem
{
    public abstract class UpgradeItem : ScriptableObject
    {
        public abstract void LoadState();
        public abstract void SaveState();

        public abstract bool ShouldBeActive();

        public abstract void Activate();

        public abstract void Upgrade();

        public abstract uint GetNextCost();

        public abstract bool CanBeUpgraded();

        public abstract GameObject GetButtonPrefab();

        [HideInInspector]
        public Gameplay.Player.PlayerInfo playerInfo;

        public void Init(Gameplay.Player.PlayerInfo playerInfo) => this.playerInfo = playerInfo;
    }
}
