using Cinemachine;
using Leopotam.Ecs;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.System;
using Project.Scripts.Experience;
using Project.Scripts.Game.Gameplay.Root.View;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.Game.MainMenu.Root;
using Project.Scripts.Services;
using Project.Scripts.UI;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.Player;
using R3;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;

namespace Project.Scripts.Game.Gameplay.Root
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private readonly WeaponHolder _weaponHolder = new ();

        [SerializeField] private DataFactory _dataFactory;
        [SerializeField] private WeaponFactory _weaponFactory;
        [SerializeField] private ViewFactory _viewFactory;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
        
        private UIGameplayRootBinder _uiScene;
        private GameplayExitParameters _exitParameters;

        private PlayerInitData _playerData;
        private LevelInitData _levelData;
        private SmallAlienEnemyInitData smallAlienEnemyData;
        private BigAlienEnemyInitData bigAlienEnemyData;
        private GunnerAlienEnemyInitData _gunnerEnemyAlienData;
        private StoneInitData _stoneData;
        private CapsuleInitData _capsuleData;
        private PlayerProgressionInitData _playerProgressionData;
        private HealingCoreInitData _healingCoreData;
        private GoldCoreInitData _goldCoreData;

        private EcsWorld _world;
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;
        private GameInitSystem _gameInitSystem;
        
        private AudioSoundsService _audioSoundsService;
        private IPauseService _pauseService;
        private OperationService _operationService;

        private HealthBar _healthBar;
        private ExperiencePoints _experiencePoints;
        private ProgressRadialBar _progressBar;
        private LevelUpPanel _levelUpPanel;
        private EndGamePanel _endGamePanel;
        private Timer _timer;
        private AdviserMessagePanel _adviserMessagePanel;
        private GoldView _goldView;
        private BallisticRocketProgressBar _ballisticRocketProgressBar;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, IPauseService pauseService,
            OperationService operationService)
        {
            _audioSoundsService = audioSoundsService;
            _pauseService = pauseService;
            _operationService = operationService;
        }

        private void OnDisable()
        {
            if (_gameInitSystem != null)
            {
                _gameInitSystem.PlayerIsSpawned -= _healthBar.Show;
                _gameInitSystem.PlayerIsSpawned -= _progressBar.Show;
                
                _gameInitSystem.PlayerHealth.Die -= _endGamePanel.Show;
                _gameInitSystem.PlayerHealth.Die -= _endGamePanel.SetDefeatPanel;
                _gameInitSystem.PlayerHealth.Die -= _progressBar.Hide;
                
                _gameInitSystem.Level.EndLevelTrigger.IsLevelCompleted -= _endGamePanel.Show;
                _gameInitSystem.Level.EndLevelTrigger.IsLevelCompleted -= _endGamePanel.SetVictoryPanel;
            }

            if (_endGamePanel != null)
            {
                _endGamePanel.GoToMainMenuButton.onClick.RemoveListener(GetMainMenuExitParameters);
                _endGamePanel.GoToMainMenuButton.onClick.RemoveListener(_uiScene.HandleGoToNextSceneButtonClick);
                _endGamePanel.NextLevelButton.onClick.RemoveListener(GetGameplayExitParameters);
                _endGamePanel.NextLevelButton.onClick.RemoveListener(_uiScene.HandleGoToNextSceneButtonClick);
                _endGamePanel.RebornPlayerButton.onClick.RemoveListener(_gameInitSystem.CreateCapsule);
            }

            if (_experiencePoints != null)
            {
                _experiencePoints.CurrentLevelIsUpgraded -= _levelUpPanel.OnCurrentLevelIsUpgraded;
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
            _pauseService.PlayGame();
            
            _operationService.SetCurrentNumberLevel(enterParameters.CurrentNumberLevel);

            InitData();

            FloatingTextView textView = _viewFactory.CreateDamageTextView();
            textView.Hide();
            FloatingTextService textService = new FloatingTextService(textView);

            _goldView = _viewFactory.CreateGoldView();
            _adviserMessagePanel = _viewFactory.CreateAdviserMessagePanel();
            _timer = _viewFactory.CreateTimer();
            _ballisticRocketProgressBar = _viewFactory.CreateBallisticRocketProgressBar();
            
            _experiencePoints = new ExperiencePoints(_playerProgressionData);

            InitEcs(textService);

            _levelUpPanel = _viewFactory.CreateLevelUpPanel();
            _endGamePanel = _viewFactory.CreateEndGamePanel();
            _healthBar = _viewFactory.CreateHealthBar(_gameInitSystem.PlayerHealth);
            _progressBar = _viewFactory.CreateProgressBar(_experiencePoints, _gameInitSystem.PlayerTransform);

            _weaponFactory.GetData(_gameInitSystem.PlayerTransform, _weaponHolder);
            _weaponFactory.CreateEnemyDetector();
            _weaponFactory.CreateWeapon(WeaponType.Gun);
            
            _levelUpPanel.GetServices(_weaponFactory, _weaponHolder);
            _levelUpPanel.GetStartImprovements();

            _uiScene = Instantiate(_sceneUIRootPrefab);
            _healthBar.transform.SetParent(_uiScene.transform);
            _ballisticRocketProgressBar.transform.SetParent(_uiScene.transform);
            _levelUpPanel.transform.SetParent(_uiScene.transform);
            _endGamePanel.transform.SetParent(_uiScene.transform);
            _adviserMessagePanel.transform.SetParent(_uiScene.transform);
            _timer.transform.SetParent(_uiScene.transform);
            _goldView.transform.SetParent(_uiScene.transform);
            _weaponFactory.GetMinesButton(_uiScene.MinesButton);
            uiRoot.AttachSceneUI(_uiScene.gameObject);
            
            var container = gameObject.scene.GetSceneContainer();
            GameObjectInjector.InjectRecursive(uiRoot.gameObject, container);
            
            _uiScene.GetUIStateMachine(uiRoot.UIStateMachine, uiRoot.UIRootButtons);
            
            _gameInitSystem.PlayerHealth.Die += _endGamePanel.Show;
            _gameInitSystem.PlayerHealth.Die += _endGamePanel.SetDefeatPanel;
            _gameInitSystem.PlayerHealth.Die += _progressBar.Hide;
            
            _gameInitSystem.Level.EndLevelTrigger.IsLevelCompleted += _endGamePanel.Show;
            _gameInitSystem.Level.EndLevelTrigger.IsLevelCompleted += _endGamePanel.SetVictoryPanel;
            
            _gameInitSystem.PlayerIsSpawned += _healthBar.Show;
            _gameInitSystem.PlayerIsSpawned += _progressBar.Show;
            
            _endGamePanel.RebornPlayerButton.onClick.AddListener(_gameInitSystem.CreateCapsule);
            
            _endGamePanel.GoToMainMenuButton.onClick.AddListener(GetMainMenuExitParameters);
            _endGamePanel.GoToMainMenuButton.onClick.AddListener(_uiScene.HandleGoToNextSceneButtonClick);
            
            _endGamePanel.NextLevelButton.onClick.AddListener(GetGameplayExitParameters);
            _endGamePanel.NextLevelButton.onClick.AddListener(_uiScene.HandleGoToNextSceneButtonClick);
            
            _weaponFactory.MinesIsCreated += _uiScene.ShowMinesButton;
            _experiencePoints.CurrentLevelIsUpgraded += _levelUpPanel.OnCurrentLevelIsUpgraded;

            var exitSceneSignalSubject = new Subject<Unit>();
            _uiScene.Bind(exitSceneSignalSubject);

            var exitToSceneSignal = exitSceneSignalSubject.Select(_ => _exitParameters);

            return exitToSceneSignal;
        }
        
        private void OnDestroy()
        {
            _updateSystems?.Destroy();
            _fixedUpdateSystems?.Destroy();
            _world?.Destroy();
        }

        private void GetMainMenuExitParameters()
        {
            var mainMenuEnterParameters = new MainMenuEnterParameters("Enter parameters");
            _exitParameters = new GameplayExitParameters(mainMenuEnterParameters);
        }
        
        private void GetGameplayExitParameters()
        {
            int nextNumberLevel = _operationService.CurrentNumberLevel + 1;

            var gameplayEnterParameters = new GameplayEnterParameters("",
                _operationService.CurrentOperation, nextNumberLevel);
            _exitParameters = new GameplayExitParameters(gameplayEnterParameters);
        }

        private void InitData()
        {
            _playerData = _dataFactory.CreatePlayerData();
            _levelData = _dataFactory.CreateLevelData(_operationService.CurrentOperation,
                _operationService.CurrentNumberLevel);
            smallAlienEnemyData = _dataFactory.CreateSmallEnemyAlienData();
            bigAlienEnemyData = _dataFactory.CreateBigEnemyAlienData();
            _gunnerEnemyAlienData = _dataFactory.CreateGunnerAlienEnemyData();
            _stoneData = _dataFactory.CreateStoneData();
            _capsuleData = _dataFactory.CreateCapsuleData();
            _playerProgressionData = _dataFactory.CreatePlayerProgression();
            _healingCoreData = _dataFactory.CreateHealingCoreData();
            _goldCoreData = _dataFactory.CreateGoldCoreData();
        }

        private void InitEcs(FloatingTextService textService)
        {
            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world);

            _updateSystems.Inject(_adviserMessagePanel);
            _updateSystems.Inject(_experiencePoints);
            _updateSystems.Inject(textService);
            _updateSystems.Inject(_goldView);
            _updateSystems.Inject(_audioSoundsService);
            _updateSystems.Inject(_timer);
            _updateSystems.Inject(_ballisticRocketProgressBar);

            _updateSystems.Add(_gameInitSystem = new GameInitSystem(_playerData, smallAlienEnemyData, bigAlienEnemyData, 
                _gunnerEnemyAlienData, _stoneData, _capsuleData, _levelData, _healingCoreData, _goldCoreData));
            _updateSystems.Add(new PlayerInputSystem());
            _updateSystems.Add(new MainCameraSystem(_cinemachineVirtualCamera));
            _updateSystems.Add(new PlayerAnimatedSystem());
            _updateSystems.Add(new EnemyAnimatedSystem());
            _updateSystems.Add(new EnemyMeleeAttackSystem());
            _updateSystems.Add(new ResourcesAnimatedSystem());
            _updateSystems.Init();

            _fixedUpdateSystems.Inject(bigAlienEnemyData.ProjectilePrefab);
            _fixedUpdateSystems.Add(new PlayerMoveSystem());
            _fixedUpdateSystems.Add(new FollowSystem());
            _fixedUpdateSystems.Add(new EnemyRangeAttackSystem());
            _fixedUpdateSystems.Init();
        }
    }
}