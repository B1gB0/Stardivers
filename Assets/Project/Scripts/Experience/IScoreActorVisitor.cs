using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Experience
{
    public interface IScoreActorVisitor
    {
        public void Visit(SmallEnemy smallEnemy);
        public void Visit(BigEnemy bigEnemy);
        public void Visit(GunnerEnemy gunnerEnemy);
        public void Visit(EnemyTurret enemyTurret);
        public void Visit(StoneActor stone);
        public void Visit(HealingCore healingCore);
        public void Visit(GoldCore goldCore);
    }
}
