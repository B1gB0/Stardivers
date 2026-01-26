using System;
using Leopotam.Ecs;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.Components;
using Project.Scripts.Experience;
using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Services;

namespace Project.Scripts.ECS.EntityActors
{
    public abstract class EnemyActor : EntityActor, IExperienceScoreActor, IAcceptable
    {
        private const float MoveSpeedFactor = 1f;

        protected ExperiencePoints ExperiencePoints;
        protected IFloatingTextService TextService;
        protected ParticleEffectsService ParticleEffectsService;
        protected AudioSoundsService AudioSoundsService;

        private EcsEntity _enemyEntity;
        private EnemyData _data;

        public event Action<EnemyActor> Die;

        public int Experience { get; private set; }
        public int Score { get; private set; }
        public bool IsEnemy { get; private set; }

        private void OnDestroy()
        {
            OnChangeSpeed -= UpdateCurrentSpeed;
        }

        public void Construct(
            ExperiencePoints experiencePoints,
            IFloatingTextService textService,
            EnemyData data,
            EcsEntity enemyEntity,
            ParticleEffectsService particleEffectsService,
            AudioSoundsService audioSoundsService)
        {
            ExperiencePoints = experiencePoints;
            
            _data = data;
            Experience = data.Experience;
            Score = data.Score;
            IsEnemy = true;
            
            _enemyEntity = enemyEntity;
            ParticleEffectsService = particleEffectsService;
            AudioSoundsService = audioSoundsService;

            TextService = textService;

            Health.IsSpawnedDamageText += TextService.OnChangedFloatingText;
            OnChangeSpeed += UpdateCurrentSpeed;
        }
        
        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
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
            var moveSpeed = _data.Speed * (MoveSpeedFactor + GetCurrentModifier());
            ChangeMoveSpeed(moveSpeed);
        }

        private void ChangeMoveSpeed(float moveSpeed)
        {
            ref var movableComponent = ref _enemyEntity.Get<EnemyMovableComponent>();

            if (movableComponent.NavMeshAgent == null)
                return;

            movableComponent.NavMeshAgent.speed = moveSpeed;
        }
    }
}