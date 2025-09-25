using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class TruckObstacleTrigger : Trigger
    {
        public bool IsObstacleForward { get; private set; }
        
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out EnemyAlienActor enemy))
            {
                IsObstacleForward = true;
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out EnemyAlienActor enemy))
            {
                IsObstacleForward = false;
            }
        }
    }
}