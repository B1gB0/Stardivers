using System.Collections.Generic;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public class EnemyDetector : MonoBehaviour
    {
        private const float MinValue = 0f;
        private const float SearchRadius = 5f;
    
        private readonly List<EnemyAlienActor> _enemies = new ();
    
        private float _currentDistanceOfClosestEnemy;

        public EnemyAlienActor NearestAlienEnemy { get; private set; }

        private void Update()
        {
            SetNearestEnemy();
        }

        private void SetNearestEnemy()
        {
            _enemies.Clear();
        
            Collider[] colliders = Physics.OverlapSphere(transform.position, SearchRadius);
        
            foreach (Collider collider in colliders)
                if (collider.attachedRigidbody != null && collider.gameObject.TryGetComponent(out EnemyAlienActor enemyActor))
                    _enemies.Add(enemyActor);

            float distance = Mathf.Infinity;

            Vector3 position = transform.localPosition;

            foreach (EnemyAlienActor enemy in _enemies)
            {
                if (enemy.Health.TargetHealth > MinValue)
                {
                    _currentDistanceOfClosestEnemy = Vector3.Distance(position, enemy.transform.localPosition);

                    if (_currentDistanceOfClosestEnemy < distance)
                    {
                        NearestAlienEnemy = enemy;
                        distance = _currentDistanceOfClosestEnemy;
                    }
                }
            }
        }
    }
}
