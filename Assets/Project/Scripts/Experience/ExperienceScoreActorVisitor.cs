using Project.Scripts.ECS.EntityActors;
using Project.Scripts.UI.Panel;
using YG;

namespace Project.Scripts.Experience
{
    public class ExperienceScoreActorVisitor : IScoreActorVisitor
    {
        private const int MinValue = 0;
        public int AccumulatedExperience { get; private set; }
        public int AccumulatedEnemyKills { get; private set; }
        public int AccumulatedScore { get; private set; }

        public void UpdateAccumulatedExperience(int newValue)
        {
            AccumulatedExperience = newValue;
        }

        public void ResetAccumulatedValues()
        {
            AccumulatedEnemyKills = MinValue;
            AccumulatedScore = MinValue;
        }

        public void Visit(SmallEnemy smallEnemy)
        {
            AccumulatedExperience += smallEnemy.Data.Experience;
            AccumulatedEnemyKills++;
            AccumulatedScore += smallEnemy.Data.Score;
            YG2.saves.AcumulatedScore += smallEnemy.Data.Score;
        }

        public void Visit(BigEnemy bigEnemy)
        {
            AccumulatedExperience += bigEnemy.Data.Experience;
            AccumulatedEnemyKills++;
            AccumulatedScore += bigEnemy.Data.Score;
            YG2.saves.AcumulatedScore += bigEnemy.Data.Score;
        }

        public void Visit(GunnerEnemy gunnerEnemy)
        {
            AccumulatedExperience += gunnerEnemy.Data.Experience;
            AccumulatedEnemyKills++;
            AccumulatedScore += gunnerEnemy.Data.Score;
            YG2.saves.AcumulatedScore += gunnerEnemy.Data.Score;
        }

        public void Visit(EnemyTurret enemyTurret)
        {
            AccumulatedExperience += enemyTurret.Data.Experience;
            AccumulatedEnemyKills++;
            AccumulatedScore += enemyTurret.Data.Score;
            YG2.saves.AcumulatedScore += enemyTurret.Data.Score;
        }

        public void Visit(StoneActor stone)
        {
            AccumulatedExperience += stone.Data.Experience;
            AccumulatedScore += stone.Data.Score;
            YG2.saves.AcumulatedScore += stone.Data.Score;
        }

        public void Visit(HealingCore healingCore)
        {
            AccumulatedExperience += healingCore.Data.Experience;
            AccumulatedScore += healingCore.Data.Score;
            YG2.saves.AcumulatedScore += healingCore.Data.Score;
        }

        public void Visit(GoldCore goldCore)
        {
            AccumulatedExperience += goldCore.Data.Experience;
            AccumulatedScore += goldCore.Data.Score;
            YG2.saves.AcumulatedScore += goldCore.Data.Score;
        }

        public void Visit(AlienCocoon alienCocoon)
        {
            AccumulatedExperience += alienCocoon.Data.Experience;
            AccumulatedScore += alienCocoon.Data.Score;
            YG2.saves.AcumulatedScore += alienCocoon.Data.Score;
        }

// #if UNITY_EDITOR
        public void Visit(CheatPanel cheatPanel)
        {
            AccumulatedExperience += cheatPanel.ExpValue;
        }
// #endif
    }
}