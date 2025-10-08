using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class MissionProgressBar : MonoBehaviour, IView
    {
        [SerializeField] protected Slider SmoothSlider;

        public void OnChangedValues(float currentHealth, float maxHealth)
        {
            SetValue(currentHealth, maxHealth);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void SetValue(float currentValue, float maxValue)
        {
            SmoothSlider.value = currentValue / maxValue;
        }
    }
}