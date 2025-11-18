using Project.Scripts.Experience;

namespace Project.Scripts.ECS.EntityActors
{
    public class StoneActor : ResourceActor, IAcceptable
    {
        private void OnEnable()
        {
            Health.Die += Die;
            Health.IsDamaged += OnPlayParticleEffect;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
            Health.IsDamaged -= OnPlayParticleEffect;
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