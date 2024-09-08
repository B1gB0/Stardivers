using Build.Game.Scripts;
using Build.Game.Scripts.ECS.EntityActors;

public interface IActorVisitor
{
    void Visit(EnemyActor enemy);
    
    void Visit(StoneActor stone);
}
