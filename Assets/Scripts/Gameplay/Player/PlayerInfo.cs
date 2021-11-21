using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] float health;
    public float Health => health;

    public void TakeDamage(EnemyDamageInfo damageinfo) {
        Debug.Log($"taking damage to player...");
        health -= damageinfo.damage;
    }
}

public interface IPlayerPart
{
    public PlayerInfo GetPlayerInfo();
}