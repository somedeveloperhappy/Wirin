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
        [SerializeField] private AudioSystem.BackgroundMusic _backgroundMusic;
        [SerializeField] private AudioSystem.StaticSounds _menu_sfx;
        [SerializeField] private AudioSystem.StaticSounds _ingame_sfx;
        [SerializeField] private UnityEngine.Audio.AudioMixerGroup _ingameMixerGroup;
        [SerializeField] private UnityEngine.Audio.AudioMixer _audioMixer;
        [SerializeField] private GameObject _gameplayObjects;
        [SerializeField] private HighScore.HighScoreManager _highScoreManager;


        public static Gameplay.Player.PlayerInfo playerInfo => instance?._playerInfo;
        public static PostPro postPro => instance?._postPro;
        public static PlayManager.PlayerPressManager playerPressManager => instance?._playerPressManager;
        public static Camera currentCamera => instance?._currentCamera;
        public static Gameplay.EnemyNamespace.EnemySpawnInfo[] enemySpawnInfos => instance?._enemySpawnInfos;
        public static Management.LevelManager levelManager => instance?._levelManager;
        public static Management.GameController gameController => instance?._gameController;
        public static AudioSystem.BackgroundMusic backgroundMusic => instance?._backgroundMusic;
        public static AudioSystem.StaticSounds menu_sfx => instance?._menu_sfx;
        public static AudioSystem.StaticSounds ingame_sfx => instance?._ingame_sfx;
        public static UnityEngine.Audio.AudioMixerGroup ingameMixerGroup => instance?._ingameMixerGroup;
        public static UnityEngine.Audio.AudioMixer audioMixer => instance?._audioMixer;
        public static GameObject gameplayObjects => instance?._gameplayObjects;
        public static HighScore.HighScoreManager highScoreManager => instance?._highScoreManager;


        static UnityEngine.EventSystems.EventSystem _eventSystem;
        public static UnityEngine.EventSystems.EventSystem eventSystem
        {
                get
                {
                        return (_eventSystem ??= UnityEngine.EventSystems.EventSystem.current);
                }
        }
        private void Awake()
        {
                if (instance)
                        Destroy(instance);
                instance = this;
        }
}
