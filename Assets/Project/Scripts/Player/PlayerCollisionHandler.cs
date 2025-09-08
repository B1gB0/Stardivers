using Project.Scripts.Crystals;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Player
{
    [RequireComponent(typeof(PlayerActor))]
    public class PlayerCollisionHandler : MonoBehaviour
    {
        private PlayerActor _player;
        
        private void Start()
        {
            _player = GetComponent<PlayerActor>();
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
                if(_player.Health.TargetHealth == _player.Health.MaxHealth) return;
            
                _player.Health.AddHealth(healingCrystal.HealthValue);
                healingCrystal.Destroy();
            }
            else if (collision.gameObject.TryGetComponent(out GoldCrystal goldCrystal))
            {
                goldCrystal.Destroy();
            }
        }
    }
}