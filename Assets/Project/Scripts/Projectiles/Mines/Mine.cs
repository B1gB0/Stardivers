using System.Collections;
using System.Collections.Generic;
using Project.Game.Scripts;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles;
using Project.Scripts.Services;
using UnityEngine;

public class Mine : Projectile
{
    private ParticleSystem _explosionEffect;
    private AudioSoundsService _audioSoundsService;
    
    private float _damage;
    private float _explosionRadius;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyAlienActor enemy))
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
    
    public void GetExplosionEffects(ParticleSystem effect, AudioSoundsService audioSoundsService)
    {
        _explosionEffect = effect;
        _audioSoundsService = audioSoundsService;
    }

    protected override IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(LifeTime);
        
        Explode();
    }

    private void Explode()
    {
        _explosionEffect.transform.position = transform.position;
        _explosionEffect.Play();
        _audioSoundsService.PlaySound(Sounds.Mines);

        foreach (EnemyAlienActor explosiveObject in GetExplosiveObjects())
        {
            explosiveObject.Health.TakeDamage(_damage);
        }
        
        gameObject.SetActive(false);
    }
    
    private List<EnemyAlienActor> GetExplosiveObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        List<EnemyAlienActor> enemies = new();

        foreach (Collider hit in hits)
            if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out EnemyAlienActor enemyActor))
                enemies.Add(enemyActor);

        return enemies;
    }
}
