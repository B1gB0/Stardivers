using Build.Game.Scripts;
using Build.Game.Scripts.ECS.EntityActors;

public interface IActorVisitor
{
    void Visit(SmallAlienEnemyActor smallAlienEnemy);
    
    void Visit(StoneActor stone);
}
