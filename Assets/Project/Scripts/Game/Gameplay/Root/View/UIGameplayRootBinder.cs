using Build.Game.Scripts.Game.Gameplay.View;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.UI.StateMachine;
using Project.Scripts.UI.StateMachine.States;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayRootBinder : MonoBehaviour
{
    [field: SerializeField] public GameplayElements _uiScene { get; private set; }
    
    [field: SerializeField] public Joystick Joystick { get; private set; }
    
    [field: SerializeField] public Button MinesButton { get; private set; }

    private void Awake()
    {
        if (!Application.isMobilePlatform)
        {
            Joystick.Hide();
        }
    }

    private Subject<Unit> _exitSceneSignalSubject;
    private UIStateMachine _uiStateMachine;

    public void GetUIStateMachine(UIStateMachine uiStateMachine, UIRootButtons uiRootButtons)
    {
        _uiStateMachine = uiStateMachine;
        _uiStateMachine.AddState(new GameplayState(_uiScene, uiRootButtons));
        _uiStateMachine.EnterIn<GameplayState>();
    }

    public void HandleGoToMainMenuButtonClick()
    {
        _exitSceneSignalSubject?.OnNext(Unit.Default);
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
}
