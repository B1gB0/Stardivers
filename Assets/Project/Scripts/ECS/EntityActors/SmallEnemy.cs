using Project.Scripts.Experience;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.EntityActors
{
    public class SmallEnemy : EnemyActor
    {
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

        protected override void OnDie()
        {
            Health.IsSpawnedDamageText -= TextService.OnChangedFloatingText;
            ExperiencePoints.OnKill(this);
            base.OnDie();

            gameObject.SetActive(false);
        }
    }
}