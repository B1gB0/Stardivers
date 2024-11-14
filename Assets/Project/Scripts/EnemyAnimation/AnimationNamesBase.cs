using UnityEngine;

namespace Project.Scripts.EnemyAnimation
{
    public class AnimationNamesBase
    {
        public readonly int Idle = Animator.StringToHash(nameof(Idle));
        public readonly int IdleGun = Animator.StringToHash(nameof(IdleGun));
        public readonly int Move = Animator.StringToHash(nameof(Move));
        public readonly int Attack = Animator.StringToHash(nameof(Attack));
        public readonly int GetGun = Animator.StringToHash(nameof(GetGun));
    }
}