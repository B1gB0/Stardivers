using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace Build.Game.Scripts.ECS.System
{
    public class EnemyAttackSystem : IEcsRunSystem
    {
        private readonly EcsFilter<EnemyComponent, FollowPlayerComponent, AttackComponent, MovableComponent> _attackFilter;
        private readonly float _delay = 1f;
        private readonly float _minValue = 0f;
        
        private float _lastHitTime = 2f;

        public void Run()
        {
            foreach (var entity in _attackFilter)
            {
                ref var enemyComponent = ref _attackFilter.Get1(entity);
                ref var followPlayerComponent = ref _attackFilter.Get2(entity);
                ref var attackComponent = ref _attackFilter.Get3(entity);
                ref var movableComponent = ref _attackFilter.Get4(entity);

                if (!movableComponent.isMoving && followPlayerComponent.target.Health.Value > _minValue && 
                    enemyComponent.health.Value > _minValue)
                {
                    Debug.Log(movableComponent.isMoving);
                    attackComponent.isAttacking = true;
                    
                    if (_lastHitTime <= _minValue)
                    {
                        followPlayerComponent.target.Health.TakeDamage(attackComponent.damage);

                        _lastHitTime = _delay;
                    }
                    
                    _lastHitTime -= Time.deltaTime;
                }
                else
                {
                    attackComponent.isAttacking = false;
                }
            }
        }
    }
}