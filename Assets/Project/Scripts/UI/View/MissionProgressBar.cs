using System.Threading;
using Cysharp.Threading.Tasks;
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
        private const float MinValue = 0f;
        private const float MaxValue = 1f;
        
        [SerializeField] private Slider _smoothSlider;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Transform _showPoint;
        [SerializeField] private Transform _hidePoint;
        [SerializeField] private float _animationDuration = 0.5f;
    
        private float _currentDisplayValue;
        private CancellationTokenSource _animationCancellation;

        private ITweenAnimationService _tweenAnimationService;
        private ILevelTextService _levelTextService;
        private LevelTextData _levelTextData;

        [Inject]
        private void Construct(ITweenAnimationService tweenAnimationService, ILevelTextService levelTextService)
        {
            _tweenAnimationService = tweenAnimationService;
            _levelTextService = levelTextService;
        }
        
        private void OnDestroy()
        {
            transform.DOKill();
        }
        
        public void OnChangedValues(float currentProgress, float maxProgress)
        {
            SetValue(currentProgress, maxProgress);
        }

        public void OnChangeValuesSmoothly(float currentProgress, float maxProgress)
        {
            float targetValue = currentProgress / maxProgress;
            
            _animationCancellation?.Cancel();
            _animationCancellation = new CancellationTokenSource();
            
            AnimateProgressAsync(targetValue, _animationCancellation.Token).Forget();
        }
        
        private async UniTaskVoid AnimateProgressAsync(float targetValue, CancellationToken cancellationToken)
        {
            float startValue = _currentDisplayValue;
            float elapsedTime = MinValue;
        
            while (elapsedTime < _animationDuration && !cancellationToken.IsCancellationRequested)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / _animationDuration;
                
                _currentDisplayValue = Mathf.SmoothStep(startValue, targetValue, progress);
            
                SetDisplayValue(_currentDisplayValue);
            
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        
            if (!cancellationToken.IsCancellationRequested)
            {
                _currentDisplayValue = targetValue;
                SetDisplayValue(_currentDisplayValue);
            }
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

        private void SetDisplayValue(float currentProgress)
        {
            SetValue(currentProgress, MaxValue);
        }
        
        private void SetValue(float currentValue, float maxValue)
        {
            _smoothSlider.value = currentValue / maxValue;
        }
    }
}