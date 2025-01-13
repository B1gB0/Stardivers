using System.Collections.Generic;
using UnityEngine;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Levels.Mars.SecondLevel;

namespace Project.Scripts.Levels.Triggers
{
    public class BallisticRocketTrigger : Trigger
    {
        [SerializeField] private BallisticRocket _ballisticRocket;
        [SerializeField] private List<GameObject> _rocketBorders;

        private void OnEnable()
        {
            _ballisticRocket.LaunchCompleted += OffBorders;
        }

        private void OnDisable()
        {
            _ballisticRocket.LaunchCompleted -= OffBorders;
        }

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
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

        private void OffBorders()
        {
            foreach (var border in _rocketBorders)
            {
                border.gameObject.SetActive(false);
            }
        }
    }
}
