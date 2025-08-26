using System.Collections.Generic;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public class NewEnemyDetector : MonoBehaviour
    {
        private const int MinValue = 0;
        
        private readonly List<EnemyAlienActor> _enemiesInRange = new();

        private void OnTriggerEnter(Collider thisCollider)
        {
            if (!thisCollider.TryGetComponent(out EnemyAlienActor enemyAlienActor))
                return;
            
            if (_enemiesInRange.Count == MinValue)
            {
                _enemiesInRange.Add(enemyAlienActor);
            }
            else if (!_enemiesInRange.Contains(enemyAlienActor))
            {
                _enemiesInRange.Add(enemyAlienActor);
            }
        }

        private void OnTriggerExit(Collider thisCollider)
        {
            if (!thisCollider.TryGetComponent(out EnemyAlienActor enemyAlienActor))
                return;
            
            if (_enemiesInRange.Count > MinValue)
            {
                _enemiesInRange.Remove(enemyAlienActor);
            }
        }

        public EnemyAlienActor GetClosestEnemy()
        {
            if (_enemiesInRange.Count <= MinValue)
                return null;
            
            EnemyAlienActor bestTarget = null;
            var closestDistanceSqr = Mathf.Infinity;
            var currentPosition = transform.position;

            foreach (EnemyAlienActor closestEnemy in _enemiesInRange)
            {
                var directionToTarget = closestEnemy.transform.position - currentPosition;
                var dSqrToTarget = directionToTarget.sqrMagnitude;

                if (!(dSqrToTarget < closestDistanceSqr))
                    continue;
                
                closestDistanceSqr = dSqrToTarget;
                bestTarget = closestEnemy;
            }

            return bestTarget;

        }

        public List<EnemyAlienActor> GetEnemiesInRange()
        {
            return _enemiesInRange;
        }
    }
}