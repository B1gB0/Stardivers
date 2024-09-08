using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Build.Game.Scripts.ECS.System
{
    public class PlayerMoveSystem : IEcsRunSystem
    {
        private const float MoveDirectionY = 0f;
        private const float MinMagnitude = 0f;

        private readonly EcsFilter<PlayerComponent, MovableComponent, InputEventComponent> _MoveFilter;

        public void Run()
        {
            foreach (var entity in _MoveFilter)
            {
                ref var movableComponent = ref _MoveFilter.Get2(entity);
                ref var rigidbody = ref movableComponent.rigidbody;
                
                ref var inputComponent = ref _MoveFilter.Get3(entity);
                ref var playerInputController = ref inputComponent.PlayerInputController;

                if (movableComponent.isMoving)
                {
                    rigidbody.velocity = new Vector3(playerInputController.MoveDirection.x * movableComponent.moveSpeed,
                        rigidbody.velocity.y, playerInputController.MoveDirection.y * movableComponent.moveSpeed);
                    
                    Vector3 moveDirection = new Vector3(playerInputController.MoveDirection.x, MoveDirectionY,
                        playerInputController.MoveDirection.y);

                    Quaternion toRotation = Quaternion.LookRotation(moveDirection);
                   
                    rigidbody.rotation = Quaternion.RotateTowards(rigidbody.rotation, toRotation, 
                        movableComponent.rotationSpeed * Time.fixedDeltaTime);
                }

                movableComponent.isMoving = playerInputController.MoveDirection.sqrMagnitude > MinMagnitude;
            }
        }
    }
}