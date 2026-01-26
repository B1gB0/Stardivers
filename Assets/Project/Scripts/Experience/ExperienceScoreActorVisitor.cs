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

        public void Visit(IExperienceScoreActor experienceScoreActor)
        {
            AccumulatedExperience += experienceScoreActor.Experience;
            AccumulatedScore += experienceScoreActor.Score;
            YG2.saves.AcumulatedScore += experienceScoreActor.Score;
        
            if (experienceScoreActor.IsEnemy)
            {
                AccumulatedEnemyKills++;
            }
        }

#if UNITY_EDITOR
        public void Visit(CheatPanel cheatPanel)
        {
            AccumulatedExperience += cheatPanel.ExpValue;
        }
#endif
    }
}