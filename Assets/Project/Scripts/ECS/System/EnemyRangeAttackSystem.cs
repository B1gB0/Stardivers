using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using Project.Scripts.Projectiles.Bullets;
using UnityEngine;

namespace Project.Scripts.ECS.System
{
    public class EnemyRangeAttackSystem : IEcsRunSystem
    {
        private const string BigEnemyAlienProjectilePool = nameof(BigEnemyAlienProjectilePool);
        
        private const float Delay = 5f;
        private const float MinValue = 0f;
        private const int CountProjectiles = 3;
        private const bool IsAutoExpand = true;
        
        private readonly EcsFilter<EnemyComponent, FollowPlayerComponent, RangeAttackComponent, EnemyMovableComponent,
            AnimatedComponent> _attackFilter;

        private float _lastShotTime = 2f;
        private ObjectPool<BigEnemyAlienProjectile> _projectilePool;

        public void Run()
        {
            foreach (var entity in _attackFilter)
            {
                ref var enemyComponent = ref _attackFilter.Get1(entity);
                ref var followPlayerComponent = ref _attackFilter.Get2(entity);
                ref var attackComponent = ref _attackFilter.Get3(entity);
                ref var movableComponent = ref _attackFilter.Get4(entity);
                ref var animatedComponent = ref _attackFilter.Get5(entity);

                _projectilePool ??= new ObjectPool<BigEnemyAlienProjectile>(attackComponent.Projectile,
                    CountProjectiles, new GameObject(BigEnemyAlienProjectilePool).transform)
                {
                    AutoExpand = IsAutoExpand
                };

                if (!movableComponent.IsMoving && followPlayerComponent.Target.Health.TargetHealth > MinValue && 
                    enemyComponent.Health.TargetHealth > MinValue)
                {
                    animatedComponent.IsAttacking = true;
                    
                    if (_lastShotTime <= MinValue)
                    {
                        BigEnemyAlienProjectile projectile = _projectilePool.GetFreeElement();

                        projectile.transform.position = movableComponent.Transform.position;
                        
                        projectile.SetDirection(followPlayerComponent.Target.transform);
                        projectile.SetCharacteristics(attackComponent.Damage, attackComponent.ProjectileSpeed);

                        _lastShotTime = Delay;
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