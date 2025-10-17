using System.Collections.Generic;
using System.Linq;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public class EnemyDetector : MonoBehaviour
    {
        private const int MinValue = 0;

        private readonly HashSet<EnemyActor> _enemiesInRange = new();

        private float _closestEnemyDistanceSqr;

        public float ClosestEnemyDistance => Mathf.Sqrt(_closestEnemyDistanceSqr);

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (!otherCollider.TryGetComponent(out EnemyActor enemyAlienActor))
                return;

            _enemiesInRange.Add(enemyAlienActor);

            enemyAlienActor.Die += OnEnemyDie;
        }

        private void OnTriggerExit(Collider otherCollider)
        {
            if (!otherCollider.TryGetComponent(out EnemyActor enemyAlienActor))
                return;
            
            _enemiesInRange.Remove(enemyAlienActor);

            enemyAlienActor.Die -= OnEnemyDie;
        }

        public EnemyActor GetClosestEnemy()
        {
            if (_enemiesInRange.Count <= MinValue)
                return null;

            EnemyActor bestTarget = null;
            _closestEnemyDistanceSqr = Mathf.Infinity;
            var currentPosition = transform.position;

            foreach (EnemyActor closestEnemy in _enemiesInRange)
            {
                var directionToTarget = closestEnemy.transform.position - currentPosition;
                var dSqrToTarget = directionToTarget.sqrMagnitude;

                if (!(dSqrToTarget < _closestEnemyDistanceSqr))
                    continue;

                _closestEnemyDistanceSqr = dSqrToTarget;
                bestTarget = closestEnemy;
            }

            return bestTarget;
        }

        public HashSet<EnemyActor> GetEnemiesInRange()
        {
            return _enemiesInRange;
        }

        private void OnEnemyDie(EnemyActor enemyActor)
        {
            _enemiesInRange.Remove(enemyActor);
            enemyActor.Die -= OnEnemyDie;
        }

        private void OnDestroy()
        {
            foreach (var enemyAlienActor in _enemiesInRange.Where(enemyAlienActor => enemyAlienActor != null))
            {
                enemyAlienActor.Die -= OnEnemyDie;
            }

            _enemiesInRange.Clear();
        }
    }
}