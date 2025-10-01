using System.Collections.Generic;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Panel
{
    public class EndGamePanel : MonoBehaviour, IView
    {
        private const string VictoryLabelText = "Victory!";
        private const string DefeatLabelText = "Defeat!";
        
        private readonly Color _redColor = Color.red;
        private readonly Color _blueColor = Color.blue;

        [SerializeField] private Text _labelText;
        [SerializeField] private Button _goToMainMenuButton;
        [SerializeField] private Button _rebornPlayerButton;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private List<Image> _images;
        
        private IPauseService _pauseService;
        private OperationService _operationService;

        public Button GoToMainMenuButton => _goToMainMenuButton;
        
        public Button RebornPlayerButton => _rebornPlayerButton;
        
        public Button NextLevelButton => _nextLevelButton;

        [Inject]
        public void Construct(IPauseService pauseService, OperationService operationService)
        {
            _pauseService = pauseService;
            _operationService = operationService;
        }

        private void OnEnable()
        {
            _goToMainMenuButton.onClick.AddListener(Hide);
            _rebornPlayerButton.onClick.AddListener(Hide);
            
            _rebornPlayerButton.onClick.AddListener(OnPlayGame);
            _nextLevelButton.onClick.AddListener(OnPlayGame);
            _goToMainMenuButton.onClick.AddListener(OnPlayGame);
        }

        private void OnDisable()
        {
            _goToMainMenuButton.onClick.RemoveListener(Hide);
            _rebornPlayerButton.onClick.RemoveListener(Hide);
            
            _rebornPlayerButton.onClick.RemoveListener(OnPlayGame);
            _nextLevelButton.onClick.RemoveListener(OnPlayGame);
            _goToMainMenuButton.onClick.RemoveListener(OnPlayGame);
        }

        public void SetVictoryPanel()
        {
            _nextLevelButton.gameObject.SetActive(_operationService.CurrentNumberLevel != 
                                                  _operationService.CurrentOperation.Maps.Count - 1);

            _rebornPlayerButton.gameObject.SetActive(false);
            _labelText.text = VictoryLabelText;
            OnChangeColor(_blueColor);
        }

        public void SetDefeatPanel()
        {
            _rebornPlayerButton.gameObject.SetActive(true);
            _nextLevelButton.gameObject.SetActive(false);
            _labelText.text = DefeatLabelText;
            OnChangeColor(_redColor);
        }

        public void Show()
        {
            _pauseService.StopGame();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void OnChangeColor(Color color)
        {
            foreach (var image in _images)
            {
                image.color = color;
            }
        }

        private void OnPlayGame()
        {
            _pauseService.PlayGame();
        }
    }
}
