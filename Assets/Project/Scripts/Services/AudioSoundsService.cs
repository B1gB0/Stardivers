using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Audio;
using Project.Scripts.Audio.Sounds;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Project.Scripts.Services
{
    public class AudioSoundsService : MonoBehaviour, IService
    {
        private const string AudioConfigPath = "AudioConfig";

        private const int CountAudioSources = 10;

        private const float CapsuleFlightDuration = 4.5f;
        private const float CapsuleExplosionDelay = 2.5f;

        private readonly Dictionary<SoundsType, Sound> _soundDictionary = new();
        
        [SerializeField] private AudioMixerGroup _musicGroup;
        [SerializeField] private AudioMixerGroup _effectsGroup;

        private AudioSource _musicAudioSource;
        private SoundsType _currentMusicType;
        private CancellationTokenSource _capsuleSoundToken;

        private Queue<AudioSource> _availableAudioSources;
        private List<AudioSource> _allAudioSources;
        private IResourceService _resourceService;

        public bool IsInitiated { get; private set; }

        [Inject]
        private void Construct(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        public async UniTask Init()
        {
            if (IsInitiated)
                return;
            
            await InitializeSoundDictionary();
            
            InitializeMusicAudioSource();
            InitializeAudioSourcePool();
            IsInitiated = true;
        }

        public async UniTask PlaySound(SoundsType sound)
        {
            try
            {
                if (!IsInitiated)
                    return;

                if (!_soundDictionary.ContainsKey(sound))
                    return;

                var config = _soundDictionary[sound];

                if (!_availableAudioSources.Any())
                {
                    CreateAudioSource();
                }

                var audioSource = _availableAudioSources.Dequeue();

                if (sound == SoundsType.CapsuleFlight)
                {
                    await HandleCapsuleSoundSequence(audioSource, config);
                }
                else
                {
                    await PlaySoundAsync(audioSource, config);
                }
            }
            catch (OperationCanceledException) { }
        }

        public void PlayMusic(SoundsType musicType)
        {
            if (!IsInitiated)
                return;

            if (_currentMusicType == musicType && _musicAudioSource.isPlaying)
                return;

            if (!_soundDictionary.ContainsKey(musicType))
                return;

            var musicConfig = _soundDictionary[musicType];

            StopCurrentMusic();

            _musicAudioSource.clip = musicConfig.Clip;
            _musicAudioSource.volume = musicConfig.Volume;
            _musicAudioSource.loop = true;
            _musicAudioSource.Play();

            _currentMusicType = musicType;
        }

        public void StopAllSounds()
        {
            if (_capsuleSoundToken != null)
            {
                _capsuleSoundToken?.Cancel();
                _capsuleSoundToken?.Dispose();
                _capsuleSoundToken = null;
            }

            foreach (var audioSource in _allAudioSources.Where(audioSource => audioSource.isPlaying))
            {
                audioSource.Stop();
                if (!_availableAudioSources.Contains(audioSource))
                {
                    _availableAudioSources.Enqueue(audioSource);
                }
            }
        }

        public void PauseAllSounds()
        {
            foreach (var audioSource in _allAudioSources.Where(audioSource => audioSource.isPlaying))
            {
                audioSource.Pause();
            }
        }

        public void ResumeAllSounds()
        {
            foreach (var audioSource in _allAudioSources.Where(audioSource => audioSource.isPlaying))
            {
                audioSource.Play();
            }
        }

        private void StopCurrentMusic()
        {
            if (_musicAudioSource == null || !_musicAudioSource.isPlaying)
                return;

            _musicAudioSource.Stop();
            _musicAudioSource.clip = null;
            _currentMusicType = SoundsType.None;
        }

        private void StopSound(AudioSource audioSource)
        {
            if (audioSource == null)
                return;

            if (!audioSource.isPlaying)
                return;

            audioSource.Stop();
            _availableAudioSources.Enqueue(audioSource);
        }

        private async UniTask HandleCapsuleSoundSequence(AudioSource audioSource, Sound soundConfig)
        {
            _capsuleSoundToken = new CancellationTokenSource();

            try
            {
                PlaySoundAsync(audioSource, soundConfig).Forget();

                PlayDelayedSound(SoundsType.CapsuleExplosion, CapsuleExplosionDelay, _capsuleSoundToken.Token).Forget();

                await UniTask.WaitForSeconds(CapsuleFlightDuration, cancellationToken: _capsuleSoundToken.Token);

                StopSound(audioSource);
            }
            finally
            {
                _capsuleSoundToken?.Dispose();
                _capsuleSoundToken = null;
            }
        }

        private async UniTask PlayDelayedSound(
            SoundsType soundType,
            float delay,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await UniTask.WaitForSeconds(delay, cancellationToken: cancellationToken);
                if (!cancellationToken.IsCancellationRequested)
                {
                    await PlaySound(soundType);
                }
            }
            catch (OperationCanceledException) { }
        }

        private void InitializeAudioSourcePool()
        {
            _availableAudioSources = new Queue<AudioSource>();
            _allAudioSources = new List<AudioSource>();

            for (int i = 0; i < CountAudioSources; i++)
            {
                CreateAudioSource();
            }
        }

        private void InitializeMusicAudioSource()
        {
            _musicAudioSource = gameObject.AddComponent<AudioSource>();
            _musicAudioSource.playOnAwake = false;
            _musicAudioSource.loop = true;

            if (_musicGroup != null)
            {
                _musicAudioSource.outputAudioMixerGroup = _musicGroup;
            }
        }

        private async UniTask InitializeSoundDictionary()
        {
            AudioConfig audioConfig = await _resourceService.Load<AudioConfig>(AudioConfigPath);

            foreach (var sound in audioConfig.Sounds)
            {
                AudioClip clip = await _resourceService.Load<AudioClip>(sound.ClipName);
                sound.Clip = clip;
                Enum.TryParse(sound.ClipName, out SoundsType soundType);
                _soundDictionary.TryAdd(soundType, sound);
            }
        }

        private async UniTask PlaySoundAsync(AudioSource audioSource, Sound config)
        {
            audioSource.clip = config.Clip;
            audioSource.volume = config.Volume;
            audioSource.loop = config.IsLoop;
            audioSource.Play();

            if (!config.IsLoop)
            {
                await UniTask.WaitForSeconds(config.Clip.length);
                audioSource.Stop();
                _availableAudioSources.Enqueue(audioSource);
            }
        }

        private void CreateAudioSource()
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            if (_effectsGroup != null)
            {
                audioSource.outputAudioMixerGroup = _effectsGroup;
            }

            _availableAudioSources.Enqueue(audioSource);
            _allAudioSources.Add(audioSource);
        }
    }
}