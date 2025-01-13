using System;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class EndLevelTrigger : Trigger
    {
        public event Action IsLevelCompleted; 

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                IsLevelCompleted?.Invoke();
            }
        }
    }
}
