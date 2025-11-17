using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Services;

namespace Project.Scripts.ParticleEffects
{
    public class ParticleEffectBuilder : GenericScriptableObjectBuilder<ParticleEffectType, ParticleEffect>
    {
        private const string EffectBuilder = nameof(EffectBuilder);

        public ParticleEffectBuilder(IResourceService resourceService) 
            : base(resourceService, EffectBuilder) { }
    }
}