using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

public class ClosestEnemyDetector : MonoBehaviour
{
    private const float MinValue = 0f;
    private const float SearchRadius = 5f;
    
    private readonly List<SmallAlienEnemy> _enemies = new ();
    
    private float _currentDistanceOfClosestEnemy;

    public SmallAlienEnemy СlosestSmallAlienEnemy { get; private set; }

    private void Update()
    {
        SetClosestEnemy();
    }

    private void SetClosestEnemy()
    {
        _enemies.Clear();
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, SearchRadius);
        
        foreach (Collider collider in colliders)
            if (collider.attachedRigidbody != null && collider.gameObject.TryGetComponent(out SmallAlienEnemy enemyActor))
                _enemies.Add(enemyActor);

        float distance = Mathf.Infinity;

        Vector3 position = transform.localPosition;

        foreach (SmallAlienEnemy enemy in _enemies)
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
