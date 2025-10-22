using Project.Scripts.Experience;
using Project.Scripts.Weapon.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.EntityActors
{
    public class GunnerEnemy : EnemyActor, IAcceptable, IFreezable
    {
        [field: SerializeField] public GunnerEnemyAlienWeapon Weapon { get; private set; }
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        
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
        
        public void SetSpeed(float speed)
        {
            NavMeshAgent.speed += speed;
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
