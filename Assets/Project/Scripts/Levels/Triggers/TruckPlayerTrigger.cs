using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Levels.Mars.ThirdLevel;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class TruckPlayerTrigger : Trigger
    {
        [SerializeField] private Truck _truck;
        
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor player))
                _truck.OnPlayerIsNearby(player);
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor player))
                _truck.OnPlayerIsNotNearby();
        }
    }
}