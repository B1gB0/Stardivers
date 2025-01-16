using Project.Scripts.UI.View;
using TMPro;
using UnityEngine;

public class LoadingPanel : MonoBehaviour, IView
{
    private const float ProgressFactor = 0.9f;
    private const int TextFormat = 100;
    
    [SerializeField] private TMP_Text _progressText;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetProgressText(float progress)
    {
        progress /= ProgressFactor; 
        _progressText.text = $"{progress * TextFormat:0}%";
    }
}
