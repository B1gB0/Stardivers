using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Levels.Mars.ThirdLevel;
using Project.Scripts.Levels.Outpost;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class ParkingTrigger : Trigger
    {
        [field: SerializeField] public Entrance Entrance { get; private set; }
        
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out Truck truck) || trigger.TryGetComponent(out PlayerActor playerActor))
            {
                Entrance.OpenGate();
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out Truck truck) || trigger.TryGetComponent(out PlayerActor playerActor))
            {
                Entrance.CloseGate();
            }
        }
    }
}