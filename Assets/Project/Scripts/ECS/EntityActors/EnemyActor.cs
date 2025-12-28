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
        protected AudioSoundsService AudioSoundsService;

        private EcsEntity _enemyEntity;

        public EnemyData Data { get; private set; }

        public event Action<EnemyActor> Die;

        public void Construct(ExperiencePoints experiencePoints, IFloatingTextService textService, EnemyData data,
            EcsEntity enemyEntity, ParticleEffectsService particleEffectsService, AudioSoundsService audioSoundsService)
        {
            ExperiencePoints = experiencePoints;
            Data = data;
            _enemyEntity = enemyEntity;
            ParticleEffectsService = particleEffectsService;
            AudioSoundsService = audioSoundsService;
            
            TextService = textService;
            
            Health.IsSpawnedDamageText += TextService.OnChangedFloatingText;
            OnChangeSpeed += UpdateCurrentSpeed;
        }

        private void OnDestroy()
        {
            OnChangeSpeed -= UpdateCurrentSpeed;
        }
        
        protected virtual void OnDie()
        {
            ResetModifiers();
            Health.IsSpawnedDamageText -= TextService.OnChangedFloatingText;
            OnChangeSpeed -= UpdateCurrentSpeed;
            Die?.Invoke(this);
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
            ref var movableComponent = ref _enemyEntity.Get<EnemyMovableComponent>();

            if(movableComponent.NavMeshAgent == null)
                return;
            
            movableComponent.NavMeshAgent.speed = moveSpeed;
        }
    }
}