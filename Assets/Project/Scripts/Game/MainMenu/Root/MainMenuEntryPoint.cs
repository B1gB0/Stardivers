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
        private IGoldService _goldService;
        
        private MainMenuExitParameters _exitParameters;

        [Inject]
        private void Construct(OperationService operationService, IDataBaseService dataBaseService, 
            IGoldService goldService)
        {
            _dataBaseService = dataBaseService;
            _operationService = operationService;
            _goldService = goldService;
        }

        private async void Start()
        {
            if(_operationService.IsInitiated)
                return;
            
            await _dataBaseService.Init();
            await _operationService.Init();
            await _goldService.Init();
        }

        public Observable<MainMenuExitParameters> Run(UIRootView uiRoot, MainMenuEnterParameters enterParameters)
        {
            _uiScene = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(_uiScene.gameObject);
            
            _uiScene.OnGameplayStarted += GetMainMenuExitParameters;
            
            var container = gameObject.scene.GetSceneContainer();
            GameObjectInjector.InjectRecursive(uiRoot.gameObject, container);

            _uiScene.GetUIStateMachineAndStates(uiRoot.UIStateMachine, uiRoot.UIRootButtons);

            var exitSignalSubject = new Subject<Unit>();
            _uiScene.Bind(exitSignalSubject);

            var exitToGameplaySceneSignal = exitSignalSubject.Select(_ => _exitParameters);

            return exitToGameplaySceneSignal;
        }

        private void GetMainMenuExitParameters()
        {
            var sceneName = _operationService.GetSceneNameByCurrentNumber();

            var gameplayEnterParameters = new GameplayEnterParameters(_operationService.CurrentOperation,
                _operationService.CurrentNumberLevel, sceneName);
            
            _exitParameters = new MainMenuExitParameters(gameplayEnterParameters);
        }

        private void OnDestroy()
        {
            _uiScene.OnGameplayStarted -= GetMainMenuExitParameters;
        }
    }
}