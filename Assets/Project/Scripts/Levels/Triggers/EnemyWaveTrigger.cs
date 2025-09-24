using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class EnemyWaveTrigger : Trigger
    {
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                Deactivate();
            }
        }
    }
}