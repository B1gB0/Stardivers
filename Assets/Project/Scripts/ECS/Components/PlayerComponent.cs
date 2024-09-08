using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Build.Game.Scripts.ECS.Components
{
    public struct PlayerComponent
    {
        public Health health;
        public List<Weapon> weapons;
        public MiningToolActor MiningTool;
    }
}