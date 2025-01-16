using Project.Game.Scripts;
using Project.Scripts.Services;
using Project.Scripts.UI.StateMachine;
using Project.Scripts.UI.StateMachine.States;
using Project.Scripts.UI.View;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Panel
{
    public class ChoosingOperationPanel : MonoBehaviour, IView
    {
        [SerializeField] private OperationView _operationView;
        
        [SerializeField] private Button _backToMainMenuButton;
        [SerializeField] private Button _priviousButton;
        [SerializeField] private Button _nextButton;

        private int _currentIndex;
        private UIStateMachine _uiStateMachine;

        private AudioSoundsService _audioSoundsService;
        private OperationAndLevelSetterService operationAndLevelSetterService;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, OperationAndLevelSetterService operationAndLevelSetterService)
        {
            _audioSoundsService = audioSoundsService;
            this.operationAndLevelSetterService = operationAndLevelSetterService;
        }

        private void Start()
        {
            SetOperation(_currentIndex);
        }

        private void OnEnable()
        {
            _priviousButton.onClick.AddListener(SetPreviousOperation);
            _nextButton.onClick.AddListener(SetNextOperation);
            _backToMainMenuButton.onClick.AddListener(HandleBackButtonClick);
        }

        private void OnDisable()
        {
            _priviousButton.onClick.RemoveListener(SetPreviousOperation);
            _nextButton.onClick.RemoveListener(SetNextOperation);
            _backToMainMenuButton.onClick.RemoveListener(HandleBackButtonClick);
        }

        public void GetUIStateMachine(UIStateMachine uiStateMachine)
        {
            _uiStateMachine = uiStateMachine;
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void HandleBackButtonClick()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            _uiStateMachine.EnterIn<MainMenuState>();
        }

        private void SetNextOperation()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            
            if (_currentIndex == operationAndLevelSetterService.Operations.Count - 1)
                _currentIndex = 0;
            else
                _currentIndex++;
        
            SetOperation(_currentIndex);
        }

        private void SetPreviousOperation()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            
            if (_currentIndex == 0)
                _currentIndex = operationAndLevelSetterService.Operations.Count - 1;
            else
                _currentIndex--;
        
            SetOperation(_currentIndex);
        }
    
        private void SetOperation(int index)
        {
            operationAndLevelSetterService.SetCurrentOperation(index);
            _operationView.GetOperation(operationAndLevelSetterService.CurrentOperation);
        }
    }
}