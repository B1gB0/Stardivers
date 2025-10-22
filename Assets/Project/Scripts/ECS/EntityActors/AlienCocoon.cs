using Project.Scripts.Experience;

namespace Project.Scripts.ECS.EntityActors
{
    public class AlienCocoon : ResourceActor, IAcceptable
    {
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
            ExperiencePoints.OnKill(this);
            gameObject.SetActive(false);
        }

        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}