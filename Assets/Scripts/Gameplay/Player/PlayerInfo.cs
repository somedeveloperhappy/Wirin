using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] float health;
    public float Health => health;
    
    public void TakeDamage(EnemyDamageInfo damageinfo)
    {
        
    }
}

public interface IPlayerPart {
    public PlayerInfo GetPlayerInfo();
}