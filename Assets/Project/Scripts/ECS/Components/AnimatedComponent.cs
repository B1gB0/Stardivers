using Project.Scripts.EnemyAnimation;
using UnityEngine;

namespace Project.Scripts.ECS.Components
{
    public struct AnimatedComponent
    {
        public Animator Animator;
        public AnimatedStateMachine AnimatedStateMachine;
        public bool IsAttacking;
    }
}