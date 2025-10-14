using DG.Tweening;
using Project.Scripts.Services;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class MissionProgressBar : MonoBehaviour, IView
    {
        [SerializeField] private Slider _smoothSlider;
        [SerializeField] private TMP_Text _text;
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

        public void SetText(string text)
        {
            _text.text = text;
        }
        
        private void SetValue(float currentValue, float maxValue)
        {
            _smoothSlider.value = currentValue / maxValue;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}