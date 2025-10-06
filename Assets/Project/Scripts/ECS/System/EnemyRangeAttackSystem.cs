using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using Project.Scripts.EnemyAnimation.States;
using UnityEngine;

namespace Project.Scripts.ECS.System
{
    public class EnemyRangeAttackSystem : IEcsRunSystem
    {
        private const float FireRate = 2f;
        private const float MinValue = 0f;

        private readonly EcsFilter<EnemyComponent, FollowPlayerComponent, EnemyMovableComponent, AnimatedComponent,
            EnemyGunnerAlienAttackComponent> _gunnerAlienEnemyAttackFilter;
        
        private readonly EcsFilter<EnemyComponent, FollowPlayerComponent, EnemyMovableComponent, AnimatedComponent,
            EnemyBigAlienAttackComponent> _bigAlienEnemyAttackFilter;

        private float _lastShotTime = 1f;

        public void Run()
        {
            foreach (var entity in _gunnerAlienEnemyAttackFilter)
            {
                ref var enemyComponent = ref _gunnerAlienEnemyAttackFilter.Get1(entity);
                ref var followPlayerComponent = ref _gunnerAlienEnemyAttackFilter.Get2(entity);
                ref var movableComponent = ref _gunnerAlienEnemyAttackFilter.Get3(entity);
                ref var animatedComponent = ref _gunnerAlienEnemyAttackFilter.Get4(entity);
                ref var attackComponent = ref _gunnerAlienEnemyAttackFilter.Get5(entity);

                if (movableComponent.IsAttack && followPlayerComponent.Target.Health.TargetHealth > MinValue && 
                    enemyComponent.Health.TargetHealth > MinValue)
                {
                    if (_lastShotTime <= MinValue)
                    {
                        animatedComponent.IsAttacking = true;

                        _lastShotTime = FireRate;
                    }
                    else if(_lastShotTime <= FireRate)
                    {
                        animatedComponent.IsAttacking = false;
                        animatedComponent.AnimatedStateMachine.EnterIn<GetGunState>();
                    }

                    _lastShotTime -= Time.fixedDeltaTime;
                }
                else
                {
                    animatedComponent.AnimatedStateMachine.EnterIn<MoveState>();
                }
            }
            
            foreach (var entity in _bigAlienEnemyAttackFilter)
            {
                ref var enemyComponent = ref _bigAlienEnemyAttackFilter.Get1(entity);
                ref var followPlayerComponent = ref _bigAlienEnemyAttackFilter.Get2(entity);
                ref var movableComponent = ref _bigAlienEnemyAttackFilter.Get3(entity);
                ref var animatedComponent = ref _bigAlienEnemyAttackFilter.Get4(entity);
                ref var attackComponent = ref _bigAlienEnemyAttackFilter.Get5(entity);

                if (movableComponent.IsAttack && followPlayerComponent.Target.Health.TargetHealth > MinValue && 
                    enemyComponent.Health.TargetHealth > MinValue)
                {
                    if (_lastShotTime <= MinValue)
                    {
                        animatedComponent.IsAttacking = true;

                        _lastShotTime = FireRate;
                    }
                    else if(_lastShotTime <= FireRate)
                    {
                        animatedComponent.IsAttacking = false;
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