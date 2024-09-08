using System;
using R3;
using Source.Game.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Build.Game.Scripts.Game.Gameplay.GameplayRoot
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;

        public Observable<MainMenuExitParameters> Run(UIRootView uiRoot, MainMenuEnterParameters enterParameters)
        {
            var uiScene = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);
            
            var exitSignalSubject = new Subject<Unit>();
            uiScene.Bind(exitSignalSubject);

            var saveFileName = "";
            var levelNumber = 1;
            
            var gameplayEnterParameters = new GameplayEnterParameters(saveFileName, levelNumber);
            var mainMenuExitParameters = new MainMenuExitParameters(gameplayEnterParameters);

            var exitToGameplaySceneSignal = exitSignalSubject.Select(_ => mainMenuExitParameters);

            return exitToGameplaySceneSignal;
        }
    }
}