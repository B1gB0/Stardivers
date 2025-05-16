using System.Collections;
using Cysharp.Threading.Tasks;
using Project.Scripts.Game.Gameplay.Root;
using Project.Scripts.Game.MainMenu.Root;
using Project.Scripts.Services;
using Project.Scripts.UI.StateMachine.States;
using R3;
using Reflex.Attributes;
using Reflex.Core;
using Source.Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Game.GameRoot
{
    public class GameEntryPoint
    {
        private const string UIRootViewPath = "UIRoot";
        private const string CoroutinesName = "[Coroutines]";

        private readonly Coroutines _coroutines;
        private readonly UIRootView _uiRoot;

        private static GameEntryPoint _instance;
        
        private AsyncOperation _asyncOperation;
        private OperationService _operationService;
        private IDataBaseService _dataBaseService;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            _instance = new GameEntryPoint();
            _instance.StartGame();
        }

        private GameEntryPoint()
        {
            _coroutines = new GameObject(CoroutinesName).AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines.gameObject);

            var prefabUIRoot = Resources.Load<UIRootView>(UIRootViewPath);
            _uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRoot.gameObject);
        }
        
        [Inject]
        private void Construct(OperationService operationService, IDataBaseService dataBaseService)
        {
            _operationService = operationService;
            _dataBaseService = dataBaseService;
        }

        private void StartGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.MainMenu)
            {
                _coroutines.StartCoroutine(LoadAndStartMainMenu());
                return;
            }

            if (sceneName == Scenes.Gameplay)
            {
                var enterParameters = new GameplayEnterParameters("", _operationService.CurrentOperation,
                    _operationService.CurrentNumberLevel);
                
                _coroutines.StartCoroutine(LoadAndStartGameplay(enterParameters));
                return;
            }

            if (sceneName != Scenes.Boot)
            {
                return;
            }
#endif

            _coroutines.StartCoroutine(LoadAndStartMainMenu());
        }

        private IEnumerator LoadAndStartMainMenu(MainMenuEnterParameters enterParameters = null)
        {
            yield return LoadScene(Scenes.Boot);
            
            _uiRoot.UIStateMachine.EnterIn<LoadingPanelState>();
            
            yield return LoadScene(Scenes.MainMenu);

            yield return new WaitForSeconds(1);

            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            sceneEntryPoint.Run(_uiRoot, enterParameters).Subscribe(mainMenuExitParameters =>
            {
                var targetSceneName = mainMenuExitParameters.TargetSceneEnterParameters.SceneName;

                if (targetSceneName == Scenes.Gameplay)
                {
                    _coroutines.StartCoroutine(LoadAndStartGameplay(mainMenuExitParameters.
                        TargetSceneEnterParameters.As<GameplayEnterParameters>()));
                }
            } );
        }
        
        private IEnumerator LoadAndStartGameplay(GameplayEnterParameters enterParameters)
        {
            yield return LoadScene(Scenes.Boot);

            _uiRoot.UIStateMachine.EnterIn<LoadingPanelState>();
            
            yield return LoadScene(Scenes.Gameplay);
            
            yield return new WaitForSeconds(1);

            var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
            sceneEntryPoint.Run(_uiRoot, enterParameters).Subscribe(gameplayExitParameters =>
            {
                var targetSceneName = gameplayExitParameters.TargetSceneEnterParameters.SceneName;

                if (targetSceneName == Scenes.MainMenu)
                {
                    _coroutines.StartCoroutine(LoadAndStartMainMenu(gameplayExitParameters.
                        TargetSceneEnterParameters.As<MainMenuEnterParameters>()));
                }
                else if(targetSceneName == Scenes.Gameplay)
                {
                    _coroutines.StartCoroutine(LoadAndStartGameplay(gameplayExitParameters.
                        TargetSceneEnterParameters.As<GameplayEnterParameters>()));
                }
            });
        }

        private IEnumerator LoadScene(string sceneName)
        {
            _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            ReflexSceneManager.PreInstallScene(SceneManager.GetSceneByName(sceneName), builder => builder.AddSingleton("Container"));
            
            while (!_asyncOperation.isDone)
            {
                _uiRoot.ShowLoadingProgress(_asyncOperation.progress);
                
                yield return null;
            }
        }
    }
}