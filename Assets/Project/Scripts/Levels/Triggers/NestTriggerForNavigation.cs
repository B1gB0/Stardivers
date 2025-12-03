using Project.Scripts.ECS.EntityActors;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class NestTriggerForNavigation : Trigger
    {
        [SerializeField] private Transform _targetMissionPoint;
        
        private Arrow _arrow;
        
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                _arrow.OnLookAtTarget(_targetMissionPoint);
            }
        }

        public void GetArrow(Arrow arrow)
        {
            _arrow = arrow;
        }
    }
}