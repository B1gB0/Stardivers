using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Build.Game.Scripts.ECS.System
{
    public class FollowSystem : IEcsRunSystem
    {
        private readonly EcsFilter<FollowPlayerComponent, MovableComponent> _enemyFollowFilter;
        private readonly float _stopDistance = 2.5f;
        
        public void Run()
        {
            foreach (var entity in _enemyFollowFilter)
            {
                ref var followComponent = ref _enemyFollowFilter.Get1(entity);
                ref var movableComponent = ref _enemyFollowFilter.Get2(entity);

                if (followComponent.target == null)
                    continue;

                var direction = (followComponent.target.transform.position - movableComponent.transform.position).normalized;
                var distance = Vector3.Distance(followComponent.target.transform.position, movableComponent.transform.position);
                var isMoving = distance > Mathf.Round(_stopDistance);
                movableComponent.isMoving = isMoving;

                if (isMoving)
                {
                    movableComponent.rigidbody.velocity = direction * movableComponent.moveSpeed;
                }

                direction.y = 0;
                movableComponent.transform.forward = direction;
            }
        }
    }
}