using Build.Game.Scripts;
using Project.Scripts.Experience;
using Project.Scripts.Score;
using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.EntityActors
{
    public abstract class EnemyAlienActor : MonoBehaviour
    {
        [field: SerializeField] public Health Health{ get; private set; }
        
        [field: SerializeField] public Animator Animator { get; private set; }

        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        
        protected ExperiencePoints ExperiencePoints;
        protected FloatingDamageTextService DamageTextService;

        public void Construct(ExperiencePoints experiencePoints, FloatingDamageTextService damageTextService)
        {
            ExperiencePoints = experiencePoints;
            
            DamageTextService = damageTextService;
            Health.IsSpawnedDamageText += DamageTextService.OnChangedDamageText;
        }
    }
}