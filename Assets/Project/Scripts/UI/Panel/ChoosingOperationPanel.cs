using System;
using System.Collections.Generic;
using Project.Scripts.Operations;
using Project.Scripts.UI.StateMachine;
using Project.Scripts.UI.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class ChoosingOperationPanel : MonoBehaviour, IView
    {
        [SerializeField] private List<Operation> _operations = new();
        [SerializeField] private OperationView _operationView;
        
        [SerializeField] private Button _backToMainMenuButton;
        [SerializeField] private Button _priviousButton;
        [SerializeField] private Button _nextButton;

        private int _currentIndex;
        private UIStateMachine _uiStateMachine;
        
        public Operation CurrentOperation { get; private set; }

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
            _uiStateMachine.EnterIn<MainMenuState>();
        }

        private void SetNextOperation()
        {
            if (_currentIndex == _operations.Count - 1)
                _currentIndex = 0;
            else
                _currentIndex++;
        
            SetOperation(_currentIndex);
        }

        private void SetPreviousOperation()
        {
            if (_currentIndex == 0)
                _currentIndex = _operations.Count - 1;
            else
                _currentIndex--;
        
            SetOperation(_currentIndex);
        }
    
        private void SetOperation(int index)
        {
            CurrentOperation = _operations[index];
            _operationView.GetOperation(CurrentOperation);
        }
    }
}