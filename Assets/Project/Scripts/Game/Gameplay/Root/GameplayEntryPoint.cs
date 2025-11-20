using Cinemachine;
using Cysharp.Threading.Tasks;
using Leopotam.Ecs;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.System;
using Project.Scripts.Experience;
using Project.Scripts.Game.Gameplay.Root.View;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.Game.MainMenu.Root;
using Project.Scripts.Levels;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.Player;
using R3;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Extensions;
using UnityEngine;
using YG;

namespace Project.Scripts.Game.Gameplay.Root
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private readonly WeaponHolder _weaponHolder = new();

        [SerializeField] private DataFactory _dataFactory;
        [SerializeField] private WeaponFactory _weaponFactory;
        [SerializeField] private ViewFactory _viewFactory;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
        [SerializeField] private Level _level;

        private UIGameplayRootBinder _uiScene;
        private UIRootView _uiRoot;
        private GameplayExitParameters _exitParameters;

        private LevelInitData _levelData;
        private PlayerInitData _playerInitData;
        private SmallAlienEnemyInitData _smallAlienEnemyData;
        private BigAlienEnemyInitData _bigAlienEnemyData;
        private GunnerAlienEnemyInitData _gunnerEnemyAlienData;
        private StoneInitData _stoneData;
        private CapsuleInitData _capsuleData;
        private HealingCoreInitData _healingCoreData;
        private GoldCoreInitData _goldCoreData;
        private AlienCocoonInitData _alienCocoonData;
        private AlienTurretEnemyInitData _alienTurretEnemyData;
        private IceCrystalInitData _iceCrystalData;

        private EcsWorld _world;
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;
        private GameInitSystem _gameInitSystem;

        private AudioSoundsService _audioSoundsService;
        private ParticleEffectsService _particleEffectsService;
        private OperationService _operationService;
        
        private IPauseService _pauseService;
        private IFloatingTextService _floatingTextService;
        private IResourceService _resourceService;
        private IDataBaseService _dataBaseService;
        private ICharacteristicsWeaponDataService _characteristicsWeaponDataService;
        private ICardService _cardService;
        private IEnemyService _enemyService;
        private IPlayerService _playerService;
        private ICurrencyService _currencyService;
        private ILevelTextService _levelTextService;
        private ICoreService _coreService;
        private ILevelUpService _levelUpService;
        private IUILocalizationService _uiLocalizationService;

        private HealthBar _healthBar;
        private ExperiencePoints _experiencePoints;
        private ProgressRadialBar _progressBar;
        private LevelUpPanel _levelUpPanel;
        private EndGamePanel _endGamePanel;
        private DialoguePanel _dialoguePanel;
        private GoldView _goldView;
        private AlienCocoonView _alienCocoonView;
        private MissionProgressBar _missionProgressBar;

        private Container _container;

// #if UNITY_EDITOR
        private CheatPanel _cheatPanel;
// #endif

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, IPauseService pauseService,
            OperationService operationService, IFloatingTextService floatingTextService,
            IDataBaseService dataBaseService,
            IResourceService resourceService, ICharacteristicsWeaponDataService characteristicsWeaponDataService,
            ICardService cardService, IEnemyService enemyService, IPlayerService playerService,
            ICurrencyService currencyService, ILevelTextService levelTextService, ICoreService coreService,
            ILevelUpService levelUpService, IUILocalizationService uiLocalizationService, 
            ParticleEffectsService particleEffectsService)
        {
            _audioSoundsService = audioSoundsService;
            _pauseService = pauseService;
            _operationService = operationService;
            _floatingTextService = floatingTextService;
            _dataBaseService = dataBaseService;
            _resourceService = resourceService;
            _characteristicsWeaponDataService = characteristicsWeaponDataService;
            _cardService = cardService;
            _enemyService = enemyService;
            _playerService = playerService;
            _currencyService = currencyService;
            _levelTextService = levelTextService;
            _coreService = coreService;
            _levelUpService = levelUpService;
            _uiLocalizationService = uiLocalizationService;
            _particleEffectsService = particleEffectsService;
        }

        private void Start()
        {
            _container = gameObject.scene.GetSceneContainer();
            
            switch (_operationService.CurrentOperation.Id)
            {
                case Constant.Operations.Mars:
                    _audioSoundsService.PlayMusic(SoundsType.MarsGameplayMusic);
                    break;
                case Constant.Operations.MysteryPlanet:
                    _audioSoundsService.PlayMusic(SoundsType.MysteryPlanetGameplayMusic);
                    break;
            }
        }

        private void Update()
        {
            _updateSystems?.Run();
        }

        private void FixedUpdate()
        {
            _fixedUpdateSystems?.Run();
        }

        public async UniTask<Observable<GameplayExitParameters>> Run(UIRootView uiRoot,
            GameplayEnterParameters enterParameters)
        {
            uiRoot.ExitButton.gameObject.SetActive(true);
            
            _uiRoot = uiRoot;
            _pauseService.PlayGame();

            _operationService.SetCurrentNumberLevel(enterParameters.CurrentNumberLevel);

            await InitData();
            
            _currencyService.SetMaxAlienCocoons(_levelData.AlienCocoonSpawnPoints.Count);

            await _characteristicsWeaponDataService.Init();
            await _cardService.Init();
            await _levelUpService.Init();
            await _enemyService.Init();
            await _playerService.Init();
            await _coreService.Init();
            await _uiLocalizationService.Init();
            await _particleEffectsService.Init();
            
            _uiScene = Instantiate(_sceneUIRootPrefab);
            
            _viewFactory.GetUIRootAndUIScene(uiRoot, _uiScene, _container);

            FloatingTextView textView = await _viewFactory.CreateDamageTextView();
            textView.Hide();
            _floatingTextService.Init(textView);

            _goldView = await _viewFactory.CreateGoldView();
            _dialoguePanel = await _viewFactory.CreateDialoguePanel();

            _levelUpPanel = await _viewFactory.CreateLevelUpPanel();
            _endGamePanel = await _viewFactory.CreateEndGamePanel();

            _experiencePoints = new ExperiencePoints(_playerService);
            
            _endGamePanel.GetExperiencePoints(_experiencePoints);

// #if UNITY_EDITOR
            _cheatPanel = await _viewFactory.CreateCheatPanel();
// #endif
            
            InitEcs();

            _healthBar = await _viewFactory.CreateHealthBar(_gameInitSystem.PlayerHealth);
            _progressBar = await _viewFactory.CreateProgressBar(_experiencePoints, _gameInitSystem.PlayerTransform);

            _weaponFactory.GetData(_gameInitSystem.PlayerTransform, _weaponHolder);
            await _weaponFactory.CreateEnemyDetectorForPlayer();

            _levelUpPanel.GetServices(_weaponFactory, _weaponHolder);
            _weaponFactory.GetMinesButton(_uiScene.MinesButton);

            uiRoot.AttachSceneUI(_uiScene.gameObject);

            _uiScene.GetUIStateMachine(uiRoot.UIStateMachine, uiRoot.UIRootButtons);

            _weaponFactory.WeaponIsCreated += _uiScene.WeaponPanel.SetData;

            _goldView.Show();

            _gameInitSystem.PlayerHealth.Die += _endGamePanel.Show;
            _gameInitSystem.PlayerHealth.Die += _uiRoot.UIRootButtons.Hide;
            _gameInitSystem.PlayerHealth.Die += _endGamePanel.SetDefeatPanel;
            _gameInitSystem.PlayerHealth.Die += _progressBar.Hide;
            _gameInitSystem.PlayerHealth.IsSpawnedHealingText += _floatingTextService.OnChangedFloatingText;
            
            uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged += _progressBar.ChangeText;
            uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged += _levelUpPanel.SetTitle;
            uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged += _endGamePanel.SetLabelText;

            _level.EndLevelTrigger.IsLevelCompleted += _levelUpPanel.OnEndGameTriggerIsReached;
            _level.EndLevelTrigger.IsLevelCompleted += _uiRoot.UIRootButtons.Hide;
            _level.EndLevelTrigger.IsLevelCompleted += _endGamePanel.SetVictoryPanel;

            _levelUpPanel.OnContinueButtonIsClicked += _endGamePanel.Show;

            _gameInitSystem.PlayerIsSpawned += _uiScene.WeaponPanel.Show;
            _gameInitSystem.PlayerIsSpawned += _progressBar.Show;
            _gameInitSystem.PlayerIsSpawned += _healthBar.Show;
            _gameInitSystem.PlayerIsSpawned += OnShowJoystick;

            _endGamePanel.RebornPlayerButton.onClick.AddListener(_gameInitSystem.CreateCapsule);
            _endGamePanel.RebornPlayerButton.onClick.AddListener(uiRoot.UIRootButtons.Show);

            _endGamePanel.GoToMainMenuButton.onClick.AddListener(GetMainMenuExitParameters);
            _endGamePanel.GoToMainMenuButton.onClick.AddListener(_uiScene.HandleGoToNextSceneButtonClick);

            _endGamePanel.NextLevelButton.onClick.AddListener(GetGameplayExitParameters);
            _endGamePanel.NextLevelButton.onClick.AddListener(_uiScene.HandleGoToNextSceneButtonClick);
            
            uiRoot.ExitPanel.OnExitToMainMenu += GetMainMenuExitParameters;
            uiRoot.ExitPanel.OnExitToMainMenu += _uiScene.HandleGoToNextSceneButtonClick;

// #if UNITY_EDITOR
            _uiScene.CheatsButton.onClick.AddListener(_cheatPanel.Show);
// #endif
            
            _weaponFactory.MinesIsCreated += _uiScene.ShowMinesButton;
            _experiencePoints.CurrentLevelIsUpgraded += _levelUpPanel.OnCurrentLevelIsUpgraded;

            var exitSceneSignalSubject = new Subject<Unit>();
            _uiScene.Bind(exitSceneSignalSubject);

            var exitToSceneSignal = exitSceneSignalSubject.Select(_ => _exitParameters);

            await _level.OnStartLevel();
            await TryLoadWeapons();

            return exitToSceneSignal;
        }

        private void OnDestroy()
        {
            YG2.SaveProgress();
            
            _weaponFactory.WeaponIsCreated -= _uiScene.WeaponPanel.SetData;

            _gameInitSystem.PlayerIsSpawned -= _uiScene.WeaponPanel.Show;
            _gameInitSystem.PlayerIsSpawned -= _healthBar.Show;
            _gameInitSystem.PlayerIsSpawned -= _progressBar.Show;
            _gameInitSystem.PlayerIsSpawned -= OnShowJoystick;
            
            _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged -= _progressBar.ChangeText;
            _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged -= _levelUpPanel.SetTitle;
            _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged -= _endGamePanel.SetLabelText;

            _gameInitSystem.PlayerHealth.Die -= _endGamePanel.Show;
            _gameInitSystem.PlayerHealth.Die -= _uiRoot.UIRootButtons.Hide;
            _gameInitSystem.PlayerHealth.Die -= _endGamePanel.SetDefeatPanel;
            _gameInitSystem.PlayerHealth.Die -= _progressBar.Hide;
            _gameInitSystem.PlayerHealth.IsSpawnedHealingText -= _floatingTextService.OnChangedFloatingText;

            _level.EndLevelTrigger.IsLevelCompleted -= _levelUpPanel.OnEndGameTriggerIsReached;
            _level.EndLevelTrigger.IsLevelCompleted -= _uiRoot.UIRootButtons.Hide;
            _level.EndLevelTrigger.IsLevelCompleted -= _endGamePanel.SetVictoryPanel;
            
            _levelUpPanel.OnContinueButtonIsClicked -= _endGamePanel.Show;

            _endGamePanel.GoToMainMenuButton.onClick.RemoveListener(GetMainMenuExitParameters);
            _endGamePanel.GoToMainMenuButton.onClick.RemoveListener(_uiScene.HandleGoToNextSceneButtonClick);
            
            _endGamePanel.NextLevelButton.onClick.RemoveListener(GetGameplayExitParameters);
            _endGamePanel.NextLevelButton.onClick.RemoveListener(_uiScene.HandleGoToNextSceneButtonClick);
            
            _endGamePanel.RebornPlayerButton.onClick.RemoveListener(_gameInitSystem.CreateCapsule);
            _endGamePanel.RebornPlayerButton.onClick.RemoveListener(_uiRoot.UIRootButtons.Show);

            _experiencePoints.CurrentLevelIsUpgraded -= _levelUpPanel.OnCurrentLevelIsUpgraded;

            _weaponFactory.MinesIsCreated -= _uiScene.ShowMinesButton;
            
// #if UNITY_EDITOR
            _uiScene.CheatsButton.onClick.RemoveListener(_cheatPanel.Show);
// #endif

            _updateSystems?.Destroy();
            _fixedUpdateSystems?.Destroy();
            _world?.Destroy();
        }
        
        private async UniTask TryLoadWeapons()
        {
            if (YG2.saves.GunCharacteristics != null)
                await _weaponFactory.CreateWeapon(WeaponType.Gun);
            if (YG2.saves.MachineGunCharacteristics != null)
                await _weaponFactory.CreateWeapon(WeaponType.MachineGun);
            if (YG2.saves.MinesCharacteristics != null)
                await _weaponFactory.CreateWeapon(WeaponType.Mines);
            if (YG2.saves.FragGrenadeCharacteristics != null)
                await _weaponFactory.CreateWeapon(WeaponType.FragGrenades);
            if (YG2.saves.FourBarrelMachineGunCharacteristics != null)
                await _weaponFactory.CreateWeapon(WeaponType.FourBarrelMachineGun);
            if (YG2.saves.ChainLightningGunCharacteristics != null)
                await _weaponFactory.CreateWeapon(WeaponType.ChainLightningGun);

            if (_weaponHolder.Weapons.Count == 0)
            {
                await _weaponFactory.CreateWeapon(WeaponType.Gun);
                _levelUpService.UpdateImprovementCardsByTypeWeapon(WeaponType.Gun);
            }
        }

        private void GetMainMenuExitParameters()
        {
            _uiRoot.UIRootButtons.Show();
            
            var mainMenuEnterParameters = new MainMenuEnterParameters("Enter parameters");
            _exitParameters = new GameplayExitParameters(mainMenuEnterParameters);
        }

        private void GetGameplayExitParameters()
        {
            _uiRoot.UIRootButtons.Show();
            
            int nextNumberLevel = _operationService.CurrentNumberLevel + 1;

            var sceneName = _operationService.GetSceneNameByNumber(nextNumberLevel);

            var gameplayEnterParameters = new GameplayEnterParameters(_operationService.CurrentOperation,
                nextNumberLevel, sceneName);

            _exitParameters = new GameplayExitParameters(gameplayEnterParameters);
        }

        private async UniTask InitData()
        {
            _levelData = _dataFactory.CreateLevelData(_operationService.CurrentOperation,
                _operationService.CurrentNumberLevel);

            _playerInitData = await _dataFactory.CreatePlayerData();
            _smallAlienEnemyData = await _dataFactory.CreateSmallEnemyAlienData();
            _bigAlienEnemyData = await _dataFactory.CreateBigEnemyAlienData();
            _gunnerEnemyAlienData = await _dataFactory.CreateGunnerAlienEnemyData();
            _stoneData = await _dataFactory.CreateStoneData();
            _capsuleData = await _dataFactory.CreateCapsuleData();
            _healingCoreData = await _dataFactory.CreateHealingCoreData();
            _goldCoreData = await _dataFactory.CreateGoldCoreData();
            _alienCocoonData = await _dataFactory.CreateAlienCocoonData();
            _alienTurretEnemyData = await _dataFactory.CreateAlienTurretEnemyData();
            _iceCrystalData = await _dataFactory.CreateIceCrystalData();
        }

        private void InitEcs()
        {
            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world);

            _updateSystems.Inject(_dialoguePanel);
            _updateSystems.Inject(_experiencePoints);
            _updateSystems.Inject(_floatingTextService);
            _updateSystems.Inject(_currencyService);
            _updateSystems.Inject(_audioSoundsService);
            _updateSystems.Inject(_viewFactory);
            _updateSystems.Inject(_pauseService);
            _updateSystems.Inject(_level);
            _updateSystems.Inject(_dataBaseService);
            _updateSystems.Inject(_resourceService);
            _updateSystems.Inject(_enemyService);
            _updateSystems.Inject(_playerService);
            _updateSystems.Inject(_playerInitData);
            _updateSystems.Inject(_smallAlienEnemyData);
            _updateSystems.Inject(_bigAlienEnemyData);
            _updateSystems.Inject(_gunnerEnemyAlienData);
            _updateSystems.Inject(_stoneData);
            _updateSystems.Inject(_goldCoreData);
            _updateSystems.Inject(_healingCoreData);
            _updateSystems.Inject(_capsuleData);
            _updateSystems.Inject(_levelData);
            _updateSystems.Inject(_levelTextService);
            _updateSystems.Inject(_coreService);
            _updateSystems.Inject(_alienCocoonData);
            _updateSystems.Inject(_alienTurretEnemyData);
            _updateSystems.Inject(_iceCrystalData);
            _updateSystems.Inject(_particleEffectsService);
            _updateSystems.Inject(_container);

            _updateSystems.Add(_gameInitSystem = new GameInitSystem());
            _updateSystems.Add(new PlayerInputSystem());
            _updateSystems.Add(new MainCameraSystem(_cinemachineVirtualCamera));
            _updateSystems.Add(new PlayerAnimatedSystem());
            _updateSystems.Add(new EnemyAnimatedSystem());
            _updateSystems.Add(new EnemyMeleeAttackSystem());
            _updateSystems.Add(new ResourcesAnimatedSystem());
            _updateSystems.Init();

            _fixedUpdateSystems.Inject(_bigAlienEnemyData.ProjectilePrefab);
            _fixedUpdateSystems.Add(new PlayerMoveSystem());
            _fixedUpdateSystems.Add(new FollowSystem());
            _fixedUpdateSystems.Add(new EnemyRangeAttackSystem());
            _fixedUpdateSystems.Add(new AttackCheckSystem());
            _fixedUpdateSystems.Add(new PatrolSystem());
            _fixedUpdateSystems.Init();
        }

        private void OnShowJoystick()
        {
            if (YG2.envir.isDesktop)
            {
                _uiScene.Joystick.Hide();
            }
            else
            {
                _uiScene.Joystick.Show();
            }
        }
    }
}