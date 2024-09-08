using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Build.Game.Scripts.ECS.System
{
    public class ResourcesAnimatedSystem : IEcsRunSystem
    {
        private static readonly int Idle = Animator.StringToHash(nameof(Idle));
        private static readonly int Hit = Animator.StringToHash(nameof(Hit));

        private readonly EcsFilter<ResourceComponent, AnimatedComponent> _animatedFilter;
        
        public void Run()
        {
            foreach (var entity in _animatedFilter)
            {
                ref var resourceComponent = ref _animatedFilter.Get1(entity);

                ref var animatedComponent = ref _animatedFilter.Get2(entity);

                animatedComponent.animator.SetBool(Hit, resourceComponent.health.IsHitting);
            }
        }
    }
}