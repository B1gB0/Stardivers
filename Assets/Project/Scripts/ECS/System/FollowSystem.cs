using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Build.Game.Scripts.ECS.System
{
    public class FollowSystem : IEcsRunSystem
    {
        private readonly EcsFilter<FollowPlayerComponent, EnemyMovableComponent> _enemyFollowFilter;
        
        public void Run()
        {
            foreach (var entity in _enemyFollowFilter)
            {
                ref var followComponent = ref _enemyFollowFilter.Get1(entity);
                ref var movableComponent = ref _enemyFollowFilter.Get2(entity);

                if (followComponent.Target == null)
                    continue;

                var navMashAgent = followComponent.NavMeshAgent;

                if (navMashAgent.gameObject.activeSelf)
                {
                    navMashAgent.destination = followComponent.Target.transform.position;
                    var isMoving = navMashAgent.remainingDistance > navMashAgent.stoppingDistance;
                    movableComponent.IsMoving = isMoving;
                }
                
                movableComponent.Transform.forward = navMashAgent.transform.forward;
            }
        }
    }
}