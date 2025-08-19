using System;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class WelcomePlanetTextTrigger : Trigger
    {
        public event Action IsWelcomeToPlanet; 

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                IsWelcomeToPlanet?.Invoke();
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