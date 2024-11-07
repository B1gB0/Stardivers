using Project.Scripts.Health;
using UnityEngine;

namespace Build.Game.Scripts.ECS.Components
{
    public struct EnemyComponent
    {
        public Health Health;
        public Collider Collider;
    }
}