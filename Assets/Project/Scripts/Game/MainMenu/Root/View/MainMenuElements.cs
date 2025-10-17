using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.Game.MainMenu.Root.View
{
    public class MainMenuElements : MonoBehaviour, IView
    {
        private ITweenAnimationService _tweenAnimationService;
        
        [Inject]
        public void Construct(ITweenAnimationService tweenAnimationService)
        {
            _tweenAnimationService = tweenAnimationService;
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
    }
}