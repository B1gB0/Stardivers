using DG.Tweening;
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
        private const int MinValue = 0;
        private const int CountCorrectFactor = 1;
        
        [SerializeField] private OperationView _operationView;

        [SerializeField] private Button _backToMainMenuButton;
        [SerializeField] private Button _priviousButton;
        [SerializeField] private Button _nextButton;

        private int _currentIndex;
        private UIStateMachine _uiStateMachine;

        private AudioSoundsService _audioSoundsService;
        private OperationService _operationService;
        private ITweenAnimationService _tweenAnimationService;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, OperationService operationService, 
            IDataBaseService dataBaseService, ITweenAnimationService tweenAnimationService)
        {
            _audioSoundsService = audioSoundsService;
            _operationService = operationService;
            _tweenAnimationService = tweenAnimationService;
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
            transform.DOKill(true);
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
            _tweenAnimationService.AnimateScale(transform);
            SetOperation(_currentIndex);
        }

        public void Hide()
        {
            _tweenAnimationService.AnimateScale(transform, true);
        }

        private void SetOperation(int index)
        {
            _operationService.SetCurrentOperation(index);
            _operationView.GetOperation(_operationService.CurrentOperation);
        }

        private void HandleBackButtonClick()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            _uiStateMachine.EnterIn<MainMenuState>();
        }

        private void SetNextOperation()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            
            if (_currentIndex == _operationService.Operations.Count - CountCorrectFactor)
                _currentIndex = MinValue;
            else
                _currentIndex++;

            SetOperation(_currentIndex);
        }

        private void SetPreviousOperation()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            
            if (_currentIndex == MinValue)
                _currentIndex = _operationService.Operations.Count - CountCorrectFactor;
            else
                _currentIndex--;
        
            SetOperation(_currentIndex);
        }

        private void OnDestroy()
        {
            transform.DOKill(true);
        }
    }
}