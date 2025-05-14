using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Experience
{
    public interface IScoreActorVisitor
    {
        void Visit(SmallAlienEnemy smallAlienEnemy);
    
        void Visit(BigAlienEnemy bigAlienEnemy);

        void Visit(GunnerAlienEnemy gunnerAlienEnemy);

        void Visit(StoneActor stone);
    
        void Visit(HealingCore healingCore);
    
        void Visit(GoldCore goldCore);
    }
}
