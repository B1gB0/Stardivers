using R3;
using Reflex.Extensions;
using Reflex.Injectors;
using Source.Game.Scripts;
using UnityEngine;

namespace Build.Game.Scripts.Game.Gameplay.GameplayRoot
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;
        
        private UIMainMenuRootBinder _uiScene;
        private string _currentOperation;
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

            var gameplayEnterParameters = new GameplayEnterParameters(saveFileName, _currentOperation);
            var mainMenuExitParameters = new MainMenuExitParameters(gameplayEnterParameters);

            var exitToGameplaySceneSignal = exitSignalSubject.Select(_ => mainMenuExitParameters);

            return exitToGameplaySceneSignal;
        }
    }
}