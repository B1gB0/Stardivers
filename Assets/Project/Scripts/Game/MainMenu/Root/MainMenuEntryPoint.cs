using Build.Game.Scripts.Game.Gameplay;
using Build.Game.Scripts.Game.Gameplay.GameplayRoot;
using R3;
using Reflex.Extensions;
using Reflex.Injectors;
using Source.Game.Scripts;
using UnityEngine;

namespace Project.Scripts.Game.MainMenu.Root
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        private const string CurrentOperationCodeName = nameof(CurrentOperationCodeName);
        
        [SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;
        
        private UIMainMenuRootBinder _uiScene;
        private string currentOperation;
        private string saveFileName;

        public Observable<MainMenuExitParameters> Run(UIRootView uiRoot, MainMenuEnterParameters enterParameters)
        {
            _uiScene = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(_uiScene.gameObject);
            
            var container = gameObject.scene.GetSceneContainer();
            GameObjectInjector.InjectRecursive(uiRoot.gameObject, container);
            
            _uiScene.GetUIStateMachineAndStates(uiRoot.UIStateMachine, uiRoot.UIRootButtons);

            var exitSignalSubject = new Subject<Unit>();
            _uiScene.Bind(exitSignalSubject);

            saveFileName = "Save";
            currentOperation = PlayerPrefs.GetString(CurrentOperationCodeName);

            var gameplayEnterParameters = new GameplayEnterParameters(saveFileName, currentOperation);
            var mainMenuExitParameters = new MainMenuExitParameters(gameplayEnterParameters);

            var exitToGameplaySceneSignal = exitSignalSubject.Select(_ => mainMenuExitParameters);

            return exitToGameplaySceneSignal;
        }
    }
}