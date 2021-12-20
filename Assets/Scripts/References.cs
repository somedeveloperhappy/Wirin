using UnityEngine;


public class References : MonoBehaviour
{
    private static References instance;
    [SerializeField] private Camera _currentCamera;
    [SerializeField] private Gameplay.EnemyNamespace.EnemySpawnInfo[] _enemySpawnInfos;
    [SerializeField] private Management.GameController _gameController;
    [SerializeField] private Management.LevelManager _levelManager;
    [SerializeField] private PlayManager.PlayerPressManager _playerPressManager;
    [SerializeField] private PostPro _postPro;
    [SerializeField] private Gameplay.Player.PlayerInfo _playerInfo;

    public static Gameplay.Player.PlayerInfo PlayerInfo => instance?._playerInfo;
    public static PostPro postPro => instance?._postPro;

    public static PlayManager.PlayerPressManager playerPressManager => instance?._playerPressManager;

    public static Camera currentCamera => instance?._currentCamera;

    public static Gameplay.EnemyNamespace.EnemySpawnInfo[] enemySpawnInfos => instance?._enemySpawnInfos;

    public static Management.LevelManager levelManager => instance?._levelManager;

    public static Management.GameController gameController => instance?._gameController;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy( this );
    }
}
