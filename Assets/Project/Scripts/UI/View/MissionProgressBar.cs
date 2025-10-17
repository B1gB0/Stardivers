using DG.Tweening;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Game.Constant;
using Project.Scripts.Levels;
using Project.Scripts.Services;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.View
{
    public class MissionProgressBar : MonoBehaviour, IView
    {
        [SerializeField] private Slider _smoothSlider;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Transform _showPoint;
        [SerializeField] private Transform _hidePoint;

        private ITweenAnimationService _tweenAnimationService;
        private ILevelTextService _levelTextService;
        private LevelTextData _levelTextData;

        [Inject]
        private void Construct(ITweenAnimationService tweenAnimationService, ILevelTextService levelTextService)
        {
            _tweenAnimationService = tweenAnimationService;
            _levelTextService = levelTextService;
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

        public void SetData()
        {
            _levelTextData = _levelTextService.GetLevelTextData(SceneManager.GetActiveScene().name,
                LevelTextsType.MissionProgressBarText);
            
            SetText();
        }

        public void SetText()
        {
            if(_levelTextData == null)
                return;
            
            _text.text = YG2.lang switch
            {
                LocalizationCode.Ru => _levelTextData.TextRu,
                LocalizationCode.En => _levelTextData.TextEn,
                LocalizationCode.Tr => _levelTextData.TextTr,
                _ => _text.text
            };
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