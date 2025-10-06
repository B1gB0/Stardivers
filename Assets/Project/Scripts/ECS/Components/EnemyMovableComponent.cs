using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.Components
{
    public struct EnemyMovableComponent
    {
        public NavMeshAgent NavMeshAgent;
        public Transform Transform;
        public float MoveSpeed;
        public bool IsMoving;
        public bool IsAttack;
    }
}