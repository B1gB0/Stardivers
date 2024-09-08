using R3;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayRootBinder : MonoBehaviour
{
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

    public void HandleGoToMainMenuButtonClick()
    {
        _exitSceneSignalSubject?.OnNext(Unit.Default);
    }

    public void Bind(Subject<Unit> exitSceneSignalSubject)
    {
        _exitSceneSignalSubject = exitSceneSignalSubject;
    }
}
