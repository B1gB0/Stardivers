using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Services
{
    public interface ITweenAnimationService : IService
    {
        public UniTask AnimateScaleAsync(Transform target, bool isDisableTarget = false);
        public void AnimateScale(Transform target, bool isDisableTarget = false);
        public void AnimateFade(Image target, bool isDisableTarget = false);
        public void AnimateMove(Transform target, Transform showPoint, Transform hidePoint,
            bool isDisableTarget = false);
        public void AnimatePointer(Transform target, Transform topPoint, Transform bottomPoint);
    }
}