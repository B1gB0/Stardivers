using DG.Tweening;
using Project.Scripts.Services;
using Reflex.Attributes;

namespace Project.Scripts.Game.MainMenu.Root.View
{
    public class MainMenuElements : UI.View.View
    {
        private ITweenAnimationService _tweenAnimationService;

        [Inject]
        public void Construct(ITweenAnimationService tweenAnimationService)
        {
            _tweenAnimationService = tweenAnimationService;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        public override void Show()
        {
            _tweenAnimationService.AnimateScale(transform);
        }

        public override void Hide()
        {
            _tweenAnimationService.AnimateScale(transform, true);
        }
    }
}