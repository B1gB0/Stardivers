using DG.Tweening;
using Project.Scripts.Services;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public abstract class Bar : MonoBehaviour, IView
    {
        [SerializeField] protected Slider SmoothSlider;
        [SerializeField] protected Slider Slider;
    
        [SerializeField] protected TMP_Text Text;
    
        [SerializeField] private Transform _showPoint;
        [SerializeField] private Transform _hidePoint;
        [SerializeField] private Transform _weaponPanelPoint;

        private ITweenAnimationService _tweenAnimationService;
    
        [Inject]
        private void Construct(ITweenAnimationService tweenAnimationService)
        {
            _tweenAnimationService = tweenAnimationService;
        }
    
        private void OnDestroy()
        {
            transform.DOKill();
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

        public void MoveToWeaponPanelPosition()
        {
            _tweenAnimationService.AnimateMove(transform, _weaponPanelPoint, _showPoint);
        }

        public void MoveToShowPosition()
        {
            _tweenAnimationService.AnimateMove(transform, _showPoint, _weaponPanelPoint);
        }
    
        public void GetPoints(Transform showPoint, Transform hidePoint, Transform weaponPanelPoint)
        {
            _showPoint = showPoint;
            _hidePoint = hidePoint;
            _weaponPanelPoint = weaponPanelPoint;
        }
    
        protected void SetValues(float currentValue, float maxValue, float targetValue)
        {
            SmoothSlider.value = currentValue / maxValue;
        
            Slider.value = targetValue / maxValue;
        
            Text.text = (int)targetValue + "/" + (int)maxValue;
        }
    }
}
