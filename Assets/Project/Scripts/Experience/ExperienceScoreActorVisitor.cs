using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Experience
{
    public class ExperienceScoreActorVisitor : IScoreActorVisitor
    {
        public int AccumulatedExperience { get; private set; }

        public void Visit(SmallAlienEnemy smallAlienEnemy)
        {
            AccumulatedExperience += 10;
        }

        public void Visit(BigAlienEnemyAlien bigAlienEnemy)
        {
            AccumulatedExperience += 25;
        }

        public void Visit(StoneActor stone)
        {
            AccumulatedExperience += 3;
        }

        public void Visit(HealingCore healingCore)
        {
            AccumulatedExperience += 5;
        }

        public void Visit(GoldCore goldCore)
        {
            AccumulatedExperience += 7;
        }

        public void UpdateAccumulatedExperience(int newValue)
        {
            AccumulatedExperience = newValue;
        }
    }
}
