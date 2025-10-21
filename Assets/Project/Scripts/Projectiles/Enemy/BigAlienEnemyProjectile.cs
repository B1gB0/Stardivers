using UnityEngine;

namespace Project.Scripts.Projectiles.Enemy
{
    public class BigAlienEnemyProjectile : EnemyProjectile
    {
        public override void SetDirection(Vector3 targetPosition)
        {
            Direction = (targetPosition - Transform.position).normalized;
            Transform.LookAt(targetPosition);
        }
    }
}