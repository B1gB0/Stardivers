using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.StateMachine;
using Project.Scripts.UI.StateMachine.States;
using Project.Scripts.UI.View;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Joystick = Project.Scripts.UI.View.Joystick;
using Unit = R3.Unit;

namespace Project.Scripts.Game.Gameplay.Root.View
{
    public class UIGameplayRootBinder : MonoBehaviour
    {
        private const int DelayToShowTutorial = 5;

        [field: SerializeField] public GameplayElements UIScene { get; private set; }
        [field: SerializeField] public Button MinesButton { get; private set; }
        [field: SerializeField] public Joystick Joystick { get; private set; }
        [field: SerializeField] public WeaponPanel WeaponPanel { get; private set; }
        [field: SerializeField] public TutorialPointer TutorialPointer { get; private set; }
        [field: SerializeField] public KeyboardTutorialView KeyboardTutorialView { get; private set; }

// #if UNITY_EDITOR
        [field: SerializeField] public Button CheatsButton { get; private set; }
// #endif

        [field: SerializeField] public Transform TopPointerPoint { get; private set; }
        [field: SerializeField] public Transform PointerPoint { get; private set; }
        [field: SerializeField] public Transform BottomPointerPoint { get; private set; }
        [field: SerializeField] public Transform ShowKeyboardTutorialPoint { get; private set; }
        [field: SerializeField] public Transform HideKeyboardTutorialPoint { get; private set; }
        [field: SerializeField] public Transform ShowMinesButtonPoint { get; private set; }
        [field: SerializeField] public Transform HideMinesButtonPoint { get; private set; }
        [field: SerializeField] public Transform ShowGoldPoint { get; private set; }
        [field: SerializeField] public Transform HideGoldPoint { get; private set; }
        [field: SerializeField] public Transform ShowAlienCocoonPoint { get; private set; }
        [field: SerializeField] public Transform HideAlienCocoonPoint { get; private set; }
        [field: SerializeField] public Transform ShowHealthPoint { get; private set; }
        [field: SerializeField] public Transform HideHealthPoint { get; private set; }
        [field: SerializeField] public Transform ShowMissionProgressPoint { get; private set; }
        [field: SerializeField] public Transform HideMissionProgressPoint { get; private set; }
        [field: SerializeField] public Transform ShowTimerPoint { get; private set; }
        [field: SerializeField] public Transform HideTimerPoint { get; private set; }

        private AudioSoundsService _audioSoundsService;
        private ITweenAnimationService _tweenAnimationService;

        private Subject<Unit> _exitSceneSignalSubject;
        private UIStateMachine _uiStateMachine;
        private CancellationTokenSource _tutorialCancellationToken;

        [Inject]
        public void Construct(AudioSoundsService audioSoundsService, ITweenAnimationService tweenAnimationService,
            IPlayerService playerService)
        {
            _audioSoundsService = audioSoundsService;
            _tweenAnimationService = tweenAnimationService;
        }

// #if UNITY_EDITOR
        private void Awake()
        {
            CheatsButton.gameObject.SetActive(true);
        }
// #endif

        private void OnDestroy()
        {
            MinesButton.transform.DOKill();
        }

        public void GetUIStateMachine(UIStateMachine uiStateMachine, UIRootButtons uiRootButtons)
        {
            _uiStateMachine = uiStateMachine;
            _uiStateMachine.RemoveState<GameplayState>();
            _uiStateMachine.AddState(new GameplayState(UIScene, uiRootButtons));
            _uiStateMachine.EnterIn<GameplayState>();
        }

        public void Bind(Subject<Unit> exitSceneSignalSubject)
        {
            _exitSceneSignalSubject = exitSceneSignalSubject;
        }

        public void ShowMinesButton()
        {
            MinesButton.gameObject.SetActive(true);
            _tweenAnimationService.AnimateMove(MinesButton.transform, ShowMinesButtonPoint, HideMinesButtonPoint);
        }

        public void HideMinesButton()
        {
            _tweenAnimationService.AnimateMove(MinesButton.transform, ShowMinesButtonPoint, HideMinesButtonPoint,
                true);
        }

        public void HandleGoToNextSceneButtonClick()
        {
            _audioSoundsService.PlaySound(SoundsType.Button).Forget();
            _exitSceneSignalSubject?.OnNext(Unit.Default);
        }

        private async UniTaskVoid ShowTutorialPointer()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DelayToShowTutorial), DelayType.DeltaTime);

            TutorialPointer.Show();
            TutorialPointer.transform.position = PointerPoint.transform.position;
            TutorialPointer.transform.SetParent(PointerPoint);
            _tweenAnimationService.AnimatePointer(TutorialPointer.transform);
        }
        
        private async UniTaskVoid ShowTutorialKeyboardView()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DelayToShowTutorial), DelayType.DeltaTime);

            KeyboardTutorialView.Show();
            _tweenAnimationService.AnimateMove(KeyboardTutorialView.transform, ShowKeyboardTutorialPoint,
                HideKeyboardTutorialPoint);
        }

        public void ResetCountdownTutorialPointer()
        {
            if (YG2.envir.isDesktop)
            {
                _tweenAnimationService.AnimateMove(KeyboardTutorialView.transform, ShowKeyboardTutorialPoint,
                    HideKeyboardTutorialPoint, true);
            }
            else
                TutorialPointer.Hide();

            CountdownToShowStoryButtonFoot().Forget();
        }

        private async UniTaskVoid CountdownToShowStoryButtonFoot()
        {
            _tutorialCancellationToken?.Cancel();
            _tutorialCancellationToken?.Dispose();
            _tutorialCancellationToken = new CancellationTokenSource();
        
            try
            {
                var completedTask = await UniTask.WhenAny(
                    UniTask.Delay(TimeSpan.FromSeconds(DelayToShowTutorial), DelayType.DeltaTime,
                        cancellationToken: _tutorialCancellationToken.Token));

                if (completedTask == 0)
                {
                    if(YG2.envir.isDesktop)
                        ShowTutorialKeyboardView().Forget();
                    else
                        ShowTutorialPointer().Forget();
                }
            }
            catch (OperationCanceledException) { }
        }
    }
}