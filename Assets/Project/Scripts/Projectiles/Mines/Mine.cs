using System.Collections;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Projectiles.Mines
{
    public class Mine : ExplodingObject
    {
        protected override void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out EnemyAlienActor enemy))
            {
                Explode();
                StopCoroutine(LifeRoutine());
            }
        }

        public override void SetCharacteristics(float damage, float explosionRadius)
        {
            Damage = damage;
            ExplosionRadius = explosionRadius;
        }

        protected override IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(LifeTime);
        
            Explode();
        }
    }
}
