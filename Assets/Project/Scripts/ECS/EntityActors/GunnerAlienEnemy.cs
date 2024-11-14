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
            Health.Die += Die;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
        }

        private void Die()
        {
            Health.IsSpawnedDamageText -= TextService.OnChangedFloatingText;
            ExperiencePoints.OnKill(this);
            gameObject.SetActive(false);
        }

        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
