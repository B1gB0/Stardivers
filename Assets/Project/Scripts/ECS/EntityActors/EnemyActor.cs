using System;
using Leopotam.Ecs;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.Components;
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

        private EcsEntity _enemyEntity;
        
        public EnemyData Data { get; private set; }

        public event Action<EnemyActor> Die;

        public void Construct(ExperiencePoints experiencePoints, IFloatingTextService textService, EnemyData data)
        {
            ExperiencePoints = experiencePoints;
            Data = data;
            
            TextService = textService;
            Health.IsSpawnedDamageText += TextService.OnChangedFloatingText;
        }
        
        public void ChangeMoveSpeed(float moveSpeed)
        {
            ref var movableComponent = ref _enemyEntity.Get<EnemyMovableComponent>();
            movableComponent.MoveSpeed = moveSpeed;
        }

        protected virtual void OnDie()
        {
            Die?.Invoke(this);
        }
    }
}