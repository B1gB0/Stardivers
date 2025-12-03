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
using Joystick = Project.Scripts.UI.View.Joystick;

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

        private void Awake()
        {
// #if UNITY_EDITOR
            CheatsButton.gameObject.SetActive(true);
// #endif
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

        public async UniTaskVoid ShowTutorialPointer()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DelayToShowTutorial), DelayType.DeltaTime);

            TutorialPointer.Show();
            TutorialPointer.transform.position = PointerPoint.transform.position;
            _tweenAnimationService.AnimatePointer(TutorialPointer.transform, TopPointerPoint,
                BottomPointerPoint);
        }
        
        public async UniTaskVoid ShowTutorialKeyboardView()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DelayToShowTutorial), DelayType.DeltaTime);

            KeyboardTutorialView.Show();
            _tweenAnimationService.AnimateMove(KeyboardTutorialView.transform, ShowKeyboardTutorialPoint,
                HideKeyboardTutorialPoint);
        }

        public void HideTutorialPointer()
        {
            TutorialPointer.Hide();
        }
        
        public void HideTutorialKeyboardView()
        {
            KeyboardTutorialView.Hide();
        }

        // public void ResetCountdownTutorialPointer()
        // {
        //     TutorialPointer.Hide();
        //     CountdownToShowStoryButtonFoot().Forget();
        // }

        private void OnDestroy()
        {
            MinesButton.transform.DOKill();
        }

        // private async UniTaskVoid CountdownToShowStoryButtonFoot()
        // {
        //     _tutorialCancellationToken?.Cancel();
        //     _tutorialCancellationToken?.Dispose();
        //     _tutorialCancellationToken = new CancellationTokenSource();
        //
        //     try
        //     {
        //         // Ожидаем 4 секунды или любое нажатие
        //         var completedTask = await UniTask.WhenAny(
        //             UniTask.Delay(TimeSpan.FromSeconds(6), DelayType.DeltaTime,
        //                 cancellationToken: _tutorialCancellationToken.Token),
        //             WaitForAnyInput(_tutorialCancellationToken.Token)
        //         );
        //
        //         // Если индекс 0 - значит сработала задержка (не было ввода)
        //         if (completedTask == 0)
        //         {
        //             TutorialPointer.Show();
        //             TutorialPointer.transform.position = PointerPoint.transform.position;
        //             _tweenAnimationService.AnimatePointer(TutorialPointer.transform, TopPointerPoint,
        //                 BottomPointerPoint);
        //         }
        //     }
        //     catch (OperationCanceledException) { }
        // }
        //
        // private async UniTask WaitForAnyInput(CancellationToken ct)
        // {
        //     await UniTask.WaitUntil(() => 
        //             Input.anyKeyDown ||
        //             (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began),
        //         cancellationToken: ct
        //     );
        // }
    }
}