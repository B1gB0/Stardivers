using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class TweenAnimationService : ITweenAnimationService
    {
        private const float MoveDistance = 15f;

        private const float ShowScale = 1f;
        private const float HideScale = 0f;

        private const float DurationShow = 0.4f;
        private const float DurationHide = 0.3f;

        private const float SmallPause = 0.1f;
        private const float BigPause = 0.5f;

        public bool IsInitiated { get; private set; }

        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;

            DOTween.Init(recycleAllByDefault: true, useSafeMode: true, logBehaviour: LogBehaviour.Default);

            IsInitiated = true;

            return UniTask.CompletedTask;
        }

        public void AnimateScale(Transform target, bool isDisableTarget = false)
        {
            if (!IsTargetValid(target))
                return;
            
            if(!isDisableTarget)
                target.gameObject.SetActive(true);

            var scaleSequence = CreateScaleSequence(target, isDisableTarget);

            scaleSequence.OnComplete(() =>
            {
                TryOffGameObject(target, isDisableTarget);
            });
        }

        public async UniTask AnimateScaleAsync(Transform target, bool isDisableTarget = false)
        {
            if (!IsTargetValid(target))
                return;
            
            if(!isDisableTarget)
                target.gameObject.SetActive(true);

            var scaleSequence = CreateScaleSequence(target, isDisableTarget);

            await scaleSequence.AsyncWaitForCompletion();

            TryOffGameObject(target, isDisableTarget);
        }

        public void AnimateMove(
            Transform target,
            Transform showPoint,
            Transform hidePoint,
            bool isDisableTarget = false,
            bool isSetParentToPoint = false)
        {
            target.DOKill(true);

            if (!isDisableTarget)
            {
                target.gameObject.SetActive(true);
                target.localPosition = hidePoint.localPosition;
            }

            Sequence _ = DOTween.Sequence()
                .Append(!isDisableTarget
                    ? target.DOMove(showPoint.position, DurationShow)
                    : target.DOMove(hidePoint.position, DurationHide))
                .SetEase(!isDisableTarget ? Ease.InSine : Ease.OutSine)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    TryOffGameObject(target, isDisableTarget);

                    if (isSetParentToPoint)
                    {
                        target.SetParent(isDisableTarget ? hidePoint : showPoint);
                    }
                });
        }

        public void AnimatePointer(Transform target, Transform pointerPoint)
        {
            target?.DOKill();

            target.position = pointerPoint.position;

            float originalY = target.localPosition.y;
            float topY = originalY + MoveDistance;
            float bottomY = originalY - MoveDistance;

            Sequence _ = DOTween.Sequence()
                .Append(target.DOLocalMoveY(bottomY, DurationHide).SetEase(Ease.Linear))
                .AppendInterval(SmallPause)
                .Append(target.DOLocalMoveY(topY, DurationHide).SetEase(Ease.OutSine))
                .Append(target.DOLocalMoveY(bottomY, DurationHide).SetEase(Ease.OutSine))
                .AppendInterval(SmallPause)
                .Append(target.DOLocalMoveY(topY, DurationShow).SetEase(Ease.OutSine))
                .Append(target.DOLocalMoveY(bottomY, DurationShow).SetEase(Ease.OutSine))
                .Append(target.DOLocalMoveY(originalY, BigPause).SetEase(Ease.Linear))
                .AppendInterval(BigPause)
                .SetLoops(-1, LoopType.Restart);
        }

        private void TryOffGameObject(Transform target, bool isDisableTarget)
        {
            if (isDisableTarget && IsTargetValid(target))
                target.gameObject.SetActive(false);
        }

        private Sequence CreateScaleSequence(Transform target, bool isDisableTarget)
        {
            target.DOKill(true);

            if (!isDisableTarget)
                target.localScale = Vector3.zero;

            Sequence scaleSequence = DOTween.Sequence()
                .Append(!isDisableTarget
                    ? target.DOScale(ShowScale, DurationShow)
                    : target.DOScale(HideScale, DurationHide))
                .SetEase(!isDisableTarget ? Ease.OutBounce : Ease.OutSine)
                .SetUpdate(true);
            return scaleSequence;
        }

        private bool IsTargetValid(Transform target)
        {
            return target != null && target.gameObject != null;
        }
    }
}