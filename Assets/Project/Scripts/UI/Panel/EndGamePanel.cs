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
        
        private PauseService _pauseService;
        private OperationSetterService operationSetterService;

        public Button GoToMainMenuButton => _goToMainMenuButton;
        
        public Button RebornPlayerButton => _rebornPlayerButton;
        
        public Button NextLevelButton => _nextLevelButton;

        [Inject]
        public void Construct(PauseService pauseService, OperationSetterService operationSetterService)
        {
            _pauseService = pauseService;
            this.operationSetterService = operationSetterService;
        }

        private void OnEnable()
        {
            _goToMainMenuButton.onClick.AddListener(Hide);
            _rebornPlayerButton.onClick.AddListener(Hide);
            
            _rebornPlayerButton.onClick.AddListener(_pauseService.PlayGame);
            _nextLevelButton.onClick.AddListener(_pauseService.PlayGame);
            _goToMainMenuButton.onClick.AddListener(_pauseService.PlayGame);
        }

        private void OnDisable()
        {
            _goToMainMenuButton.onClick.RemoveListener(Hide);
            _rebornPlayerButton.onClick.RemoveListener(Hide);
            
            _rebornPlayerButton.onClick.RemoveListener(_pauseService.PlayGame);
            _nextLevelButton.onClick.RemoveListener(_pauseService.PlayGame);
            _goToMainMenuButton.onClick.RemoveListener(_pauseService.PlayGame);
        }

        public void SetVictoryPanel()
        {
            _nextLevelButton.gameObject.SetActive(operationSetterService.CurrentNumberLevel != 
                                                  operationSetterService.CurrentOperation.Maps.Count - 1);

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

        private void OnChangeColor(Color color)
        {
            foreach (var image in _images)
            {
                image.color = color;
            }
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
    }
}
