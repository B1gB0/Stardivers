namespace Project.Scripts.ECS.EntityActors
{
    public interface IExperienceScoreActor
    {
        public int Experience { get; }
        public int Score { get; }
        public bool IsEnemy { get; }
    }
}