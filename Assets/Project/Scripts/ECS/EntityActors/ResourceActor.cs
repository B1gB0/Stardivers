using Build.Game.Scripts;
using Project.Scripts.Experience;
using Project.Scripts.MiningResources;
using Project.Scripts.Score;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public abstract class ResourceActor : MonoBehaviour
    {
        [field: SerializeField] public Health Health{ get; private set; }

        [field: SerializeField] public Animator Animator { get; private set; }
        
        protected ExperiencePoints ExperiencePoints;

        public void GetExperiencePoints(ExperiencePoints experiencePoints)
        {
            ExperiencePoints = experiencePoints;
        }
    }
}