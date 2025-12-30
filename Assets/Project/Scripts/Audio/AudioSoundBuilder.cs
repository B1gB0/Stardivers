using Project.Scripts.Audio.Sounds;
using Project.Scripts.Services;

namespace Project.Scripts.Audio
{
    public class AudioSoundBuilder : GenericScriptableObjectBuilder<SoundsType, Sound>
    {
        private const string SoundBuilder = nameof(SoundBuilder);

        public AudioSoundBuilder(IResourceService resourceService) : base(resourceService, SoundBuilder) { }
    }
}