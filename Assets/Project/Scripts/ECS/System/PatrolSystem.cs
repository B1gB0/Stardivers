using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using UnityEngine.AI;

namespace Project.Scripts.ECS.System
{
    public class PatrolSystem : IEcsRunSystem
    {
        private const int MinValue = 0;
        private const int StepPoint = 1;

        private readonly EcsFilter<PatrolComponent, EnemyMovableComponent, FollowPlayerComponent> _patrolFilter;

        public void Run()
        {
            foreach (var entity in _patrolFilter)
            {
                ref var patrolComponent = ref _patrolFilter.Get1(entity);
                ref var movableComponent = ref _patrolFilter.Get2(entity);
                ref var followComponent = ref _patrolFilter.Get3(entity);

                if (patrolComponent.Points.Count == MinValue) continue;

                if (followComponent.Target.CanFollow)
                    continue;

                var navMeshAgent = movableComponent.NavMeshAgent;

                if (!navMeshAgent.gameObject.activeSelf) continue;

                if (navMeshAgent.destination != patrolComponent.Points[patrolComponent.CurrentPointIndex])
                    SetPosition(patrolComponent, navMeshAgent, movableComponent);

                if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                    SetPosition(patrolComponent, navMeshAgent, movableComponent);

                movableComponent.IsMoving = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
            }
        }

        private void SetPosition(PatrolComponent patrol, NavMeshAgent agent, EnemyMovableComponent movableComponent)
        {
            patrol.CurrentPointIndex = (patrol.CurrentPointIndex + StepPoint) % patrol.Points.Count;
            GotoNextPoint(patrol, agent);
        }

        private void GotoNextPoint(PatrolComponent patrol, NavMeshAgent agent)
        {
            if (patrol.Points.Count == MinValue) return;

            agent.destination = patrol.Points[patrol.CurrentPointIndex];
        }
    }
}