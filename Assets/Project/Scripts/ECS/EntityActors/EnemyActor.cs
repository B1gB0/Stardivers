using System;
using Leopotam.Ecs;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.Components;
using Project.Scripts.Experience;
using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Services;

namespace Project.Scripts.ECS.EntityActors
{
    public abstract class EnemyActor : EntityActor
    {
        private const float MoveSpeedFactor = 1f;
        
        protected ExperiencePoints ExperiencePoints;
        protected IFloatingTextService TextService;
        protected ParticleEffectsService ParticleEffectsService;

        protected EcsEntity EnemyEntity;

        public EnemyData Data { get; private set; }

        public event Action<EnemyActor> Die;

        public void Construct(ExperiencePoints experiencePoints, IFloatingTextService textService, EnemyData data,
            EcsEntity enemyEntity, ParticleEffectsService particleEffectsService)
        {
            ExperiencePoints = experiencePoints;
            Data = data;
            EnemyEntity = enemyEntity;
            ParticleEffectsService = particleEffectsService;
            
            TextService = textService;
            
            Health.IsSpawnedDamageText += TextService.OnChangedFloatingText;
            OnChangeSpeed += UpdateCurrentSpeed;
        }

        protected virtual void OnDie()
        {
            ResetModifiers();
            Health.IsSpawnedDamageText -= TextService.OnChangedFloatingText;
            OnChangeSpeed -= UpdateCurrentSpeed;
            Die?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnChangeSpeed -= UpdateCurrentSpeed;
        }
        
        protected virtual void OnPlayParticleEffect()
        {
            ParticleEffectsService.PlayEffect(ParticleEffectType.EnemyHit, Health.HitPoint.position);
        }
        
        private void UpdateCurrentSpeed()
        {
            var moveSpeed = Data.Speed * (MoveSpeedFactor + GetCurrentModifier());
            ChangeMoveSpeed(moveSpeed);
        }
        
        private void ChangeMoveSpeed(float moveSpeed)
        {
            ref var movableComponent = ref EnemyEntity.Get<EnemyMovableComponent>();
            movableComponent.MoveSpeed = moveSpeed;
            movableComponent.NavMeshAgent.speed = moveSpeed;
        }
    }
}