using System;
using Project.Scripts.Health;
using UnityEngine;

namespace Build.Game.Scripts.ECS.Components
{
    public struct ResourceComponent
    {
        public Health Health;
        public bool IsHitting;
    }
}