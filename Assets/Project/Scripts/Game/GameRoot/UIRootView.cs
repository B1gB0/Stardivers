using Project.Game.Scripts;
using Project.Scripts.Localization;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.StateMachine;
using Project.Scripts.UI.StateMachine.States;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Scripts.Game.GameRoot
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private UISceneContainer _uiSceneContainer;
        [SerializeField] private UIRootButtons _uiRootButtons;

        [SerializeField] private LoadingPanel _loadingPanel;
        [SerializeField] private SettingsPanel _settingsPanel;
        [SerializeField] private LeaderboardPanel _leaderboardPanel;
        [SerializeField] private LocalizationLanguageSwitcher _localizationLanguageSwitcher;

        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _leaderboardButton;

        private AudioSoundsService _audioSoundsService;
        private IPauseService _pauseService;

        public UIStateMachine UIStateMachine { get; private set; }

        public UIRootButtons UIRootButtons => _uiRootButtons;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, IPauseService pauseService)
        {
            _audioSoundsService = audioSoundsService;
            _pauseService = pauseService;
        }

        private void Awake()
        {
            UIStateMachine = new UIStateMachine();
            UIStateMachine.AddState(new SettingsPanelState(_settingsPanel));
            UIStateMachine.AddState(new LoadingPanelState(_loadingPanel));
            UIStateMachine.AddState(new LeaderboardPanelState(_leaderboardPanel));
        }

        private void Start()
        {
            _settingsPanel.SetValuesVolume();
            _settingsPanel.Hide();
        }

        private void OnEnable()
        {
            _settingsButton.onClick.AddListener(ShowSettingsPanel);
            _settingsPanel.OnBackToSceneButtonPressed += ShowUIScene;
            _settingsPanel.OnBackToSceneButtonPressed += PlayGame;
            
            _leaderboardButton.onClick.AddListener(ShowLeaderboardPanel);
            _leaderboardPanel.OnBackToSceneButtonPressed += ShowUIScene;
            _leaderboardPanel.OnBackToSceneButtonPressed += PlayGame;
        }

        private void OnDisable()
        {
            _settingsButton.onClick.RemoveListener(ShowSettingsPanel);
            _settingsPanel.OnBackToSceneButtonPressed -= ShowUIScene;
            _settingsPanel.OnBackToSceneButtonPressed -= PlayGame;
            
            _leaderboardButton.onClick.RemoveListener(ShowLeaderboardPanel);
            _leaderboardPanel.OnBackToSceneButtonPressed -= ShowUIScene;
            _leaderboardPanel.OnBackToSceneButtonPressed -= PlayGame;
        }

        public void ShowLoadingProgress(float progress)
        {
            _loadingPanel.SetProgressText(progress);
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();
            
            sceneUI.transform.SetParent(_uiSceneContainer.transform, false);
        }

        private void StopGame()
        {
            if (SceneManager.GetActiveScene().name == Scenes.MainMenu)
                return;

            _pauseService.StopGame();
        }

        private void PlayGame()
        {
            if (SceneManager.GetActiveScene().name == Scenes.MainMenu)
                return;

            _pauseService.PlayGame();
            _audioSoundsService.PlaySound(Sounds.Button);
        }

        private void ShowSettingsPanel()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            UIStateMachine.EnterIn<SettingsPanelState>();
            StopGame();
        }

        private void ShowLeaderboardPanel()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            UIStateMachine.EnterIn<LeaderboardPanelState>();
            StopGame();
        }

        private void ShowUIScene()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            
            var sceneName = SceneManager.GetActiveScene().name;
            
            if(sceneName == Scenes.MainMenu)
                UIStateMachine.EnterIn<MainMenuState>();
            
            if(sceneName != Scenes.MainMenu)
                UIStateMachine.EnterIn<GameplayState>();
        }

        private void ClearSceneUI()
        {
            var childCount = _uiSceneContainer.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(_uiSceneContainer.transform.GetChild(i).gameObject);
            }
        }
    }
}