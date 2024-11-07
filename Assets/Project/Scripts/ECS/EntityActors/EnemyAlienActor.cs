using Project.Scripts.Experience;
using Project.Scripts.Services;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.EntityActors
{
    public abstract class EnemyAlienActor : MonoBehaviour
    {
        [field: SerializeField] public Health.Health Health{ get; private set; }
        
        [field: SerializeField] public Animator Animator { get; private set; }

        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        
        protected ExperiencePoints ExperiencePoints;
        protected FloatingTextService TextService;

        public void Construct(ExperiencePoints experiencePoints, FloatingTextService textService)
        {
            ExperiencePoints = experiencePoints;
            
            TextService = textService;
            Health.IsSpawnedDamageText += TextService.OnChangedFloatingText;
        }
    }
}