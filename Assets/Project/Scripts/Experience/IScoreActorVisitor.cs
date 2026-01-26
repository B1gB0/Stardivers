using Project.Scripts.ECS.EntityActors;
using Project.Scripts.UI.Panel;

namespace Project.Scripts.Experience
{
    public interface IScoreActorVisitor
    {
        public void Visit(IExperienceScoreActor experienceScoreActor);
        public void Visit(CheatPanel cheatPanel);
    }
}
