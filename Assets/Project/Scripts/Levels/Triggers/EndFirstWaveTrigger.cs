using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class EndFirstWaveTrigger : Trigger
    {
        [SerializeField] private EnemySpawnTriggerWithEffect enemySpawnTriggerWithEffect;
        
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                enemySpawnTriggerWithEffect.CompleteSpawn();
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                Deactivate();
            }
        }
    }
}