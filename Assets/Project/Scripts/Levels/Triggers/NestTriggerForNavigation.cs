using Project.Scripts.ECS.EntityActors;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class NestTriggerForNavigation : Trigger
    {
        [field: SerializeField] public Transform TargetNestPoint { get; private set; }

        private Arrow _arrow;

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
                _arrow.OnLookAtTarget(TargetNestPoint);
        }
        
        public void GetData(Arrow arrow)
        {
            _arrow = arrow;
        }
    }
}