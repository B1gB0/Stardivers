using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.ECS.Components
{
    public struct PatrolComponent
    {
        public List<Vector3> Points;
        public int CurrentPointIndex;
        public bool IsPatrol;
        public float WaitingDuration;
        public float Timer;
    }
}