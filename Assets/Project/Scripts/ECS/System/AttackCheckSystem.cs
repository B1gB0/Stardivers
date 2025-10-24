using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.System
{
    public class AttackCheckSystem : IEcsRunSystem
    {
        private const float MinRemainingDistance = 1f;

        private readonly EcsFilter<PatrolComponent, EnemyMovableComponent, FollowPlayerComponent> _enemyNavMeshFilter;

        private readonly EcsFilter<EnemyMovableComponent, FollowPlayerComponent, EnemyAlienTurretAttackComponent>
            _enemyTurretFilter;

        public void Run()
        {
            foreach (var entity in _enemyNavMeshFilter)
            {
                ref var movableComponent = ref _enemyNavMeshFilter.Get2(entity);
                ref var followComponent = ref _enemyNavMeshFilter.Get3(entity);

                NavMeshAgent navMeshAgent = movableComponent.NavMeshAgent;

                if (followComponent.Target == null || !navMeshAgent.isActiveAndEnabled
                                                   || !followComponent.Target.CanFollow
                                                   || navMeshAgent.remainingDistance <= MinRemainingDistance)
                {
                    movableComponent.IsAttack = false;
                    continue;
                }

                movableComponent.IsAttack = navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
            }

            foreach (var entity in _enemyTurretFilter)
            {
                ref var movableComponent = ref _enemyTurretFilter.Get1(entity);
                ref var followComponent = ref _enemyTurretFilter.Get2(entity);
                ref var turretComponent = ref _enemyTurretFilter.Get3(entity);

                var isInRangeAttack = Vector3.Distance(movableComponent.Transform.position,
                    followComponent.Target.transform.position) <= turretComponent.RangeAttack;

                if (followComponent.Target == null || !isInRangeAttack)
                {
                    movableComponent.IsAttack = false;
                    continue;
                }

                movableComponent.IsAttack = true;
            }
        }
    }
}