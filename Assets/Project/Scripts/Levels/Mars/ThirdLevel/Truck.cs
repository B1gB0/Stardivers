using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.ThirdLevel
{
    public class Truck : MonoBehaviour
    {
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out ParkingTrigger parkingTrigger))
            {
                parkingTrigger.Entrance.OpenGate();
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out ParkingTrigger parkingTrigger))
            {
                parkingTrigger.Entrance.CloseGate();
            }
        }
    }
}