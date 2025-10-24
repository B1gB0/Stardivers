using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using UnityEngine;

namespace Project.Scripts.ECS.System
{
    public class FollowSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PatrolComponent, EnemyMovableComponent, FollowPlayerComponent>
            _enemyPatrolFollowFilter;

        private readonly EcsFilter<EnemyMovableComponent, FollowPlayerComponent, EnemyAlienTurretAttackComponent>
            _enemyTurretFollowFilter;

        public void Run()
        {
            foreach (var entity in _enemyPatrolFollowFilter)
            {
                ref var movableComponent = ref _enemyPatrolFollowFilter.Get2(entity);
                ref var followComponent = ref _enemyPatrolFollowFilter.Get3(entity);

                var navMeshAgent = movableComponent.NavMeshAgent;

                if (!navMeshAgent.gameObject.activeSelf) continue;

                if (followComponent.Target == null || !followComponent.Target.CanFollow)
                    continue;

                navMeshAgent.destination = followComponent.Target.transform.position;

                var isMoving = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
                movableComponent.IsMoving = isMoving;

                var direction = (followComponent.Target.transform.position - movableComponent.Transform.position)
                    .normalized;
                
                movableComponent.Transform.forward = isMoving ? navMeshAgent.transform.forward : direction;
            }

            foreach (var entity in _enemyTurretFollowFilter)
            {
                ref var movableComponent = ref _enemyTurretFollowFilter.Get1(entity);
                ref var followComponent = ref _enemyTurretFollowFilter.Get2(entity);
                ref var turretComponent = ref _enemyTurretFollowFilter.Get3(entity);

                if (followComponent.Target == null)
                    continue;
                
                var isMoving = Vector3.Distance(movableComponent.Transform.position, 
                    followComponent.Target.transform.position) < turretComponent.RangeAttack;
                movableComponent.IsMoving = isMoving;
                movableComponent.IsAttack = isMoving;
                
                var direction = (followComponent.Target.transform.position - movableComponent.Transform.position)
                    .normalized;
                movableComponent.Transform.forward = isMoving ? direction : movableComponent.Transform.forward;
            }
        }
    }
}