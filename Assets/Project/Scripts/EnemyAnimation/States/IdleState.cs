using UnityEngine;

namespace Project.Scripts.EnemyAnimation.States
{
    public class IdleState : AnimatedState
    {
        private const float Duration = 0.1f;
        
        public IdleState(Animator animator, AnimationNamesBase animationNamesBase) : base(animator, animationNamesBase) { }
        
        public override void Enter()
        {
            base.Enter();
            Animator.StopPlayback();
            Animator.CrossFade(AnimationBase.Idle, Duration);
        }

        public override void Exit()
        {
            base.Exit();
            Animator.StopPlayback();
        }
    }
}