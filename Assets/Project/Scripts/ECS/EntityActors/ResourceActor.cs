using Project.Scripts.DataBase.Data;
using Project.Scripts.Experience;
using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public abstract class ResourceActor : MonoBehaviour
    {
        [field: SerializeField] public Health.Health Health{ get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        
        protected ExperiencePoints ExperiencePoints;
        protected ParticleEffectsService ParticleEffectsService;

        public CoreData Data { get; private set; }

        public void Construct(ExperiencePoints experiencePoints, CoreData data,
            ParticleEffectsService particleEffectsService)
        {
            ExperiencePoints = experiencePoints;
            Data = data;
            ParticleEffectsService = particleEffectsService;
        }

        protected virtual void OnPlayParticleEffect()
        {
            ParticleEffectsService.PlayEffect(ParticleEffectType.StoneHit, Health.HitPoint.position);
        }
    }
}