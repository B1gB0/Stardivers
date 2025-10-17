using Project.Scripts.ECS.EntityActors;
using YG;

namespace Project.Scripts.Experience
{
    public class ExperienceScoreActorVisitor : IScoreActorVisitor
    {
        public int AccumulatedExperience { get; private set; }

        public void Visit(SmallEnemy smallEnemy)
        {
            AccumulatedExperience += smallEnemy.Data.Experience;
            YG2.saves.AcumulatedScore += smallEnemy.Data.Score;
        }

        public void Visit(BigEnemy bigEnemy)
        {
            AccumulatedExperience += bigEnemy.Data.Experience;
            YG2.saves.AcumulatedScore += bigEnemy.Data.Score;
        }

        public void Visit(GunnerEnemy gunnerEnemy)
        {
            AccumulatedExperience += gunnerEnemy.Data.Experience;
            YG2.saves.AcumulatedScore += gunnerEnemy.Data.Score;
        }

        public void Visit(EnemyTurret enemyTurret)
        {
            AccumulatedExperience += enemyTurret.Data.Experience;
            YG2.saves.AcumulatedScore += enemyTurret.Data.Score;
        }

        public void Visit(StoneActor stone)
        {
            AccumulatedExperience += stone.Data.Experience;
            YG2.saves.AcumulatedScore += stone.Data.Score;
        }

        public void Visit(HealingCore healingCore)
        {
            AccumulatedExperience += healingCore.Data.Experience;
            YG2.saves.AcumulatedScore += healingCore.Data.Score;
        }

        public void Visit(GoldCore goldCore)
        {
            AccumulatedExperience += goldCore.Data.Experience;
            YG2.saves.AcumulatedScore += goldCore.Data.Score;
        }

        public void UpdateAccumulatedExperience(int newValue)
        {
            AccumulatedExperience = newValue;
        }
    }
}
