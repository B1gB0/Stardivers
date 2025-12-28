using System;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;
using YG;

namespace Project.Scripts.Levels.Triggers
{
    public class EndLevelTrigger : Trigger
    {
        public event Action IsLevelCompleted; 

        private void OnTriggerEnter(Collider trigger)
        {
            if (!trigger.TryGetComponent(out PlayerActor _))
                return;
            
            YG2.SaveProgress();
            IsLevelCompleted?.Invoke();
        }
    }
}
