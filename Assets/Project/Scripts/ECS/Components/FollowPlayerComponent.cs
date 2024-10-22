using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;
using UnityEngine.AI;

namespace Build.Game.Scripts.ECS.Components
{
    public struct FollowPlayerComponent
    {
        public PlayerActor target;
        public NavMeshAgent navMeshAgent;
    }
}