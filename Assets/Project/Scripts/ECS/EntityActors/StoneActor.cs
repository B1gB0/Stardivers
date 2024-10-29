using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Experience;
using NotImplementedException = System.NotImplementedException;

namespace Build.Game.Scripts.ECS.EntityActors
{
    public class StoneActor : ResourceActor, IAcceptable
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