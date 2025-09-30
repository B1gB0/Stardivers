using System;
using Project.Scripts.Levels.Mars.ThirdLevel;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class TruckFinalPointTrigger : Trigger
    {
        [SerializeField] private ParticleSystem _zoneEffect;

        public event Action IsFinalPointReached;
        
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out Truck truck))
            {
                truck.ReachFinalPoint();
                _zoneEffect.gameObject.SetActive(false);
                IsFinalPointReached?.Invoke();
                
                Deactivate();
            }
        }
    }
}