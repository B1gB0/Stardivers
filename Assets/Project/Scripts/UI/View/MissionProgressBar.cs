using DG.Tweening;
using Project.Scripts.Services;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class MissionProgressBar : MonoBehaviour, IView
    {
        [SerializeField] protected Slider SmoothSlider;
        [SerializeField] private Transform _showPoint;
        [SerializeField] private Transform _hidePoint;

        private ITweenAnimationService _tweenAnimationService;

        [Inject]
        private void Construct(ITweenAnimationService tweenAnimationService)
        {
            _tweenAnimationService = tweenAnimationService;
        }
        
        public void OnChangedValues(float currentHealth, float maxHealth)
        {
            SetValue(currentHealth, maxHealth);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _tweenAnimationService.AnimateMove(transform, _showPoint, _hidePoint);
        }

        public void Hide()
        {
            _tweenAnimationService.AnimateMove(transform, _showPoint, _hidePoint, true);
        }

        public void GetPoints(Transform showPoint, Transform hidePoint)
        {
            _showPoint = showPoint;
            _hidePoint = hidePoint;
        }
        
        private void SetValue(float currentValue, float maxValue)
        {
            SmoothSlider.value = currentValue / maxValue;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}