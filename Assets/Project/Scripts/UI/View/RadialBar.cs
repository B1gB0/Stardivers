using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts.UI
{
    public class RadialBar : MonoBehaviour, IView
    {
        private const float RecoveryRate = 10f;
        
        private readonly int RemovedSegments = Shader.PropertyToID("_RemovedSegments");
        
        [SerializeField] protected TMP_Text text;
        
        [SerializeField] private Material _backgroundBarMaterial;
        [SerializeField] private Material _barMaterial;
        
        private Coroutine _coroutine;

        public void Show()
        {
            gameObject.SetActive(true);
            _barMaterial.SetFloat(RemovedSegments, 0);
            _backgroundBarMaterial.SetFloat(RemovedSegments, 1);
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