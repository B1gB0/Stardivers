using Build.Game.Scripts.ECS.Data;
using Build.Game.Scripts.ECS.Data.SO;
using Build.Game.Scripts.ECS.System;
using Build.Game.Scripts.Game.Gameplay.GameplayRoot;
using Cinemachine;
using Leopotam.Ecs;
using Project.Game.Scripts;
using Project.Scripts.ECS.Data;
using Project.Scripts.Score;
using Project.Scripts.UI;
using R3;
using Source.Game.Scripts;
using UnityEngine;

namespace Build.Game.Scripts.Game.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private const string CurrentOperationCodeName = nameof(CurrentOperationCodeName);
        
        private readonly WeaponHolder _weaponHolder = new ();

        [SerializeField] private DataFactory _dataFactory;
        [SerializeField] private WeaponFactory _weaponFactory;
        [SerializeField] private ViewFactory _viewFactory;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
        [SerializeField] private AudioSoundsService _audioSoundsService;

        private UIGameplayRootBinder _uiScene;
        
        private PlayerInitData _playerData;
        private MapInitData _mapData;
        private EnemyInitData _enemyData;
        private StoneInitData _stoneData;
        private CapsuleInitData _capsuleData;
        private PlayerProgressionInitData _playerProgressionData;

        private EcsWorld _world;
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;
        
        private GameInitSystem _gameInitSystem;

        private HealthBar _healthBar;
        private ExperiencePoints _experiencePoints;
        private ProgressRadialBar _progressBar;
        private LevelUpPanel _levelUpPanel;

        private void OnDisable()
        {
            if (_gameInitSystem != null)
            {
                _gameInitSystem.PlayerIsLanded -= _healthBar.Show;
                _gameInitSystem.PlayerIsLanded -= _progressBar.Show;
            }

            if (_experiencePoints != null)
            {
                _experiencePoints.RewardIsShowed -= _levelUpPanel.OnLevelUpgraded;
            }
            
            if(_weaponFactory != null && _uiScene != null)
                _weaponFactory.MinesIsCreated -= _uiScene.ShowMinesButton;
        }

        private void Update()
        {
            _updateSystems?.Run();
        }

        private void FixedUpdate()
        {
            _fixedUpdateSystems?.Run();
        }

        public Observable<GameplayExitParameters> Run(UIRootView uiRoot, GameplayEnterParameters enterParameters)
        {
            _playerData = _dataFactory.CreatePlayerData();
            _mapData = _dataFactory.CreateMapData(PlayerPrefs.GetString(CurrentOperationCodeName), 0);
            _enemyData = _dataFactory.CreateEnemyData();
            _stoneData = _dataFactory.CreateStoneData();
            _capsuleData = _dataFactory.CreateCapsuleData();
            _playerProgressionData = _dataFactory.CreatePlayerProgression();

            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world);

            FloatingDamageTextView damageTextView = _viewFactory.CreateDamageTextView();
            damageTextView.Hide();
            FloatingDamageTextService damageTextService = new FloatingDamageTextService(damageTextView);
            
            _audioSoundsService = Instantiate(_audioSoundsService);
            
            _experiencePoints = new ExperiencePoints(_playerProgressionData);

            _updateSystems.Inject(_experiencePoints);
            _updateSystems.Inject(damageTextService);
            _updateSystems.Inject(_audioSoundsService);
            _updateSystems.Add(_gameInitSystem = new GameInitSystem(_playerData, _enemyData, _stoneData, _capsuleData, _mapData));
            _updateSystems.Add(new PlayerInputSystem());
            _updateSystems.Add(new MainCameraSystem(_cinemachineVirtualCamera));
            _updateSystems.Add(new PlayerAnimatedSystem());
            _updateSystems.Add(new EnemyAnimatedSystem());
            _updateSystems.Add(new EnemyAttackSystem());
            _updateSystems.Add(new ResourcesAnimatedSystem());
            _updateSystems.Init();
            
            _fixedUpdateSystems.Add(new PlayerMoveSystem());
            _fixedUpdateSystems.Add(new FollowSystem());
            _fixedUpdateSystems.Init();
            
            _levelUpPanel = _viewFactory.CreateLevelUpPanel();
            _healthBar = _viewFactory.CreateHealthBar(_gameInitSystem.PlayerHealth);
            _progressBar = _viewFactory.CreateProgressBar(_experiencePoints, _gameInitSystem.PlayerTransform);
            
            _weaponFactory.GetData(_gameInitSystem.PlayerTransform, _weaponHolder, _audioSoundsService);
            _weaponFactory.CreateEnemyDetector();
            _weaponFactory.CreateWeapon(Weapons.Gun);
            
            _levelUpPanel.GetServices(uiRoot.PauseService, _weaponFactory, _weaponHolder, _audioSoundsService);
            _levelUpPanel.GetStartImprovements();

            _uiScene = Instantiate(_sceneUIRootPrefab);
            _healthBar.transform.SetParent(_uiScene.transform);
            _levelUpPanel.transform.SetParent(_uiScene.transform);
            _weaponFactory.GetMinesButton(_uiScene.MinesButton);
            uiRoot.AttachSceneUI(_uiScene.gameObject);
            
            _uiScene.GetUIStateMachine(uiRoot.UIStateMachine, uiRoot.UIRootButtons);

            _weaponFactory.MinesIsCreated += _uiScene.ShowMinesButton;

            _experiencePoints.RewardIsShowed += _levelUpPanel.OnLevelUpgraded;
            
            _gameInitSystem.PlayerIsLanded += _healthBar.Show;
            _gameInitSystem.PlayerIsLanded += _progressBar.Show;

            var exitSceneSignalSubject = new Subject<Unit>();
            _uiScene.Bind(exitSceneSignalSubject);

            var mainMenuEnterParameters = new MainMenuEnterParameters("Enter parameters");
            var exitParameters = new GameplayExitParameters(mainMenuEnterParameters);
            var exitToMainMenuSceneSignal = exitSceneSignalSubject.Select(_ => exitParameters);

            return exitToMainMenuSceneSignal;
        }

        private void OnDestroy()
        {
            _updateSystems?.Destroy();
            _world?.Destroy();
        }
    }
}