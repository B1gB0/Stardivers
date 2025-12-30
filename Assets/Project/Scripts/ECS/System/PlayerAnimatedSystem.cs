using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using UnityEngine;

namespace Project.Scripts.ECS.System
{
    public class PlayerAnimatedSystem : IEcsRunSystem
    {
        private const float StopHitValue = 0f;
        private const float StartHitValue = 0.4f;

        public readonly int Move = Animator.StringToHash(nameof(Move));
        public readonly int Hit = Animator.StringToHash(nameof(Hit));
        public readonly int Speed = Animator.StringToHash(nameof(Speed));

        private readonly EcsFilter<AnimatedComponent, PlayerMovableComponent, PlayerComponent> _animatedFilter;

        public void Run()
        {
            foreach (var entity in _animatedFilter)
            {
                ref var animatedComponent = ref _animatedFilter.Get1(entity);
                ref var movableComponent = ref _animatedFilter.Get2(entity);
                ref var playerComponent = ref _animatedFilter.Get3(entity);

                animatedComponent.Animator.SetBool(Move, movableComponent.IsMoving);

                if (playerComponent.MiningTool.IsMining && movableComponent.IsMoving)
                {
                    SetBlendTreeHitAndMoveAnimation(animatedComponent, StartHitValue);
                }
                else if (playerComponent.MiningTool.IsMining)
                {
                    SetHitAnimation(animatedComponent, true);
                }
                else
                {
                    SetHitAnimation(animatedComponent, false);
                    SetBlendTreeHitAndMoveAnimation(animatedComponent, StopHitValue);
                }
            }
        }

        private void SetBlendTreeHitAndMoveAnimation(AnimatedComponent animatedComponent, float value)
        {
            animatedComponent.Animator.SetFloat(Speed, value);
        }

        private void SetHitAnimation(AnimatedComponent animatedComponent, bool isHiting)
        {
            animatedComponent.Animator.SetBool(Hit, isHiting);
        }
    }
}