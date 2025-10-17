using Project.Scripts.UI.View;
using TMPro;
using UnityEngine;

public class LoadingPanel : MonoBehaviour, IView
{
    private const int TextFormat = 100;
    private const float StartProgress = 0f;
    
    [SerializeField] private TMP_Text _progressText;

    public void Show()
    {
        gameObject.SetActive(true);
        _progressText.text = $"{StartProgress * TextFormat:0}%";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetProgressText(float progress)
    {
        _progressText.text = $"{progress * TextFormat:0}%";
    }
}
