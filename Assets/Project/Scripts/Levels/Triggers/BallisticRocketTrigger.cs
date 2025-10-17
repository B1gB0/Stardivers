using UnityEngine;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Levels.Mars.SecondLevel;

namespace Project.Scripts.Levels.Triggers
{
    public class BallisticRocketTrigger : Trigger
    {
        [SerializeField] private BallisticRocket _ballisticRocket;

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                if(_ballisticRocket.gameObject.activeSelf)
                    _ballisticRocket.OnChangeProgress();
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                _ballisticRocket.OnChangeProgress();
            }
        }
    }
}
