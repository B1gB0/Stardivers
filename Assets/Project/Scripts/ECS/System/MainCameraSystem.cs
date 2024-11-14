using Leopotam.Ecs;
using Cinemachine;
using Project.Scripts.ECS.Components;

namespace Build.Game.Scripts.ECS.System
{
    public class MainCameraSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent, PlayerMovableComponent> _mainCameraFilter;
        private readonly CinemachineVirtualCamera _mainCamera;

        public MainCameraSystem(CinemachineVirtualCamera mainCamera)
        {
            _mainCamera = mainCamera;
        }
        
        public void Run()
        {
            foreach (var entity in _mainCameraFilter)
            {
                ref var movableComponent = ref _mainCameraFilter.Get2(entity);
                _mainCamera.Follow = movableComponent.Transform;
            }
        }
    }
}