using System.Collections;
using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles;
using UnityEngine;

public class MachineGunBullet : Projectile
{
    private Transform _enemy;
    
    private float _bulletSpeed;
    private float _damage;
    
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyActor enemy))
        {
            enemy.Health.TakeDamage(_damage);
            gameObject.SetActive(false);
        }
    }
    
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _enemy.position,
            _bulletSpeed * Time.fixedDeltaTime);
    }

    public void SetDirection(Transform enemy)
    {
        _enemy = enemy;
        transform.LookAt(enemy);
    }

    public void SetCharacteristics(float damage, float bulletSpeed)
    {
        _damage = damage;
        _bulletSpeed = bulletSpeed;
    }
}
