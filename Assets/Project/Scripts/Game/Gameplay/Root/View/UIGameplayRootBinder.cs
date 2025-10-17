using Project.Game.Scripts;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.StateMachine;
using Project.Scripts.UI.StateMachine.States;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Game.Gameplay.Root.View
{
    public class UIGameplayRootBinder : MonoBehaviour
    {
        [field: SerializeField] public GameplayElements UIScene { get; private set; }
        [field: SerializeField] public Button MinesButton { get; private set; }
        
#if UNITY_EDITOR
        [field: SerializeField] public Button CheatsButton { get; private set; }
#endif
        
        [field: SerializeField] public WeaponPanel WeaponPanel { get; private set; }

        [field: SerializeField] public Transform ShowGoldPoint { get; private set; }
        [field: SerializeField] public Transform HideGoldPoint { get; private set; }
        [field: SerializeField] public Transform ShowHealthPoint { get; private set; }
        [field: SerializeField] public Transform HideHealthPoint { get; private set; }
        [field: SerializeField] public Transform ShowMissionProgressPoint { get; private set; }
        [field: SerializeField] public Transform HideMissionProgressPoint { get; private set; }

        private AudioSoundsService _audioSoundsService;
        private IPauseService _pauseService;

        private Subject<Unit> _exitSceneSignalSubject;
        private UIStateMachine _uiStateMachine;

        [Inject]
        public void Construct(AudioSoundsService audioSoundsService, IPauseService pauseService)
        {
            _audioSoundsService = audioSoundsService;
        }

        private void Awake()
        {
#if UNITY_EDITOR
            CheatsButton.gameObject.SetActive(true);
#endif
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
        }

        public void HideMinesButton()
        {
            MinesButton.gameObject.SetActive(false);
        }

        public void HandleGoToNextSceneButtonClick()
        {
            _audioSoundsService.PlaySound(Sounds.Button);
            _exitSceneSignalSubject?.OnNext(Unit.Default);
        }
    }
}