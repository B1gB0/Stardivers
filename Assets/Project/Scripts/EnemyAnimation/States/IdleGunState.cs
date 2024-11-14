using UnityEngine;

namespace Project.Scripts.EnemyAnimation.States
{
    public class IdleGunState : AnimatedState
    {
        private const float Duration = 0.1f;
        
        private readonly AnimatedStateMachine _animatedStateMachine;

        public IdleGunState(Animator animator, AnimationNamesBase animationNamesBase,
            AnimatedStateMachine animatedStateMachine) : base(animator, animationNamesBase)
        {
            _animatedStateMachine = animatedStateMachine;
        }
        
        public override void Enter()
        {
            _animatedStateMachine.EnterIn<GetGunState>();
            
            base.Enter();
            
            Animator.CrossFade(AnimationBase.IdleGun, Duration);
        }

        public override void Exit()
        {
            base.Exit();
            Animator.StopPlayback();
        }
    }
}