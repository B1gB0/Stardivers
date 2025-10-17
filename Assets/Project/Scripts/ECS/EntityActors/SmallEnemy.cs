using Project.Scripts.Experience;

namespace Project.Scripts.ECS.EntityActors
{
    public class SmallEnemy : EnemyActor, IAcceptable
    {
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