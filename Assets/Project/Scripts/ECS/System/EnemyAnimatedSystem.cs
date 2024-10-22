using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Build.Game.Scripts.ECS.System
{
    public class EnemyAnimatedSystem : IEcsRunSystem
    {
        public readonly int Idle = Animator.StringToHash(nameof(Idle));
        public readonly int Move = Animator.StringToHash(nameof(Move));
        public readonly int Attack = Animator.StringToHash(nameof(Attack));

        private readonly EcsFilter<AnimatedComponent, MovableComponent, AttackComponent, EnemyComponent> _animatedFilter;
        
        public void Run()
        {
            foreach (var entity in _animatedFilter)
            {
                ref var animatedComponent = ref _animatedFilter.Get1(entity);
                ref var movableComponent = ref _animatedFilter.Get2(entity);
                ref var attackComponent = ref _animatedFilter.Get3(entity);

                if(!attackComponent.isAttacking)
                    animatedComponent.animator.SetBool(Move, movableComponent.isMoving);
                
                if(!movableComponent.isMoving)
                    animatedComponent.animator.SetBool(Attack, attackComponent.isAttacking);
            }
        }
    }
}