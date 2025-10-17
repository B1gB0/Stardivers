using Project.Scripts.Experience;

namespace Project.Scripts.ECS.EntityActors
{
    public interface IAcceptable
    {
        public void AcceptScore(IScoreActorVisitor visitor) { }
    }
}
