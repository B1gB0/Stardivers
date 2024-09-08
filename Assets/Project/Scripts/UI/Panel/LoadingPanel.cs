using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour, IView
{
    private const float Duration = 1f;
    private const float ProgressFactor = 0.9f;
    private const int TextFormat = 100;
    
    private readonly Vector3 _loadingWheelPosition = new (0f, 0f, -360f);

    [SerializeField] private Image _loadingWheel;
    [SerializeField] private TMP_Text _progressText;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void RotateLoadingWheel()
    {
        _loadingWheel.transform.DORotate(_loadingWheelPosition, Duration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart).SetRelative().SetEase(Ease.Linear);
    }

    public void SetProgressText(float progress)
    {
        progress /= ProgressFactor; 
        _progressText.text = $"{progress * TextFormat:0}%";
    }
}
