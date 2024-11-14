using Project.Scripts.Experience;

namespace Project.Scripts.ECS.EntityActors
{
    public interface IAcceptable
    {
        void AcceptScore(IScoreActorVisitor visitor) { }
    }
}
