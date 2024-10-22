using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Build.Game.Scripts.ECS.System
{
    public class FollowSystem : IEcsRunSystem
    {
        private readonly EcsFilter<FollowPlayerComponent, MovableComponent> _enemyFollowFilter;
        
        public void Run()
        {
            foreach (var entity in _enemyFollowFilter)
            {
                ref var followComponent = ref _enemyFollowFilter.Get1(entity);
                ref var movableComponent = ref _enemyFollowFilter.Get2(entity);

                if (followComponent.target == null)
                    continue;

                var navMashAgent = followComponent.navMeshAgent;
                var direction = (followComponent.target.transform.position - movableComponent.transform.position).normalized;

                if (navMashAgent.gameObject.activeSelf)
                {
                    navMashAgent.destination = followComponent.target.transform.position;
                    var isMoving = navMashAgent.remainingDistance > navMashAgent.stoppingDistance;
                    movableComponent.isMoving = isMoving;
                }
                
                movableComponent.transform.forward = navMashAgent.transform.forward;
            }
        }
    }
}