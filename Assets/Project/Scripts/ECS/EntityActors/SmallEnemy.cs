using Project.Scripts.Experience;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.EntityActors
{
    public class SmallEnemy : EnemyActor, IAcceptable, IFreezable
    {
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