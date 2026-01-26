using Project.Scripts.DataBase.Data;
using Project.Scripts.Experience;
using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public abstract class ResourceActor : MonoBehaviour, IExperienceScoreActor
    {
        protected ExperiencePoints ExperiencePoints;
        protected ParticleEffectsService ParticleEffectsService;

        [field: SerializeField] public Health.Health Health { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }

        public CoreData Data { get; private set; }
        public int Experience { get; private set; }
        public int Score { get; private set; }
        public bool IsEnemy { get; private set; }

        public void Construct(
            ExperiencePoints experiencePoints,
            CoreData data,
            ParticleEffectsService particleEffectsService)
        {
            ExperiencePoints = experiencePoints;
            
            Data = data;
            Experience = data.Experience;
            Score = data.Score;
            IsEnemy = false;
            
            ParticleEffectsService = particleEffectsService;
        }

        protected virtual void OnPlayParticleEffect()
        {
            ParticleEffectsService.PlayEffect(ParticleEffectType.StoneHit, Health.HitPoint.position);
        }
    }
}