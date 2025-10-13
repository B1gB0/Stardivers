using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class TweenAnimationService : Service, ITweenAnimationService
    {
        private const float ShowScale = 1f;
        private const float HideScale = 0f;
        private const float DurationShow = 0.4f;
        private const float DurationHide = 0.3f;

        public override UniTask Init()
        {
            DOTween.Init(recycleAllByDefault: true, useSafeMode: true, logBehaviour: LogBehaviour.Default);
            return UniTask.CompletedTask;
        }

        public void AnimateScale(Transform target, bool isDisableTarget = false)
        {
            target.DOKill(true);

            if (!isDisableTarget)
                target.localScale = Vector3.zero;

            Sequence scaleSequence = DOTween.Sequence()
                .Append(!isDisableTarget
                    ? target.DOScale(ShowScale, DurationShow)
                    : target.DOScale(HideScale, DurationHide))
                .SetEase(!isDisableTarget ? Ease.OutBounce : Ease.OutSine)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    if (isDisableTarget && target != null)
                        target.gameObject.SetActive(false);
                });
        }

        public void AnimateMove(Transform target, Transform targetPoint, float duration, bool isDisableTarget = false)
        {
            target.DOKill(true);

            if (!isDisableTarget)
                target.localScale = Vector3.zero;

            Sequence scaleSequence = DOTween.Sequence()
                .Append(target.DOMove(targetPoint.position, duration))
                .SetEase(!isDisableTarget ? Ease.OutBounce : Ease.OutSine)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    if (isDisableTarget && target != null)
                        target.gameObject.SetActive(false);
                });
        }
    }
}