using Project.Scripts.Experience;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public abstract class ResourceActor : MonoBehaviour
    {
        [field: SerializeField] public Health.Health Health{ get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        
        protected ExperiencePoints ExperiencePoints;

        public void GetExperiencePoints(ExperiencePoints experiencePoints)
        {
            ExperiencePoints = experiencePoints;
        }
    }
}