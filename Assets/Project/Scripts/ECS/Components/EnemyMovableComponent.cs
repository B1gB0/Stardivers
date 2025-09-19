using UnityEngine;

namespace Project.Scripts.ECS.Components
{
    public struct EnemyMovableComponent
    {
        public Transform Transform;
        public float MoveSpeed;
        public bool IsMoving;
    }
}