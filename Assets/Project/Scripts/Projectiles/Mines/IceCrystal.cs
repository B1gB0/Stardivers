using Project.Scripts.Audio.Sounds;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Projectiles.Mines
{
    public class IceCrystal : ExplodingObject
    {
        private const float DefaultDamage = 5f;
        private const float DefaultSlowingDownSpeed = -0.5f;
        private const float SlowDuration = 3f;

        private PlayerActor _player;

        protected override void OnEnable() { }
        protected override void OnDisable() { }

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
                player.PlayerCharacteristics.ApplyTemporarySpeedModifier(DefaultSlowingDownSpeed, SlowDuration);
            }
            else if(collision.gameObject.TryGetComponent(out EnemyActor enemy))
            {
                Explode();
                _player = GetPlayer();

                if (_player == null)
                    return;
                
                _player.Health.TakeDamage(Damage);
                _player.PlayerCharacteristics.ApplyTemporarySpeedModifier(DefaultSlowingDownSpeed, SlowDuration);
            }
        }

        protected override void Explode()
        {
            ExplosionEffect.transform.position = Transform.position;
            ExplosionEffect.Play();
            AudioSoundsService.PlaySound(SoundsType.Mines);

            foreach (EnemyActor enemy in GetEnemies())
            {
                enemy.Health.TakeDamage(Damage);
                
                if(enemy is IFreezable freezable)
                    freezable.SetSpeed(DefaultSlowingDownSpeed);
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