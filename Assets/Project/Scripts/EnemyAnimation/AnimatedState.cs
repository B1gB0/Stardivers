using UnityEngine;

namespace Project.Scripts.EnemyAnimation
{
    public abstract class AnimatedState
    {
        protected readonly Animator Animator;
        protected readonly AnimationNamesBase AnimationBase;

        protected AnimatedState(Animator animator, AnimationNamesBase animationBase)
        {
            Animator = animator;
            AnimationBase = animationBase;
        }

        public virtual void Enter() { }

        public virtual void Exit() { }
    }
}