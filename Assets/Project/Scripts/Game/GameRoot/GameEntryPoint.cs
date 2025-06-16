using System;
using Project.Scripts.Game.Gameplay.Root;
using Project.Scripts.Game.MainMenu.Root;
using Project.Scripts.Services;
using Project.Scripts.UI.StateMachine.States;
using R3;
using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;

namespace Project.Scripts.Game.GameRoot
{
    public class GameEntryPoint : MonoBehaviour
    {
        private const float TargetLoadingValue = 0.9f;
        
        private UIRootView _uiRoot;
        private AsyncOperation _asyncOperation;
        private OperationService _operationService;

        [Inject]
        private void Construct(UIRootView uiRoot, OperationService operationService)
        {
            _operationService = operationService;
            _uiRoot = uiRoot;
        }

        private async void Start()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
            await StartGame();
        }

        private async UniTask StartGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.MainMenu)
            {
                await LoadAndStartMainMenu();
                return;
            }

            if (sceneName == Scenes.Gameplay)
            {
                var enterParameters = new GameplayEnterParameters(
                    _operationService.CurrentOperation, 
                    _operationService.CurrentNumberLevel
                );
                await LoadAndStartGameplay(enterParameters);
                return;
            }

            if (sceneName != Scenes.Boot)
            {
                return;
            }
#endif
            await LoadAndStartMainMenu();
        }

        private async UniTask LoadAndStartMainMenu(MainMenuEnterParameters enterParameters = null)
        {
            await LoadScene(Scenes.Boot);
            
            _uiRoot.UIStateMachine.EnterIn<LoadingPanelState>();
            
            await LoadScene(Scenes.MainMenu);
            await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);

            var sceneEntryPoint = UnityEngine.Object.FindFirstObjectByType<MainMenuEntryPoint>();
            sceneEntryPoint.Run(_uiRoot, enterParameters).Subscribe(mainMenuExitParameters =>
            {
                var targetSceneName = mainMenuExitParameters.TargetSceneEnterParameters.SceneName;

                if (targetSceneName == Scenes.Gameplay)
                {
                    LoadAndStartGameplay(mainMenuExitParameters
                        .TargetSceneEnterParameters.As<GameplayEnterParameters>()
                    ).Forget();
                }
            });
        }
        
        private async UniTask LoadAndStartGameplay(GameplayEnterParameters enterParameters)
        {
            await LoadScene(Scenes.Boot);
            _uiRoot.UIStateMachine.EnterIn<LoadingPanelState>();
            
            await LoadScene(Scenes.Gameplay);
            await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);

            var sceneEntryPoint = UnityEngine.Object.FindFirstObjectByType<GameplayEntryPoint>();
            sceneEntryPoint.Run(_uiRoot, enterParameters).Subscribe(gameplayExitParameters =>
            {
                var targetSceneName = gameplayExitParameters.TargetSceneEnterParameters.SceneName;

                if (targetSceneName == Scenes.MainMenu)
                {
                    LoadAndStartMainMenu(gameplayExitParameters
                        .TargetSceneEnterParameters.As<MainMenuEnterParameters>()
                    ).Forget();
                }
                else if(targetSceneName == Scenes.Gameplay)
                {
                    LoadAndStartGameplay(gameplayExitParameters
                        .TargetSceneEnterParameters.As<GameplayEnterParameters>()
                    ).Forget();
                }
            });
        }

        private async UniTask LoadScene(string sceneName)
        {
            _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            _asyncOperation.allowSceneActivation = false;
            
            ReflexSceneManager.PreInstallScene(SceneManager.GetSceneByName(sceneName), 
                builder => builder.AddSingleton("Container"));

            while (_asyncOperation.progress < TargetLoadingValue)
            {
                _uiRoot.ShowLoadingProgress(_asyncOperation.progress);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            _asyncOperation.allowSceneActivation = true;
        }
    }
}