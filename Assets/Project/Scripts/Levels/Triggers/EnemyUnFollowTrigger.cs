using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class EnemyUnFollowTrigger : Trigger
    {
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor playerActor))
            {
                playerActor.ChangeFollowEnemyState(false);
            }
        }
    }
}