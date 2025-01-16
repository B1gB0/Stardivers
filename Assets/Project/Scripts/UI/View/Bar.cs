using Project.Scripts.UI.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Bar : MonoBehaviour, IView
{
    [SerializeField] protected Slider SmoothSlider;
    [SerializeField] protected Slider Slider;
    [SerializeField] protected TMP_Text Text;
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    protected void SetValues(float currentValue, float maxValue, float targetValue)
    {
        SmoothSlider.value = currentValue / maxValue;
        
        Slider.value = targetValue / maxValue;
        
        Text.text = (int)targetValue + "/" + (int)maxValue;
    }
}
