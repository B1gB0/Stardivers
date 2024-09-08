using System;
using UnityEngine;

namespace Build.Game.Scripts.ECS.Components
{
    public struct MovableComponent
    {
        public Transform transform;
        public float moveSpeed;
        public float rotationSpeed;
        public bool isMoving;
        public Rigidbody rigidbody;
    }
}