using Build.Game.Scripts.ECS.Components;
using Build.Game.Scripts.ECS.EntityActors;
using Leopotam.Ecs;
using UnityEngine;

namespace Build.Game.Scripts.ECS.System
{
    public class PlayerAnimatedSystem : IEcsRunSystem
    {
        private static readonly int Idle = Animator.StringToHash(nameof(Idle));
        private static readonly int Move = Animator.StringToHash(nameof(Move));
        private static readonly int Hit = Animator.StringToHash(nameof(Hit));
        private static readonly int Speed = Animator.StringToHash(nameof(Speed));

        private readonly EcsFilter<AnimatedComponent, MovableComponent, PlayerComponent> _animatedFilter;

        private readonly float _stopHitValue = 0f;
        private readonly float _startHitValue = 0.4f;
        
        public void Run()
        {
            foreach (var entity in _animatedFilter)
            {
                ref var animatedComponent = ref _animatedFilter.Get1(entity);
                ref var movableComponent = ref _animatedFilter.Get2(entity);
                ref var playerComponent = ref _animatedFilter.Get3(entity);

                animatedComponent.animator.SetBool(Move, movableComponent.isMoving);

                if (playerComponent.MiningTool.IsMining && movableComponent.isMoving)
                {
                    SetBlendTreeHitAndMoveAnimation(animatedComponent, _startHitValue);
                }
                else if (playerComponent.MiningTool.IsMining)
                {
                    SetHitAnimation(animatedComponent, true);
                }
                else
                {
                    SetHitAnimation(animatedComponent, false);
                    SetBlendTreeHitAndMoveAnimation(animatedComponent, _stopHitValue);
                }
            }
        }

        public void SetBlendTreeHitAndMoveAnimation(AnimatedComponent animatedComponent, float value)
        {
            animatedComponent.animator.SetFloat(Speed, value);
        }

        public void SetHitAnimation(AnimatedComponent animatedComponent, bool isHiting)
        {
            animatedComponent.animator.SetBool(Hit, isHiting);
        }
    }
}