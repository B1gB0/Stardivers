using Build.Game.Scripts.Game.Gameplay.GameplayRoot.View;
using Project.Game.Scripts;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.UI;
using Project.Scripts.UI.StateMachine;
using Project.Scripts.UI.StateMachine.States;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuRootBinder : MonoBehaviour
{
    private const string CurrentOperationCodeName = nameof(CurrentOperationCodeName);

    [SerializeField] private MainMenuElements _uiScene;
    [SerializeField] private ChoosingOperationPanel _choosingOperationPanel;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _startOperationButton;
    
    private Subject<Unit> _exitSceneSubjectSignal;
    private AudioSoundsService _audioSoundsService;
    private UIStateMachine _uiStateMachine;

    [Inject]
    public void Construct(AudioSoundsService audioSoundsService)
    {
        _audioSoundsService = audioSoundsService;
    }

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
    
    private void OnDestroy()
    {
        _uiStateMachine.RemoveState<MainMenuState>();
        _uiStateMachine.RemoveState<ChoosingOperationPanelState>();
    }

    public void GetUIStateMachineAndStates(UIStateMachine uiStateMachine, UIRootButtons uiRootButtons)
    {
        _uiStateMachine = uiStateMachine;
        
        _uiStateMachine.AddState(new MainMenuState(_uiScene, uiRootButtons));
        _uiStateMachine.AddState(new ChoosingOperationPanelState(_choosingOperationPanel));
        _choosingOperationPanel.GetUIStateMachine(_uiStateMachine);
        
        _uiStateMachine.EnterIn<MainMenuState>();
    }

    public void Bind(Subject<Unit> exitSceneSignalSubject)
    {
        _exitSceneSubjectSignal = exitSceneSignalSubject;
    }
    
    private void HandleGoToGameplayButtonClick()
    {
        _audioSoundsService.PlaySound(Sounds.Button);
        PlayerPrefs.SetString(CurrentOperationCodeName, _choosingOperationPanel.CurrentOperation.CodeName);
        _exitSceneSubjectSignal?.OnNext(Unit.Default);
    }

    private void HandlePlayButtonClick()
    {
        _audioSoundsService.PlaySound(Sounds.Button);
        _uiStateMachine.EnterIn<ChoosingOperationPanelState>();
    }
}
 