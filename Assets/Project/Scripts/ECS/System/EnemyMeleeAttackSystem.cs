using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Build.Game.Scripts.ECS.System
{
    public class EnemyMeleeAttackSystem : IEcsRunSystem
    {
        private const float Delay = 1f;
        private const float MinValue = 0f;
        
        private readonly EcsFilter<EnemyComponent, FollowPlayerComponent, MeleeAttackComponent, EnemyMovableComponent,
            AnimatedComponent> _attackFilter;

        private float _lastHitTime = 2f;

        public void Run()
        {
            foreach (var entity in _attackFilter)
            {
                ref var enemyComponent = ref _attackFilter.Get1(entity);
                ref var followPlayerComponent = ref _attackFilter.Get2(entity);
                ref var attackComponent = ref _attackFilter.Get3(entity);
                ref var movableComponent = ref _attackFilter.Get4(entity);
                ref var animatedComponent = ref _attackFilter.Get5(entity);

                if (!movableComponent.IsMoving && followPlayerComponent.Target.Health.TargetHealth > MinValue && 
                    enemyComponent.Health.TargetHealth > MinValue)
                {
                    animatedComponent.IsAttacking = true;
                    
                    if (_lastHitTime <= MinValue)
                    {
                        followPlayerComponent.Target.Health.TakeDamage(attackComponent.Damage);

                        _lastHitTime = Delay;
                    }
                    
                    _lastHitTime -= Time.deltaTime;
                }
                else
                {
                    animatedComponent.IsAttacking = false;
                }
            }
        }
    }
}