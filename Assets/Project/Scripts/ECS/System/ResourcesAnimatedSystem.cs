using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using UnityEngine;

namespace Project.Scripts.ECS.System
{
    public class ResourcesAnimatedSystem : IEcsRunSystem
    {
        public readonly int Idle = Animator.StringToHash(nameof(Idle));
        public readonly int Hit = Animator.StringToHash(nameof(Hit));

        private readonly EcsFilter<ResourceComponent, AnimatedComponent> _animatedFilter;
        
        public void Run()
        {
            foreach (var entity in _animatedFilter)
            {
                ref var resourceComponent = ref _animatedFilter.Get1(entity);

                ref var animatedComponent = ref _animatedFilter.Get2(entity);

                animatedComponent.Animator.SetBool(Hit, resourceComponent.Health.IsHitting);
            }
        }
    }
}