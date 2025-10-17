using DG.Tweening;
using Project.Scripts.Services;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class GoldView : MonoBehaviour, IView
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Transform _showPoint;
        [SerializeField] private Transform _hidePoint;

        private IGoldService _goldService;
        private ITweenAnimationService _tweenAnimationService;

        [Inject]
        private void Construct(IGoldService goldService, ITweenAnimationService tweenAnimationService)
        {
            _goldService = goldService;
            _tweenAnimationService = tweenAnimationService;
            
            _text.text = _goldService.Gold.ToString();
            _goldService.OnValueChanged += SetValue;
        }

        private void OnDestroy()
        {
            _goldService.OnValueChanged -= SetValue;
            transform.DOKill();
        }

        public void GetPoints(Transform showPoint, Transform hidePoint)
        {
            _showPoint = showPoint;
            _hidePoint = hidePoint;
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
        
        private void SetValue(int value)
        {
            _text.text = _goldService.Gold.ToString();
        }
    }
}
