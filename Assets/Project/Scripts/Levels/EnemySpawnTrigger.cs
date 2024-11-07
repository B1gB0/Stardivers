using System;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;
using Action = Unity.Plastic.Newtonsoft.Json.Serialization.Action;

namespace Project.Scripts.Levels
{
    public class EnemySpawnTrigger : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _zoneEffect;
        
        public bool IsEnemySpawned { get; private set; }

        public event Action IsTimerLaunched;

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                IsEnemySpawned = true;
                _zoneEffect.gameObject.SetActive(false);
                IsTimerLaunched?.Invoke();
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                gameObject.SetActive(false);
            }
        }

        public void CompleteSpawn()
        {
            IsEnemySpawned = false;
        }
    }
}