using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class RadialBar : View
    {
        private const float RecoveryRate = 10f;
        private const float ApproximateValue = 0.01f;

        private const int DefaultBarValue = 0;
        private const int DefaultBackgroundBarValue = 2;

        private readonly int _removedSegments = Shader.PropertyToID("_RemovedSegments");

        [SerializeField] protected TMP_Text Text;

        [SerializeField] private Material _backgroundBarMaterial;
        [SerializeField] private Material _barMaterial;

        private CancellationTokenSource _cancellationTokenSource;

        private void Start()
        {
            _barMaterial.SetFloat(_removedSegments, DefaultBarValue);
            _backgroundBarMaterial.SetFloat(_removedSegments, DefaultBackgroundBarValue);
        }

        private void OnDestroy()
        {
            CancelAnimation();
        }

        protected void OnChangeValue(float currentValue, float targetValue, float maxValue)
        {
            CancelAnimation();
            _cancellationTokenSource = new ();
            SetValueAsync(currentValue, targetValue, maxValue, _cancellationTokenSource.Token).Forget();
        }

        private async UniTask SetValueAsync(
            float currentValue,
            float targetValue,
            float maxValue,
            CancellationToken token)
        {
            while (Mathf.Abs(currentValue - targetValue) > ApproximateValue && !token.IsCancellationRequested)
            {
                currentValue =
                    Mathf.MoveTowards(currentValue, targetValue, RecoveryRate * Time.unscaledDeltaTime);

                float sliderValue = currentValue / maxValue;
                _barMaterial.SetFloat(_removedSegments, sliderValue);

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (!token.IsCancellationRequested)
            {
                float finalSliderValue = targetValue / maxValue;
                _barMaterial.SetFloat(_removedSegments, finalSliderValue);
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