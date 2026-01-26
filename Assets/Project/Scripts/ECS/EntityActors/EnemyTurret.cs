using Project.Scripts.Experience;
using Project.Scripts.Weapon.Enemy;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class EnemyTurret : EnemyActor
    {
        [field: SerializeField] public AlienTurretWeapon Weapon { get; private set; }

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