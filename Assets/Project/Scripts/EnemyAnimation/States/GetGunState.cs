using UnityEngine;

namespace Project.Scripts.EnemyAnimation.States
{
    public class GetGunState : AnimatedState
    {
        private const float Duration = 0.1f;

        public GetGunState(Animator animator, AnimationNamesBase animationNamesBase) : base(animator, animationNamesBase) { }
        
        public override void Enter()
        {
            base.Enter();
            Animator.StopPlayback();
            Animator.CrossFade(AnimationBase.GetGun, Duration);
        }

        public override void Exit()
        {
            base.Exit();
            Animator.StopPlayback();
        }
    }
}