using System.Collections;
using Project.Game.Scripts;
using Project.Scripts.ECS.Components;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Projectiles.Mines
{
    public class IceCrystal : ExplodingObject
    {
        private const float DefaultDamage = 5f;
        private const float DefaultSlowingDownSpeed = -1f;

        private PlayerActor _player;
        private PlayerMovableComponent _playerMovableComponent;

        private void Start()
        {
            Damage = DefaultDamage;
        }

        protected override void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out PlayerActor player))
            {
                Explode();
                player.Health.TakeDamage(Damage);
                StopCoroutine(LifeRoutine());
            }
            else if(collision.gameObject.TryGetComponent(out EnemyAlienActor enemy))
            {
                Explode();
                _player = GetPlayer();

                if (_player != null)
                {
                    _player.Health.TakeDamage(Damage);
                }
            }
        }

        protected override IEnumerator LifeRoutine()
        {
            yield break;
        }

        protected override void Explode()
        {
            ExplosionEffect.transform.position = Transform.position;
            ExplosionEffect.Play();
            AudioSoundsService.PlaySound(Sounds.Mines);

            foreach (EnemyAlienActor enemy in GetEnemies())
            {
                enemy.Health.TakeDamage(Damage);
                enemy.SetSpeed(DefaultSlowingDownSpeed);
            }
        
            gameObject.SetActive(false);
        }

        private PlayerActor GetPlayer()
        {
            Collider[] hits = Physics.OverlapSphere(Transform.position, ExplosionRadius);

            foreach (Collider hit in hits)
                if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out PlayerActor player))
                    return player;

            return null;
        }
    }
}