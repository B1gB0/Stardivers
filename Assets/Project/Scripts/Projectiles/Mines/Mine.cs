using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles;
using UnityEngine;

public class Mine : Projectile
{
    private ParticleSystem _explosionEffect;
    
    private float _damage;
    private float _explosionRadius;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyActor enemy))
        {
            Explode();
            StopCoroutine(LifeRoutine());
        }
    }

    public void SetCharacteristics(float damage, float explosionRadius)
    {
        _damage = damage;
        _explosionRadius = explosionRadius;
    }
    
    public void GetEffect(ParticleSystem effect)
    {
        _explosionEffect = effect;
    }
    
    private void Explode()
    {
        //_source.Play();
        _explosionEffect.transform.position = transform.position;
        _explosionEffect.Play();

        foreach (EnemyActor explosiveObject in GetExplosiveObjects())
        {
            explosiveObject.Health.TakeDamage(_damage);
        }
        
        gameObject.SetActive(false);
    }
    
    private List<EnemyActor> GetExplosiveObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        List<EnemyActor> enemies = new();

        foreach (Collider hit in hits)
            if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out EnemyActor enemyActor))
                enemies.Add(enemyActor);

        return enemies;
    }
}
