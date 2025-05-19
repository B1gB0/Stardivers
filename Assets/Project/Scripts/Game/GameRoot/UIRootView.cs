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
        [SerializeField] private LocalizationLanguageSwitcher _localizationLanguageSwitcher;

        [SerializeField] private Button _settingsButton;

        private AudioSoundsService _audioSoundsService;

        public UIStateMachine UIStateMachine { get; private set; }

        public UIRootButtons UIRootButtons => _uiRootButtons;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService)
        {
            _audioSoundsService = audioSoundsService;
        }

        private void Awake()
        {
            UIStateMachine = new UIStateMachine();
            UIStateMachine.AddState(new SettingsPanelState(_settingsPanel));
            UIStateMachine.AddState(new LoadingPanelState(_loadingPanel));
        }

        private void Start()
        {
            _settingsPanel.SetValuesVolume();
            _settingsPanel.Hide();
        }

        private void OnEnable()
        {
            _settingsButton.onClick.AddListener(ShowSettingsPanel);
            _settingsPanel.OnExitButtonPressed += ShowUIScene;
        }

        private void OnDisable()
        {
            _settingsButton.onClick.RemoveListener(ShowSettingsPanel);
            _settingsPanel.OnExitButtonPressed -= ShowUIScene;
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

        private void ShowSettingsPanel()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            UIStateMachine.EnterIn<SettingsPanelState>();
            _settingsPanel.StopGame();
        }

        private void ShowUIScene()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            
            var sceneName = SceneManager.GetActiveScene().name;
            
            if(sceneName == Scenes.MainMenu)
                UIStateMachine.EnterIn<MainMenuState>();
            
            if(sceneName == Scenes.Gameplay)
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