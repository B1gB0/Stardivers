using Project.Scripts.Experience;
using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Weapon.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.EntityActors
{
    public class BigEnemy : EnemyActor
    {
        [field: SerializeField] public BigEnemyAlienWeapon Weapon { get; private set; }
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }

        private void Start()
        {
            Weapon.GetServices(AudioSoundsService);
        }

        private void OnEnable()
        {
            Health.Die += OnDie;
            Health.IsDamaged += OnPlayParticleEffect;
        }

        private void OnDisable()
        {
            Health.Die -= OnDie;
            Health.IsDamaged -= OnPlayParticleEffect;
        }

        protected override void OnDie()
        {
            Health.IsSpawnedDamageText -= TextService.OnChangedFloatingText;
            ExperiencePoints.OnKill(this);
            base.OnDie();

            gameObject.SetActive(false);
        }

        protected override void OnPlayParticleEffect()
        {
            ParticleEffectsService.PlayEffect(ParticleEffectType.BigEnemyHit, Health.HitPoint.position);
        }
    }
}