using Leopotam.Ecs;
using Project.Scripts.ECS.Components;

namespace Project.Scripts.ECS.System
{
    public class FollowSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PatrolComponent, EnemyMovableComponent, FollowPlayerComponent> _enemyFollowFilter;

        public void Run()
        {
            foreach (var entity in _enemyFollowFilter)
            {
                ref var patrolComponent = ref _enemyFollowFilter.Get1(entity);
                ref var movableComponent = ref _enemyFollowFilter.Get2(entity);
                ref var followComponent = ref _enemyFollowFilter.Get3(entity);

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
        }
    }
}