using Project.Scripts.Crystals;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.Services;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.Player
{
    [RequireComponent(typeof(PlayerActor))]
    public class PlayerCollisionHandler : MonoBehaviour
    {
        private IPlayerService _playerService;

        [Inject]
        private void Construct(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out EntranceTrigger entranceTrigger))
            {
                entranceTrigger.Entrance.OpenGate();
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out EntranceTrigger entranceTrigger))
            {
                entranceTrigger.Entrance.CloseGate();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out RedCrystal healingCrystal))
            {
                if (_playerService.PlayerActor.Health.TargetHealth == _playerService.PlayerActor.Health.MaxHealth)
                    return;

                _playerService.PlayerActor.Health.AddHealth(healingCrystal.HealthValue);
                healingCrystal.Destroy();
            }
            else if (collision.gameObject.TryGetComponent(out GoldCrystal goldCrystal))
            {
                goldCrystal.Destroy();
            }
        }
    }
}