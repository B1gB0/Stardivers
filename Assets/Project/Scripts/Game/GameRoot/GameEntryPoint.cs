using System.Collections;
using Build.Game.Scripts.Game.Gameplay;
using Build.Game.Scripts.Game.Gameplay.GameplayRoot;
using Project.Scripts.UI.StateMachine.States;
using R3;
using Reflex.Core;
using Source.Game.Scripts;
using Source.Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Build.Game.Scripts.Game.GameRoot
{
    public class GameEntryPoint
    {
        private const string UIRootViewPath = "UIRoot";
        private const string CoroutinesName = "[Coroutines]";

        private readonly Coroutines _coroutines;
        private readonly UIRootView _uiRoot;

        private static GameEntryPoint _instance;
        
        private AsyncOperation _asyncOperation;

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
                var operation = "Mars";
                var enterParameters = new GameplayEnterParameters("", operation);
                
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
            _uiRoot.UIStateMachine.EnterIn<LoadingPanelState>();
            
            Debug.Log(_uiRoot.UIStateMachine != null);
            
            yield return LoadScene(Scenes.Boot);
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
            _uiRoot.UIStateMachine.EnterIn<LoadingPanelState>();

            yield return LoadScene(Scenes.Boot);
            yield return LoadScene(Scenes.Gameplay);
            
            yield return new WaitForSeconds(1);

            var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
            sceneEntryPoint.Run(_uiRoot, enterParameters).Subscribe(gameplayExitParameters =>
            {
                _coroutines.StartCoroutine(LoadAndStartMainMenu(gameplayExitParameters.MainMenuEnterParameters));
            });
        }

        private IEnumerator LoadScene(string sceneName)
        {
            _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            ReflexSceneManager.PreInstallScene(SceneManager.GetSceneByName(sceneName), builder => builder.AddSingleton("Beautifull"));
            
            while (!_asyncOperation.isDone)
            {
                _uiRoot.ShowLoadingProgress(_asyncOperation.progress);
                
                yield return null;
            }
        }
    }
}