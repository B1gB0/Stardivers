using System.Collections.Generic;
using Build.Game.Scripts.ECS.Components;
using Leopotam.Ecs;
using Project.Scripts.Score;

namespace Build.Game.Scripts.ECS.System
{
    public class PlayerProgressSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent> _attackFilter;
        
        private ExperiencePoints experiencePoints;
        
        public void Run()
        {
            foreach (var entity in  _attackFilter)
            {
                ref var player = ref _attackFilter.Get1(entity);

                ref List<Weapon> weapons = ref player.weapons;
                
                
            }
        }
    }
}