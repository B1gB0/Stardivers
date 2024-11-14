using UnityEngine;

namespace Project.Scripts.EnemyAnimation.States
{
    public class AttackState : AnimatedState
    {
        private const float Duration = 0.1f;
        
        public AttackState(Animator animator, AnimationNamesBase animationNamesBase) : base(animator, animationNamesBase) { }
        
        public override void Enter()
        {
            base.Enter();
            Animator.StopPlayback();
            Animator.CrossFade(AnimationBase.Attack, Duration);
        }

        public override void Exit()
        {
            base.Exit();
            Animator.StopPlayback();
        }
    }
}