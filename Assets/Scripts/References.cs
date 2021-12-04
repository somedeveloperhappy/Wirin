using Gameplay;
using Gameplay.EnemyNamespace.Types;
using Gameplay.Player;
using LevelManaging;
using PlayManagement;
using UnityEngine;

public class References : MonoBehaviour
{
	private static References instance;
	[SerializeField] private Camera _currentCamera;
	[SerializeField] private EnemyBase[] _enemies;
	[SerializeField] private GameController _gameController;
	[SerializeField] private LevelManager _levelManager;
	[SerializeField] private Pivot _pivot;
	[SerializeField] private PlayerPressManager _playerPressManager;
	[SerializeField] private PostPro _postPro;
	[SerializeField] private Trinon _trinon;

	public static Pivot pivot => instance?._pivot;

	public static Trinon trinon => instance?._trinon;

	public static PostPro postPro => instance?._postPro;

	public static PlayerPressManager playerPressManager => instance?._playerPressManager;

	public static Camera currentCamera => instance?._currentCamera;

	public static EnemyBase[] enemies => instance?._enemies;

	public static LevelManager levelManager => instance?._levelManager;

	public static GameController gameController => instance?._gameController;

	private void Awake()
	{
		if (!instance)
			instance = this;
		else
			Destroy(this);
	}
}