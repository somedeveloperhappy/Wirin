using UnityEngine;


public class References : MonoBehaviour
{
	private static References instance;
	[SerializeField] private Camera _currentCamera;
	[SerializeField] private Gameplay.EnemyNamespace.EnemySpawnInfo[] _enemySpawnInfos;
	[SerializeField] private Management.GameController _gameController;
	[SerializeField] private Management.LevelManager _levelManager;
	[SerializeField] private Gameplay.Player.Pivot _pivot;
	[SerializeField] private PlayManager.PlayerPressManager _playerPressManager;
	[SerializeField] private PostPro _postPro;
	[SerializeField] private Gameplay.Player.Trinon _trinon;

	public static Gameplay.Player.Pivot pivot => instance?._pivot;

	public static Gameplay.Player.Trinon trinon => instance?._trinon;

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
			Destroy (this);
	}
}
