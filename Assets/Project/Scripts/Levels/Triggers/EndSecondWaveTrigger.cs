using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class EndSecondWaveTrigger : Trigger
    {
        [SerializeField] private EnemySpawnTriggerWithoutEffect enemySpawnTriggerWithoutEffect;
        
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                enemySpawnTriggerWithoutEffect.CompleteSpawn();
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