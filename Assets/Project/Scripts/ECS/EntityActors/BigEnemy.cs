using Project.Scripts.Experience;
using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Weapon.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.EntityActors
{
    public class BigEnemy : EnemyActor, IAcceptable
    {
        [field: SerializeField] public BigEnemyAlienWeapon Weapon { get; private set; }
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }

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
        
        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
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