using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class TweenAnimationService : ITweenAnimationService
    {
        private const float ShowScale = 1f;
        private const float HideScale = 0f;
        private const float DurationShow = 0.4f;
        private const float DurationHide = 0.3f;

        public bool IsInitiated { get; private set; }

        public UniTask Init()
        {
            if(IsInitiated)
                return UniTask.CompletedTask;
            
            DOTween.Init(recycleAllByDefault: true, useSafeMode: true, logBehaviour: LogBehaviour.Default);

            IsInitiated = true;
            
            return UniTask.CompletedTask;
        }

        public void AnimateScale(Transform target, bool isDisableTarget = false)
        {
            var scaleSequence = CreateScaleSequence(target, isDisableTarget);

            scaleSequence.OnComplete(() =>
            {
                TryOffGameObject(target, isDisableTarget);
            });
        }

        public async UniTask AnimateScaleAsync(Transform target, bool isDisableTarget = false)
        {
            var scaleSequence = CreateScaleSequence(target, isDisableTarget);

            await scaleSequence.AsyncWaitForCompletion();

            TryOffGameObject(target, isDisableTarget);
        }

        public void AnimateMove(Transform target, Transform showPoint, Transform hidePoint, bool isDisableTarget = false)
        {
            target.DOKill(true);

            if (!isDisableTarget)
            {
                target.localPosition = hidePoint.localPosition;
            }

            Sequence scaleSequence = DOTween.Sequence()
                .Append(!isDisableTarget
                    ? target.DOMove(showPoint.position, DurationShow)
                    : target.DOMove(hidePoint.position, DurationHide))
                .SetEase(!isDisableTarget ? Ease.InSine : Ease.OutSine)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    TryOffGameObject(target, isDisableTarget);
                });
        }
        
        private void TryOffGameObject(Transform target, bool isDisableTarget)
        {
            if (isDisableTarget && target != null)
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
    }
}