using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

public class ClosestEnemyDetector : MonoBehaviour
{
    private const float MinValue = 0f;
    private const float SearchRadius = 5f;
    
    private readonly List<EnemyActor> _enemies = new ();
    
    private float _currentDistanceOfClosestEnemy;

    public EnemyActor СlosestEnemy { get; private set; }

    private void Update()
    {
        SetClosestEnemy();
    }

    private void SetClosestEnemy()
    {
        _enemies.Clear();
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, SearchRadius);
        
        foreach (Collider collider in colliders)
            if (collider.attachedRigidbody != null && collider.gameObject.TryGetComponent(out EnemyActor enemyActor))
                _enemies.Add(enemyActor);

        float distance = Mathf.Infinity;

        Vector3 position = transform.localPosition;

        foreach (EnemyActor enemy in _enemies)
        {
            if (enemy.Health.Value > MinValue)
            {
                _currentDistanceOfClosestEnemy = Vector3.Distance(position, enemy.transform.localPosition);

                if (_currentDistanceOfClosestEnemy < distance)
                {
                    СlosestEnemy = enemy;
                    distance = _currentDistanceOfClosestEnemy;
                }
            }
        }
    }
}
