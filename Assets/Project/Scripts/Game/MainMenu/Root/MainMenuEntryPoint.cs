using Cysharp.Threading.Tasks;
using Project.Scripts.Game.Gameplay.Root;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.Game.MainMenu.Root.View;
using Project.Scripts.Services;
using R3;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;

namespace Project.Scripts.Game.MainMenu.Root
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;
        
        private UIMainMenuRootBinder _uiScene;
        private OperationService _operationService;
        private IDataBaseService _dataBaseService;

        [Inject]
        private void Construct(OperationService operationService, IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
            _operationService = operationService;
        }

        private async void Start()
        {
            await _dataBaseService.Init();
        }

        public Observable<MainMenuExitParameters> Run(UIRootView uiRoot, MainMenuEnterParameters enterParameters)
        {
            _uiScene = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(_uiScene.gameObject);
            
            var container = gameObject.scene.GetSceneContainer();
            GameObjectInjector.InjectRecursive(uiRoot.gameObject, container);

            _uiScene.GetUIStateMachineAndStates(uiRoot.UIStateMachine, uiRoot.UIRootButtons);
            _operationService.Init().Forget();

            var exitSignalSubject = new Subject<Unit>();
            _uiScene.Bind(exitSignalSubject);

            var gameplayEnterParameters = new GameplayEnterParameters(_operationService.CurrentOperation,
                _operationService.CurrentNumberLevel);
            var mainMenuExitParameters = new MainMenuExitParameters(gameplayEnterParameters);

            var exitToGameplaySceneSignal = exitSignalSubject.Select(_ => mainMenuExitParameters);

            return exitToGameplaySceneSignal;
        }
    }
}