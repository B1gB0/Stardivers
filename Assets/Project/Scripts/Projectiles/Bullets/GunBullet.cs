using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles;
using UnityEngine;

public class GunBullet : Projectile
{
    private Vector3 _direction;
    
    private float _bulletSpeed;
    private float _damage;
    
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.TryGetComponent(out SmallAlienEnemy enemy))
        {
            enemy.Health.TakeDamage(_damage);
            gameObject.SetActive(false);
        }
    }
    
    private void FixedUpdate()
    {
        transform.position += _direction * (_bulletSpeed * Time.fixedDeltaTime);
    }

    public void SetDirection(Transform enemy)
    {
        _direction = (enemy.position - transform.position).normalized;
        transform.LookAt(enemy);
    }
    
    public void SetCharacteristics(float damage, float bulletSpeed)
    {
        _damage = damage;
        _bulletSpeed = bulletSpeed;
    }
}
