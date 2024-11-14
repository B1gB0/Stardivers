using Project.Scripts.ECS.EntityActors;
using UnityEngine.AI;

namespace Project.Scripts.ECS.Components
{
    public struct FollowPlayerComponent
    {
        public PlayerActor Target;
        public NavMeshAgent NavMeshAgent;
    }
}