using System;
using Build.Game.Scripts.Game.Gameplay.GameplayRoot.View;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.UI;
using Project.Scripts.UI.StateMachine;
using Project.Scripts.UI.StateMachine.States;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuRootBinder : MonoBehaviour
{
    private const string CurrentOperationCodeName = nameof(CurrentOperationCodeName);

    [SerializeField] private MainMenuElements _uiScene;
    [SerializeField] private ChoosingOperationPanel _choosingOperationPanel;
    [SerializeField] private AudioSource _buttonSound;
    
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _startOperationButton;
    
    private Subject<Unit> _exitSceneSubjectSignal;
    private UIStateMachine _uiStateMachine;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(HandlePlayButtonClick);
        _startOperationButton.onClick.AddListener(HandleGoToGameplayButtonClick);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(HandlePlayButtonClick);
        _startOperationButton.onClick.RemoveListener(HandleGoToGameplayButtonClick);
    }

    public void GetUIStateMachineAndStates(UIStateMachine uiStateMachine, UIRootButtons uiRootButtons)
    {
        _uiStateMachine = uiStateMachine;
        
        _uiStateMachine.AddState(new MainMenuState(_uiScene, uiRootButtons));
        _uiStateMachine.AddState(new ChoosingOperationPanelState(_choosingOperationPanel));
        _choosingOperationPanel.GetUIStateMachine(_uiStateMachine);
        
        _uiStateMachine.EnterIn<MainMenuState>();
    }

    public void HandleGoToGameplayButtonClick()
    {
        _buttonSound.PlayOneShot(_buttonSound.clip);
        PlayerPrefs.SetString(CurrentOperationCodeName, _choosingOperationPanel.CurrentOperation.CodeName);
        _exitSceneSubjectSignal?.OnNext(Unit.Default);
    }

    public void Bind(Subject<Unit> exitSceneSignalSubject)
    {
        _exitSceneSubjectSignal = exitSceneSignalSubject;
    }

    private void HandlePlayButtonClick()
    {
        _uiStateMachine.EnterIn<ChoosingOperationPanelState>();
    }
}
 