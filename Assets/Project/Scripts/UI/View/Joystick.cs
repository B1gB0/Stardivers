using DG.Tweening;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Reflex.Attributes;
using UnityEngine;

public class Joystick : MonoBehaviour, IView
{
    [SerializeField] private Transform _showPoint;
    [SerializeField] private Transform _hidePoint;

    private ITweenAnimationService _tweenAnimationService;

    [Inject]
    private void Construct(ITweenAnimationService tweenAnimationService)
    {
        _tweenAnimationService = tweenAnimationService;
    }

    private void Start()
    {
        if (!Application.isMobilePlatform)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
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
}
