using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.System
{
    public class PatrolSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PatrolComponent, EnemyMovableComponent, FollowPlayerComponent> _patrolFilter;

        public void Run()
        {
            foreach (var entity in _patrolFilter)
            {
                ref var patrol = ref _patrolFilter.Get1(entity);
                ref var movable = ref _patrolFilter.Get2(entity);
                ref var followComponent = ref _patrolFilter.Get3(entity);
                
                if (patrol.Points.Count == 0) continue;

                if (followComponent.Target.CanFollow)
                    continue;
                
                var navMeshAgent = movable.NavMeshAgent;
                
                if (!navMeshAgent.gameObject.activeSelf) continue;
                
                Debug.Log(navMeshAgent.remainingDistance + " требуемая дистанция");
                Debug.Log(navMeshAgent.stoppingDistance + " остановочная дистанция");

                if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    patrol.CurrentPointIndex = (patrol.CurrentPointIndex + 1) % patrol.Points.Count;
                    GotoNextPoint(patrol, navMeshAgent);
                }

                movable.IsMoving = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
            }
        }
    
        private void GotoNextPoint(PatrolComponent patrol, NavMeshAgent agent)
        {
            Debug.Log(patrol.Points.Count);
            
            if (patrol.Points.Count == 0) return;
            
            agent.destination = patrol.Points[patrol.CurrentPointIndex];
        }
    }
}