using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Panel
{
    public class ExitPanel : MonoBehaviour, IView
    {
        [SerializeField] private Button _noButton;
        [SerializeField] private Button _yesButton;

        private IPauseService _pauseService;
        private ITweenAnimationService _tweenAnimationService;
        private AudioSoundsService _audioSoundsService;

        private bool _isExitToMainMenu;

        public event Action OnExitToMainMenu;
        public event Action OnBackToSceneButtonPressed;

        [Inject]
        public void Construct(
            IPauseService pauseService,
            ITweenAnimationService tweenAnimationService,
            AudioSoundsService audioSoundsService)
        {
            _tweenAnimationService = tweenAnimationService;
            _pauseService = pauseService;
            _audioSoundsService = audioSoundsService;
        }

        private void Start()
        {
            _noButton.onClick.AddListener(MoveBackToScene);
            _yesButton.onClick.AddListener(OnYesButtonClicked);
        }

        private void OnDestroy()
        {
            transform.DOKill();

            _noButton.onClick.RemoveListener(MoveBackToScene);
            _yesButton.onClick.RemoveListener(OnYesButtonClicked);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _tweenAnimationService.AnimateScale(transform);
        }

        public void Hide()
        {
            if (_isExitToMainMenu)
                gameObject.SetActive(false);
            else
                _tweenAnimationService.AnimateScale(transform, true);
        }

        private void MoveBackToScene()
        {
            _audioSoundsService.PlaySound(SoundsType.Button).Forget();
            _isExitToMainMenu = false;
            OnBackToSceneButtonPressed?.Invoke();
        }

        private void OnYesButtonClicked()
        {
            _audioSoundsService.StopAllSounds();
            _audioSoundsService.PlaySound(SoundsType.Button).Forget();

            _isExitToMainMenu = true;
            _pauseService.OnPlayGame();
            OnExitToMainMenu?.Invoke();
        }
    }
}