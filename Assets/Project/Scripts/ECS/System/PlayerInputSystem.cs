using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;

namespace Build.Game.Scripts.ECS.System
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent, InputEventComponent> _inputEventFilter;

        public void Run()
        {
            foreach (var entity in _inputEventFilter)
            {
                ref var inputEvent = ref _inputEventFilter.Get2(entity);
            }
        }
    }
}