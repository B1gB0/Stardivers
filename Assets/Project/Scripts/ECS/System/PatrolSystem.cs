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
                {
                    patrolComponent.IsPatrol = false;
                    continue;
                }

                var navMeshAgent = movableComponent.NavMeshAgent;

                if (!navMeshAgent.gameObject.activeSelf) continue;

                if (!patrolComponent.IsPatrol)
                {
                    GotoCurrentPoint(ref patrolComponent, navMeshAgent);
                    patrolComponent.IsPatrol = true;
                }
                
                if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    SetNextPoint(ref patrolComponent);
                    GotoCurrentPoint(ref patrolComponent, navMeshAgent);
                }
                
                movableComponent.IsMoving = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
            }
        }

        private void SetNextPoint(ref PatrolComponent patrol)
        {
            patrol.CurrentPointIndex = (patrol.CurrentPointIndex + StepPoint) % patrol.Points.Count;
        }

        private void GotoCurrentPoint(ref PatrolComponent patrol, NavMeshAgent agent)
        {
            if (patrol.Points.Count == MinValue) return;
            
            agent.destination = patrol.Points[patrol.CurrentPointIndex];
        }
    }
}