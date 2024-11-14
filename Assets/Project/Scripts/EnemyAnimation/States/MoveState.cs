using UnityEngine;

namespace Project.Scripts.EnemyAnimation.States
{
    public class MoveState : AnimatedState
    {
        private const float Duration = 0.1f;
        
        public MoveState(Animator animator, AnimationNamesBase animationNamesBase) : base(animator, animationNamesBase) { }
        
        public override void Enter()
        {
            base.Enter();
            Animator.StopPlayback();
            Animator.CrossFade(AnimationBase.Move, Duration);
        }

        public override void Exit()
        {
            base.Exit();
            Animator.StopPlayback();
        }
    }
}