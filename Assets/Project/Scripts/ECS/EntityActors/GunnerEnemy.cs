using Project.Scripts.Experience;
using Project.Scripts.Weapon.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.EntityActors
{
    public class GunnerEnemy : EnemyActor
    {
        [field: SerializeField] public GunnerEnemyAlienWeapon Weapon { get; private set; }
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
    }
}