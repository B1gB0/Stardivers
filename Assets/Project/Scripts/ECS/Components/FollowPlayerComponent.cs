using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;
using UnityEngine.AI;

namespace Build.Game.Scripts.ECS.Components
{
    public struct FollowPlayerComponent
    {
        public PlayerActor Target;
        public NavMeshAgent NavMeshAgent;
    }
}