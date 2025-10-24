using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using UnityEngine;

namespace Project.Scripts.ECS.System
{
    public class EnemyAnimatedSystem : IEcsRunSystem
    {
        public readonly int Move = Animator.StringToHash(nameof(Move));
        public readonly int Attack = Animator.StringToHash(nameof(Attack));

        private readonly EcsFilter<AnimatedComponent, EnemyMovableComponent, PatrolComponent> _animatedNavMeshFilter;
        private readonly EcsFilter<AnimatedComponent, EnemyAlienTurretAttackComponent> _animatedTurretFilter;
        
        public void Run()
        {
            foreach (var entity in _animatedNavMeshFilter)
            {
                ref var animatedComponent = ref _animatedNavMeshFilter.Get1(entity);
                ref var movableComponent = ref _animatedNavMeshFilter.Get2(entity);

                animatedComponent.Animator.SetBool(Attack, animatedComponent.IsAttacking);
                animatedComponent.Animator.SetBool(Move, movableComponent.IsMoving);
            }
            
            foreach (var entity in _animatedTurretFilter)
            {
                ref var animatedComponent = ref _animatedTurretFilter.Get1(entity);

                animatedComponent.Animator.SetBool(Attack, animatedComponent.IsAttacking);
            }
        }
    }
}