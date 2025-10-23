using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.Services
{
    public interface ITweenAnimationService
    {
        public UniTask Init();
        public UniTask AnimateScaleAsync(Transform target, bool isDisableTarget = false);
        public void AnimateScale(Transform target, bool isDisableTarget = false);
        public void AnimateMove(Transform target, Transform showPoint, Transform hidePoint, bool isDisableTarget = false);
    }
}