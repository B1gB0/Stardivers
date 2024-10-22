using Build.Game.Scripts.Game.Gameplay.View;
using Project.Game.Scripts;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.UI;
using Project.Scripts.UI.StateMachine;
using Project.Scripts.UI.StateMachine.States;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayRootBinder : MonoBehaviour
{
    [field: SerializeField] public GameplayElements UIScene { get; private set; }

    [field: SerializeField] public Joystick Joystick { get; private set; }
    
    [field: SerializeField] public Button MinesButton { get; private set; }

    private AudioSoundsService _audioSoundsService;
    private PauseService _pauseService;

    private Subject<Unit> _exitSceneSignalSubject;
    private UIStateMachine _uiStateMachine;

    [Inject]
    public void Construct(AudioSoundsService audioSoundsService, PauseService pauseService)
    {
        _audioSoundsService = audioSoundsService;
    }

    private void Awake()
    {
        if (!Application.isMobilePlatform)
        {
            Joystick.Hide();
        }
    }

    private void OnDestroy()
    {
        _uiStateMachine.RemoveState<GameplayState>();
    }

    public void GetUIStateMachine(UIStateMachine uiStateMachine, UIRootButtons uiRootButtons)
    {
        _uiStateMachine = uiStateMachine;
        _uiStateMachine.AddState(new GameplayState(UIScene, uiRootButtons));
        _uiStateMachine.EnterIn<GameplayState>();
    }

    public void Bind(Subject<Unit> exitSceneSignalSubject)
    {
        _exitSceneSignalSubject = exitSceneSignalSubject;
    }

    public void ShowMinesButton()
    {
        MinesButton.gameObject.SetActive(true);
    }
    
    public void HideMinesButton()
    {
        MinesButton.gameObject.SetActive(false);
    }
    
    public void HandleGoToMainMenuButtonClick()
    {
        _audioSoundsService.PlaySound(Sounds.Button);
        _exitSceneSignalSubject?.OnNext(Unit.Default);
    }
}
