using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class TruckPlayerTrigger : Trigger
    {
        public bool IsPlayerNearby { get; private set; }
        
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor player))
            {
                IsPlayerNearby = true;
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor player))
            {
                IsPlayerNearby = false;
            }
        }
    }
}