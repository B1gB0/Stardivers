using UnityEngine;

namespace Project.Scripts.ECS.Components
{
    public struct PlayerMovableComponent
    {
        public Transform Transform;
        public float MoveSpeed;
        public float RotationSpeed;
        public bool IsMoving;
        public Rigidbody Rigidbody;
    }
}