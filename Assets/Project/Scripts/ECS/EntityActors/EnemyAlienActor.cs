using System;
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
        protected IFloatingTextService TextService;

        public event Action<EnemyAlienActor> Die;

        public void Construct(ExperiencePoints experiencePoints, IFloatingTextService textService)
        {
            ExperiencePoints = experiencePoints;
            
            TextService = textService;
            Health.IsSpawnedDamageText += TextService.OnChangedFloatingText;
        }

        public void SetSpeed(float speed)
        {
            NavMeshAgent.speed += speed;
        }

        protected virtual void OnDie()
        {
            Die?.Invoke(this);
        }
    }
}