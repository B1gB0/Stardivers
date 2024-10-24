using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

public class ClosestEnemyDetector : MonoBehaviour
{
    private const float MinValue = 0f;
    private const float SearchRadius = 5f;
    
    private readonly List<SmallAlienEnemyActor> _enemies = new ();
    
    private float _currentDistanceOfClosestEnemy;

    public SmallAlienEnemyActor СlosestSmallAlienEnemy { get; private set; }

    private void Update()
    {
        SetClosestEnemy();
    }

    private void SetClosestEnemy()
    {
        _enemies.Clear();
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, SearchRadius);
        
        foreach (Collider collider in colliders)
            if (collider.attachedRigidbody != null && collider.gameObject.TryGetComponent(out SmallAlienEnemyActor enemyActor))
                _enemies.Add(enemyActor);

        float distance = Mathf.Infinity;

        Vector3 position = transform.localPosition;

        foreach (SmallAlienEnemyActor enemy in _enemies)
        {
            if (enemy.Health.TargetHealth > MinValue)
            {
                _currentDistanceOfClosestEnemy = Vector3.Distance(position, enemy.transform.localPosition);

                if (_currentDistanceOfClosestEnemy < distance)
                {
                    СlosestSmallAlienEnemy = enemy;
                    distance = _currentDistanceOfClosestEnemy;
                }
            }
        }
    }
}
