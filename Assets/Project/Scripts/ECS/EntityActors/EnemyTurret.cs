using Project.Scripts.Experience;
using Project.Scripts.Weapon.Enemy;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class EnemyTurret : EnemyActor, IAcceptable
    {
        [field: SerializeField] public AlienTurretWeapon Weapon { get; private set; }

        private void OnEnable()
        {
            Health.Die += OnDie;
        }

        private void OnDisable()
        {
            Health.Die -= OnDie;
        }

        protected override void OnDie()
        {
            Health.IsSpawnedDamageText -= TextService.OnChangedFloatingText;
            ExperiencePoints.OnKill(this);
            base.OnDie();

            gameObject.SetActive(false);
        }
        
        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}