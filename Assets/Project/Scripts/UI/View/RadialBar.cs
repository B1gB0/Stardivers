using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class RadialBar : MonoBehaviour, IView
    {
        private const float RecoveryRate = 10f;
        private const int DefaultBarValue = 0;
        private const int DefaultBackgroundBarValue = 2;
        
        private readonly int RemovedSegments = Shader.PropertyToID("_RemovedSegments");
        
        [SerializeField] protected TMP_Text text;
        
        [SerializeField] private Material _backgroundBarMaterial;
        [SerializeField] private Material _barMaterial;
        
        private CancellationTokenSource _cancellationTokenSource;

        private void Start()
        {
            _barMaterial.SetFloat(RemovedSegments, DefaultBarValue);
            _backgroundBarMaterial.SetFloat(RemovedSegments, DefaultBackgroundBarValue);
        }

        private void OnDestroy()
        {
            CancelAnimation();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        protected void OnChangeValue(float currentValue, float targetValue, float maxValue)
        {
            CancelAnimation();
            _cancellationTokenSource = new();
            SetValueAsync(currentValue, targetValue, maxValue, _cancellationTokenSource.Token).Forget();
        }

        private async UniTask SetValueAsync(float currentValue, float targetValue, float maxValue,
            CancellationToken token)
        {
            while (Mathf.Abs(currentValue - targetValue) > 0.01f && !token.IsCancellationRequested)
            {
                currentValue =
                    Mathf.MoveTowards(currentValue, targetValue, RecoveryRate * Time.unscaledDeltaTime);
                
                float sliderValue = currentValue / maxValue;
                _barMaterial.SetFloat(RemovedSegments, sliderValue);

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
            
            if (!token.IsCancellationRequested)
            {
                float finalSliderValue = targetValue / maxValue;
                _barMaterial.SetFloat(RemovedSegments, finalSliderValue);
            }
        }
        
        private void CancelAnimation()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }
    }
}