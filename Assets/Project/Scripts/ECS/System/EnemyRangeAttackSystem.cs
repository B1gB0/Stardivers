using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Project.Scripts.ECS.System
{
    public class EnemyRangeAttackSystem : IEcsRunSystem
    {
        private const float Delay = 2f;
        private const float MinValue = 0f;

        private readonly EcsFilter<EnemyComponent, FollowPlayerComponent, EnemyMovableComponent, AnimatedComponent> _attackFilter;

        private float _lastShotTime = 0.2f;

        public void Run()
        {
            foreach (var entity in _attackFilter)
            {
                ref var enemyComponent = ref _attackFilter.Get1(entity);
                ref var followPlayerComponent = ref _attackFilter.Get2(entity);
                ref var movableComponent = ref _attackFilter.Get3(entity);
                ref var animatedComponent = ref _attackFilter.Get4(entity);

                if (!movableComponent.IsMoving && followPlayerComponent.Target.Health.TargetHealth > MinValue && 
                    enemyComponent.Health.TargetHealth > MinValue)
                {
                    if (_lastShotTime <= MinValue)
                    {
                        animatedComponent.IsAttacking = true;

                        _lastShotTime = Delay;
                    }
                    else if (_lastShotTime <= Delay)
                    {
                        animatedComponent.IsAttacking = false;
                    }

                    _lastShotTime -= Time.fixedDeltaTime;
                }
                else
                {
                    animatedComponent.IsAttacking = false;
                }
            }
        }
    }
}