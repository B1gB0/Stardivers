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
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Project.Scripts.Game.GameRoot
{
    public class GameEntryPoint : MonoBehaviour
    {
        private const float TargetValue = 1f;
        private const float SpeedLoadingScene = 5f;
        private const float SpeedFinalLoadingScene = 0.5f;
        private const float MinLoadTime = 2.0f;
        private const float ActivationThreshold = 0.9f;

        private const string Mars = "mars_operation";
        private const string MysteryPlanet = "mystery_planet_operation";

        private AsyncOperationHandle<SceneInstance> _sceneHandle;

        private UIRootView _uiRoot;
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

            // if (sceneName == Scenes.Gameplay)
            // {
            //     var enterParameters = new GameplayEnterParameters(
            //         _operationService.CurrentOperation, 
            //         _operationService.CurrentNumberLevel
            //     );
            //     await LoadAndStartGameplay(enterParameters);
            //     return;
            // }

            if (sceneName != Scenes.Boot)
            {
                return;
            }
#endif
            await LoadAndStartMainMenu();
        }

        private async UniTask LoadAndStartMainMenu(MainMenuEnterParameters enterParameters = null)
        {
            _uiRoot.UIStateMachine.EnterIn<LoadingPanelState>();

            await LoadScene(Scenes.MainMenu);
            await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);

            var sceneEntryPoint = FindFirstObjectByType<MainMenuEntryPoint>();
            sceneEntryPoint.Run(_uiRoot, enterParameters).Subscribe(mainMenuExitParameters =>
            {
                if (_operationService.CurrentOperation.Id == Mars)
                {
                    mainMenuExitParameters.TargetSceneEnterParameters.SetNewSceneName(Scenes.MarsFirstLevel);
                }
                else if(_operationService.CurrentOperation.Id == MysteryPlanet)
                {
                    mainMenuExitParameters.TargetSceneEnterParameters.SetNewSceneName(Scenes.MarsFirstLevel);
                }
                
                LoadAndStartGameplay(mainMenuExitParameters
                    .TargetSceneEnterParameters.As<GameplayEnterParameters>()
                ).Forget();
            });
        }

        private async UniTask LoadAndStartGameplay(GameplayEnterParameters enterParameters)
        {
            _uiRoot.UIStateMachine.EnterIn<LoadingPanelState>();

            await LoadScene(enterParameters.SceneName);
            await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);

            var sceneEntryPoint = FindFirstObjectByType<GameplayEntryPoint>();
            sceneEntryPoint.Run(_uiRoot, enterParameters).Subscribe(gameplayExitParameters =>
            {
                var targetSceneName = gameplayExitParameters.TargetSceneEnterParameters.SceneName;

                if (targetSceneName == Scenes.MainMenu)
                {
                    LoadAndStartMainMenu(gameplayExitParameters
                        .TargetSceneEnterParameters.As<MainMenuEnterParameters>()
                    ).Forget();
                }
                else
                {
                    LoadAndStartGameplay(gameplayExitParameters
                        .TargetSceneEnterParameters.As<GameplayEnterParameters>()
                    ).Forget();
                }
            });
        }

        private async UniTask LoadScene(string sceneName)
        {
            if (_sceneHandle.IsValid())
            {
                Addressables.Release(_sceneHandle);
            }

            _sceneHandle = Addressables.LoadSceneAsync(
                sceneName,
                LoadSceneMode.Single,
                false
            );

            if (sceneName != Scenes.Boot)
            {
                float timer = 0f;
                float fakeProgress = 0f;

                while (fakeProgress < ActivationThreshold)
                {
                    timer += Time.deltaTime;

                    float realProgress = _sceneHandle.PercentComplete;

                    fakeProgress = Mathf.Lerp(fakeProgress, realProgress, Time.deltaTime * SpeedLoadingScene);
                    fakeProgress = Mathf.Clamp01(Mathf.Max(fakeProgress, timer / MinLoadTime));
                    fakeProgress = Mathf.Min(fakeProgress, ActivationThreshold);

                    _uiRoot.ShowLoadingProgress(fakeProgress);
                    await UniTask.Yield(PlayerLoopTiming.Update);
                }

                fakeProgress = ActivationThreshold;

                while (fakeProgress < TargetValue)
                {
                    fakeProgress = Mathf.MoveTowards(fakeProgress, TargetValue,
                        Time.deltaTime * SpeedFinalLoadingScene);

                    _uiRoot.ShowLoadingProgress(fakeProgress);
                    await UniTask.Yield(PlayerLoopTiming.Update);
                }

                _uiRoot.ShowLoadingProgress(TargetValue);
                await UniTask.Yield();
            }

            var activateOp = _sceneHandle.Result.ActivateAsync();

            while (!activateOp.isDone)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            Scene loadedScene = SceneManager.GetSceneByName(sceneName);
            ReflexSceneManager.PreInstallScene(loadedScene,
                builder => builder.AddSingleton("Container"));
        }
    }
}