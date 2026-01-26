using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Projectiles.Mines
{
    public class IceCrystal : ExplodingObject
    {
        private const float DefaultDamage = 5f;
        private const float DefaultExplodingRadius = 5f;
        private const float DefaultSlowingDownSpeed = -0.5f;
        private const float SlowDuration = 3f;
        private const int MaxCount = 20;

        private readonly Collider[] _hits = new Collider[MaxCount];

        protected override void OnEnable() { }

        private void Start()
        {
            Damage = DefaultDamage;
            ExplosionRadius = DefaultExplodingRadius;
        }

        protected override void OnDisable() { }

        public void Construct(ParticleEffectsService particleEffectsService, AudioSoundsService audioSoundsService)
        {
            GetExplosionEffects(particleEffectsService, audioSoundsService);
        }

        protected override void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerActor player))
            {
                Explode();
                player.Health.TakeDamage(Damage);
                player.ApplyTemporaryModifier(DefaultSlowingDownSpeed, SlowDuration);
            }
            else if (collision.gameObject.TryGetComponent(out EnemyActor _))
            {
                Explode();
                var playerActor = GetPlayer();

                if (playerActor == null)
                    return;

                playerActor.Health.TakeDamage(Damage);
                playerActor.ApplyTemporaryModifier(DefaultSlowingDownSpeed, SlowDuration);
            }
        }

        protected override void Explode()
        {
            ParticleEffectsService.PlayEffect(ParticleEffectType.IceCrystalExplosion, Transform.position);
            AudioSoundsService.PlaySound(SoundsType.IceCrystalExplosion).Forget();

            foreach (EnemyActor enemy in GetEnemies())
            {
                enemy.Health.TakeDamage(Damage);
                enemy.ApplyTemporaryModifier(DefaultSlowingDownSpeed, SlowDuration);
            }

            gameObject.SetActive(false);
        }

        // private PlayerActor GetPlayer()
        // {
        //     Physics.OverlapSphereNonAlloc(Transform.position, ExplosionRadius, _hits);
        //
        //     foreach (Collider hit in _hits)
        //     {
        //         if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out PlayerActor player))
        //         {
        //             return player;
        //         }
        //     }
        //
        //     return null;
        // }
        
        private PlayerActor GetPlayer()
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, _hits);
    
            for (int i = 0; i < count; i++)
            {
                Collider hit = _hits[i];
                if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out PlayerActor player))
                {
                    return player;
                }
            }
    
            return null;
        }
    }
}