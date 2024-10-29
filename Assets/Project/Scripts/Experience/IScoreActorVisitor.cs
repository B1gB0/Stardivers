using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Experience
{
    public interface IScoreActorVisitor
    {
        void Visit(SmallAlienEnemy smallAlienEnemy);
    
        void Visit(BigAlienEnemyAlien bigAlienEnemy);

        void Visit(StoneActor stone);
    
        void Visit(HealingCore healingCore);
    
        void Visit(GoldCore goldCore);
    }
}
