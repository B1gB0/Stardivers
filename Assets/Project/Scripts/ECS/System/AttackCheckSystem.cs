using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using UnityEngine.AI;

namespace Project.Scripts.ECS.System
{
    public class AttackCheckSystem : IEcsRunSystem
    {
        private const float MinRemainingDistance = 1f;
        
        private readonly EcsFilter<FollowPlayerComponent, EnemyMovableComponent> _enemyFilter;

        public void Run()
        {
            foreach (var entity in _enemyFilter)
            {
                ref var followComponent = ref _enemyFilter.Get1(entity);
                ref var movableComponent = ref _enemyFilter.Get2(entity);

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
        }
    }
}