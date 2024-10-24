using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Build.Game.Scripts.ECS.Components
{
    public struct PlayerComponent
    {
        public Health Health;
        public List<Weapon> Weapons;
        public MiningToolActor MiningTool;
    }
}