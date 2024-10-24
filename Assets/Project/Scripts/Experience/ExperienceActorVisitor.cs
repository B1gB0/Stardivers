using Build.Game.Scripts.ECS.EntityActors;

public class ExperienceActorVisitor : IActorVisitor
{
    public int AccumulatedExperience { get; private set; }

    public void Visit(SmallAlienEnemyActor smallAlienEnemy)
    {
        AccumulatedExperience += 15;
    }

    public void Visit(StoneActor stone)
    {
        AccumulatedExperience += 5;
    }

    public void UpdateAccumulatedExperience(int newValue)
    {
        AccumulatedExperience = newValue;
    }
}
