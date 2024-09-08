using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Game.Scripts
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private Transform _uiSceneContainer;

        [SerializeField] private LoadingPanel _loadingPanel;
        [SerializeField] private SettingsPanel _settingsPanel;

        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _backToSceneButton;

        public PauseService PauseService { get; } = new ();

        private void Awake()
        {
            _settingsPanel.GetPauseService(PauseService);
            HideLoadingScreen();
        }

        private void Start()
        {
            _settingsPanel.SetValuesVolume();
            _settingsPanel.Hide();
        }

        private void OnEnable()
        {
            _settingsButton.onClick.AddListener(_settingsPanel.Show);
            _settingsButton.onClick.AddListener(_settingsPanel.StopGame);
            _settingsButton.onClick.AddListener(HideUIScene);
            _backToSceneButton.onClick.AddListener(ShowUIScene);
        }

        private void OnDisable()
        {
            _settingsButton.onClick.RemoveListener(_settingsPanel.Show);
            _settingsButton.onClick.RemoveListener(_settingsPanel.StopGame);
            _settingsButton.onClick.RemoveListener(HideUIScene);
            _backToSceneButton.onClick.RemoveListener(ShowUIScene);
        }
        
        public void ShowLoadingScreen()
        {
            _loadingPanel.Show();
            _loadingPanel.RotateLoadingWheel();
        }

        public void ShowLoadingProgress(float progress)
        {
            _loadingPanel.SetProgressText(progress);
        }
        
        public void HideLoadingScreen()
        {
            _loadingPanel.Hide();
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();
            
            sceneUI.transform.SetParent(_uiSceneContainer, false);
        }

        private void ShowUIScene()
        {
            _uiSceneContainer.gameObject.SetActive(true);
        }

        private void HideUIScene()
        {
            _uiSceneContainer.gameObject.SetActive(false);
        }

        private void ClearSceneUI()
        {
            var childCount = _uiSceneContainer.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(_uiSceneContainer.GetChild(i).gameObject);
            }
        }
    }
}