using Project.Scripts.Experience;
using Project.Scripts.Weapon.Enemy;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class GunnerAlienEnemy : EnemyAlienActor, IAcceptable
    {
        [field: SerializeField] public GunnerEnemyAlienWeapon Weapon { get; private set; }
        
        private void OnEnable()
        {
            Health.Die += OnDie;
        }

        private void OnDisable()
        {
            Health.Die -= OnDie;
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
    }
}
