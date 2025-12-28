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
            if (trigger.TryGetComponent(out PlayerActor player))
                _radioTower.OnChangeProgress(player);
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor player))
                _radioTower.OnStopChangeProgress();
        }
    }
}