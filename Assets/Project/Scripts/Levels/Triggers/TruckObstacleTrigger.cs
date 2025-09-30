using System.Collections.Generic;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class TruckObstacleTrigger : Trigger
    {
        private const int MinValue = 0;
            
        private readonly HashSet<GameObject> _obstaclesInTrigger = new ();
        
        public bool IsObstacleForward { get; private set; }

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out EnemyAlienActor enemy))
            {
                enemy.Health.DieHealth += OnObstacleDestroy;
                
                _obstaclesInTrigger.Add(trigger.gameObject);
                IsObstacleForward = true;
            }

            if (trigger.TryGetComponent(out ResourceActor resource))
            {
                resource.Health.DieHealth += OnObstacleDestroy;
                
                _obstaclesInTrigger.Add(trigger.gameObject);
                IsObstacleForward = true;
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out EnemyAlienActor enemy) || trigger.TryGetComponent(out ResourceActor resource))
            {
                _obstaclesInTrigger.Remove(trigger.gameObject);
                IsObstacleForward = _obstaclesInTrigger.Count > MinValue;
            }
        }

        private void OnObstacleDestroy(Health.Health health)
        {
            health.DieHealth -= OnObstacleDestroy;
            
            _obstaclesInTrigger.RemoveWhere(obstacle => 
                obstacle == null || !obstacle.activeInHierarchy);
            
            IsObstacleForward = _obstaclesInTrigger.Count > MinValue;
        }
    }
}