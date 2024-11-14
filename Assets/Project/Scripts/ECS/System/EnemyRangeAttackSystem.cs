using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using Project.Scripts.EnemyAnimation.States;
using UnityEngine;

namespace Project.Scripts.ECS.System
{
    public class EnemyRangeAttackSystem : IEcsRunSystem
    {
        private const float FireRate = 4f;
        private const float MinValue = 0f;
        private const float Delay = 2f;

        private readonly EcsFilter<EnemyComponent, FollowPlayerComponent, EnemyMovableComponent, AnimatedComponent,
            EnemyGunnerAlienAttackComponent> _rangeGunnerAlienEnemyAttackFilter;
        
        private readonly EcsFilter<EnemyComponent, FollowPlayerComponent, EnemyMovableComponent, AnimatedComponent,
            EnemyBigAlienAttackComponent> _rangeBigAlienEnemyAttackFilter;

        private float _lastShotTime = 2f;

        public void Run()
        {
            foreach (var entity in _rangeGunnerAlienEnemyAttackFilter)
            {
                ref var enemyComponent = ref _rangeGunnerAlienEnemyAttackFilter.Get1(entity);
                ref var followPlayerComponent = ref _rangeGunnerAlienEnemyAttackFilter.Get2(entity);
                ref var movableComponent = ref _rangeGunnerAlienEnemyAttackFilter.Get3(entity);
                ref var animatedComponent = ref _rangeGunnerAlienEnemyAttackFilter.Get4(entity);
                ref var attackComponent = ref _rangeGunnerAlienEnemyAttackFilter.Get5(entity);

                if (!movableComponent.IsMoving && followPlayerComponent.Target.Health.TargetHealth > MinValue && 
                    enemyComponent.Health.TargetHealth > MinValue)
                {
                    if (_lastShotTime <= MinValue)
                    {
                        animatedComponent.AnimatedStateMachine.EnterIn<AttackState>();

                        _lastShotTime = FireRate;
                    }
                    else if(_lastShotTime <= FireRate)
                    {
                        animatedComponent.AnimatedStateMachine.EnterIn<IdleState>();
                    }

                    _lastShotTime -= Time.fixedDeltaTime;
                }
                else
                {
                    animatedComponent.AnimatedStateMachine.EnterIn<MoveState>();
                }
            }
            
            foreach (var entity in _rangeBigAlienEnemyAttackFilter)
            {
                ref var enemyComponent = ref _rangeBigAlienEnemyAttackFilter.Get1(entity);
                ref var followPlayerComponent = ref _rangeBigAlienEnemyAttackFilter.Get2(entity);
                ref var movableComponent = ref _rangeBigAlienEnemyAttackFilter.Get3(entity);
                ref var animatedComponent = ref _rangeBigAlienEnemyAttackFilter.Get4(entity);
                ref var attackComponent = ref _rangeBigAlienEnemyAttackFilter.Get5(entity);

                if (!movableComponent.IsMoving && followPlayerComponent.Target.Health.TargetHealth > MinValue && 
                    enemyComponent.Health.TargetHealth > MinValue)
                {
                    if (_lastShotTime <= MinValue)
                    {
                        animatedComponent.AnimatedStateMachine.EnterIn<AttackState>();

                        _lastShotTime = FireRate;
                    }
                    else if(_lastShotTime <= FireRate)
                    {
                        animatedComponent.AnimatedStateMachine.EnterIn<IdleState>();
                    }

                    _lastShotTime -= Time.fixedDeltaTime;
                }
                else
                {
                    animatedComponent.AnimatedStateMachine.EnterIn<MoveState>();
                }
            }
        }
    }
}