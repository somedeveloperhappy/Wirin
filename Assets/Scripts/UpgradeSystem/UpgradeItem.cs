using UnityEngine;

namespace UpgradeSystem
{
    public abstract class UpgradeItem : ScriptableObject
    {
        protected Gameplay.Player.PlayerInfo playerInfo;
        public abstract void LoadState();
        public abstract void SaveState();
        public abstract void Activate();
        internal abstract void Upgrade();
        public abstract uint GetNextCost();
        public abstract bool CanBeUpgraded();
        public abstract bool IsFullyUpgraded();
        public void Init(Gameplay.Player.PlayerInfo playerInfo) => this.playerInfo = playerInfo;
    }
}
