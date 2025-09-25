using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class TruckTrigger : Trigger
    {
        public bool IsPlayerNearby { get; private set; }
        
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor player) || trigger.TryGetComponent(out ResourceActor resource))
            {
                IsPlayerNearby = true;
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor player) || trigger.TryGetComponent(out ResourceActor resource))
            {
                IsPlayerNearby = false;
            }
        }
    }
}