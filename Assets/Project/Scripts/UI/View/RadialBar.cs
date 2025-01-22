using System.Collections;
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
        
        private Coroutine _coroutine;

        private void Start()
        {
            _barMaterial.SetFloat(RemovedSegments, DefaultBarValue);
            _backgroundBarMaterial.SetFloat(RemovedSegments, DefaultBackgroundBarValue);
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
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(SetValue(currentValue, targetValue, maxValue));
        }
        
        protected void UpdateLevelValue(float value, float maxValue)
        {
            float valueForView = value / maxValue;
            _barMaterial.SetFloat(RemovedSegments, valueForView);
        }

        private IEnumerator SetValue(float currentValue, float targetValue, float maxValue)
        {
            while (currentValue != targetValue)
            {
                currentValue = Mathf.MoveTowards(currentValue, targetValue, RecoveryRate * Time.deltaTime);
                
                float sliderValue = currentValue / maxValue;
                _barMaterial.SetFloat(RemovedSegments, sliderValue);

                yield return null;
            }
        }
    }
}