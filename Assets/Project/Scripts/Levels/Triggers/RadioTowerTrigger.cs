using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Levels.MysteryPlanet.SecondLevel;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class RadioTowerTrigger : Trigger
    {
        [SerializeField] private RadioTower _radioTower;

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                if(_radioTower.gameObject.activeSelf)
                    _radioTower.OnChangeProgress();
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                _radioTower.OnChangeProgress();
            }
        }
    }
}