using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using Cinemachine;

namespace Build.Game.Scripts.ECS.System
{
    public class MainCameraSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent, MovableComponent> _mainCameraFilter;
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
                _mainCamera.Follow = movableComponent.transform;
            }
        }
    }
}