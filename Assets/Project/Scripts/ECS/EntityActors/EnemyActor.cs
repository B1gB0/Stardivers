using System;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Experience;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public abstract class EnemyActor : MonoBehaviour
    {
        [field: SerializeField] public Health.Health Health{ get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }

        protected ExperiencePoints ExperiencePoints;
        protected IFloatingTextService TextService;
        
        public EnemyData Data { get; private set; }

        public event Action<EnemyActor> Die;

        public void Construct(ExperiencePoints experiencePoints, IFloatingTextService textService, EnemyData data)
        {
            ExperiencePoints = experiencePoints;
            Data = data;
            
            TextService = textService;
            Health.IsSpawnedDamageText += TextService.OnChangedFloatingText;
        }

        protected virtual void OnDie()
        {
            Die?.Invoke(this);
        }
    }
}