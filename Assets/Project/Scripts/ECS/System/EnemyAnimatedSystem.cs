using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using UnityEngine;

namespace Project.Scripts.ECS.System
{
    public class EnemyAnimatedSystem : IEcsRunSystem
    {
        public readonly int Move = Animator.StringToHash(nameof(Move));
        public readonly int Attack = Animator.StringToHash(nameof(Attack));

        private readonly EcsFilter<AnimatedComponent, EnemyMovableComponent, EnemyComponent>
            _animatedFilter;
        
        public void Run()
        {
            foreach (var entity in _animatedFilter)
            {
                ref var animatedComponent = ref _animatedFilter.Get1(entity);
                ref var movableComponent = ref _animatedFilter.Get2(entity);

                animatedComponent.Animator.SetBool(Attack, animatedComponent.IsAttacking);
                animatedComponent.Animator.SetBool(Move, movableComponent.IsMoving);
            }
        }
    }
}