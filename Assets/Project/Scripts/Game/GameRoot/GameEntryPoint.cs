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
using YG;

namespace Project.Scripts.Game.GameRoot
{
    public class GameEntryPoint : MonoBehaviour
    {
        private const float TargetValue = 1f;
        private const float MinValue = 0f;
        private const float SpeedLoadingScene = 5f;
        private const float SpeedFinalLoadingScene = 0.5f;
        private const float MinLoadTime = 2.0f;
        private const float ActivationThreshold = 0.9f;
        
        private const int DelayOfTransition = 100;

        private AsyncOperationHandle<SceneInstance> _sceneHandle;
        private bool _isLoadingScene;

        private UIRootView _uiRoot;
        private OperationService _operationService;
        private IPauseService _pauseService;

        [Inject]
        private void Construct(UIRootView uiRoot, OperationService operationService,
            IPauseService pauseService)
        {
            _operationService = operationService;
            _uiRoot = uiRoot;
            _pauseService = pauseService;
        }

        private async void Start()
        {
            await Addressables.InitializeAsync().Task;

            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
            await StartGame();
            
            _pauseService.OnPlayGame();
            
            YG2.onShowWindowGame += _pauseService.OnPlayGame;
            YG2.onHideWindowGame += _pauseService.OnStopGameWithMusic;
            YG2.onOpenAnyAdv += _pauseService.OnStopGameWithMusic;
            YG2.onCloseAnyAdv += _pauseService.OnPlayGameAndResetAllPauses;
            YG2.onCloseRewardedAdv += _pauseService.OnPlayGameAndResetAllPauses;
        }

        private void OnDestroy()
        {
            YG2.onShowWindowGame -= _pauseService.OnPlayGame;
            YG2.onHideWindowGame -= _pauseService.OnStopGameWithMusic;
            YG2.onOpenAnyAdv -= _pauseService.OnStopGameWithMusic;
            YG2.onCloseAnyAdv -= _pauseService.OnPlayGameAndResetAllPauses;
            YG2.onCloseRewardedAdv -= _pauseService.OnPlayGameAndResetAllPauses;
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

            if (sceneName != Scenes.Boot)
            {
                return;
            }
#endif
            _uiRoot.UIStateMachine.EnterIn<LoadingPanelState>();

            await LoadAndStartMainMenu();
        }

        private async UniTask LoadAndStartMainMenu(MainMenuEnterParameters enterParameters = null)
        {
            _uiRoot.UIStateMachine.EnterIn<LoadingPanelState>();

            await LoadScene(Scenes.MainMenu);

            var sceneEntryPoint = FindFirstObjectByType<MainMenuEntryPoint>();
            sceneEntryPoint.Run(_uiRoot, enterParameters).Subscribe(mainMenuExitParameters =>
            {
                if (_operationService.CurrentOperation.Id == Constant.Operations.Mars)
                {
                    mainMenuExitParameters.TargetSceneEnterParameters
                        .SetNewSceneName(_operationService.GetSceneNameByCurrentNumber());
                }
                else if (_operationService.CurrentOperation.Id == Constant.Operations.MysteryPlanet)
                {
                    mainMenuExitParameters.TargetSceneEnterParameters
                        .SetNewSceneName(_operationService.GetSceneNameByCurrentNumber());
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

            var sceneEntryPoint = FindFirstObjectByType<GameplayEntryPoint>();
            var observable = await sceneEntryPoint.Run(_uiRoot, enterParameters);
            
            var exitParameters = await observable.FirstAsync();
            await HandleExitGameplayScene(exitParameters);
        }

        private async UniTask<GameplayExitParameters> HandleExitGameplayScene(
            GameplayExitParameters gameplayExitParameters)
        {
            YG2.InterstitialAdvShow();
            
            var targetSceneName = gameplayExitParameters.TargetSceneEnterParameters.SceneName;

            if (targetSceneName == Scenes.MainMenu)
            {
                await LoadAndStartMainMenu(gameplayExitParameters
                    .TargetSceneEnterParameters.As<MainMenuEnterParameters>()
                );
            }
            else
            {
                await LoadAndStartGameplay(gameplayExitParameters
                    .TargetSceneEnterParameters.As<GameplayEnterParameters>()
                );
            }
            
            return gameplayExitParameters;
        }

        private async UniTask LoadScene(string sceneName)
        {
            if(_isLoadingScene) return;
            _isLoadingScene = true;

            try
            {
                var newSceneHandle = Addressables.LoadSceneAsync(
                    sceneName,
                    LoadSceneMode.Single,
                    false
                );

                await newSceneHandle.Task;

                if (sceneName != Scenes.Boot)
                {
                    await SimulateLoadingProgress(newSceneHandle);
                }

                await UniTask.Yield();

                var activateOp = newSceneHandle.Result.ActivateAsync();
                await activateOp;

                if (_sceneHandle.IsValid())
                {
                    await UniTask.Delay(DelayOfTransition);
                    Addressables.Release(_sceneHandle);
                }

                _sceneHandle = newSceneHandle;

                Scene loadedScene = SceneManager.GetSceneByName(sceneName);
                ReflexSceneManager.PreInstallScene(loadedScene,
                    builder => builder.AddSingleton("Container"));
            }
            finally
            {
                _isLoadingScene = false;
            }
        }
        
        private async UniTask SimulateLoadingProgress(AsyncOperationHandle<SceneInstance> sceneHandle)
        {
            float timer = MinValue;
            float fakeProgress = MinValue;

            while (fakeProgress < ActivationThreshold)
            {
                timer += Time.deltaTime;

                float realProgress = sceneHandle.PercentComplete;

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
        }
    }
}