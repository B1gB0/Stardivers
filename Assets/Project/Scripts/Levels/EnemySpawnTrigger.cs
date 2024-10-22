using System.Collections;
using Build.Game.Scripts.ECS.EntityActors;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Project.Scripts.Operations
{
    public class EnemySpawnTrigger : MonoBehaviour
    {
        private Coroutine _coroutine;

        public bool IsEnemySpawned { get; private set; }

        public event Action IsTimerLaunched;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerActor _))
            {
                IsEnemySpawned = true;
                IsTimerLaunched?.Invoke();
            }
        }

        public void CompleteSpawn()
        {
            IsEnemySpawned = false;
        }
    }
}