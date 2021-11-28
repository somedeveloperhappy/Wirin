[System.Serializable]
public struct PlayerBulletDamageInfo
{
    public float damage;
    public float stunDuration;

    public PlayerBulletDamageInfo(float damage, float stunDuration) {
        this.damage = damage;
        this.stunDuration = stunDuration;
    }
}

[System.Serializable]
public struct EnemyDamageInfo
{
    public float damage;

    public EnemyDamageInfo(float damage) {
        this.damage = damage;
    }
}