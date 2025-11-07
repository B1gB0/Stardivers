using System;
using DG.Tweening;
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

        public event Action OnExitToMainMenu;
        public event Action OnBackToSceneButtonPressed;
        
        [Inject]
        public void Construct(IPauseService pauseService, ITweenAnimationService tweenAnimationService)
        {
            _tweenAnimationService = tweenAnimationService;
            _pauseService = pauseService;
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
            _tweenAnimationService.AnimateScale(transform, true);
        }
        
        private void MoveBackToScene()
        {
            OnBackToSceneButtonPressed?.Invoke();
        }

        private void OnYesButtonClicked()
        {
            _pauseService.PlayGame();
            OnExitToMainMenu?.Invoke();
        }
    }
}