using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Scripts.Audio;
using Project.Scripts.Audio.Sounds;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Project.Scripts.Services
{
    public class AudioSoundsService : MonoBehaviour
    {
        private const string GunSoundPath = "GunSound";
        private const string ButtonSoundPath = "ButtonSound";
        private const string MinesSoundPath = "MinesSound";
        private const string MiningStoneSoundPath = "MiningStoneSound";
        private const string FourBarrelMachineGunSoundPath = "FourBarrelMachineGunSound";
        private const string MachineGunSoundPath = "MachineGunSound";
        private const string CardViewButtonSoundPath = "CardViewButtonSound";
        private const string ChainLightningGunSoundPath = "ChainLightningGunSound";
        private const string CapsuleFlightSoundPath = "CapsuleFlightSound";
        private const string CapsuleExplosionSoundPath = "CapsuleExplosionSound";
        private const string GrenadesSoundPath = "GrenadesSound";
        private const string MainMenuMusicPath = "MainMenuMusic";
        private const string MarsGameplayMusicPath = "MarsGameplayMusic";
        private const string MysteryPlanetGameplayMusicPath = "MysteryPlanetGameplayMusic";

        private const int CountAudioSources = 3;

        private const float MinValue = 0f;
        private const float FadeDuration = 2f;
        private const float CapsuleFlightDuration = 4.5f;
        private const float CapsuleExplosionDelay = 2.5f;

        [SerializeField] private AudioMixerGroup _musicGroup;
        [SerializeField] private AudioMixerGroup _effectsGroup;

        private AudioSource _musicAudioSource;
        private SoundsType _currentMusicType;

        private Dictionary<SoundsType, Sound> _soundDictionary;
        private Queue<AudioSource> _availableAudioSources;
        private List<AudioSource> _allAudioSources;
        private IResourceService _resourceService;
        private bool _isInitialized;

        [Inject]
        private void Construct(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        public async UniTask Init()
        {
            await InitializeSoundDictionary();
            InitializeMusicAudioSource();
            InitializeAudioSourcePool();
            _isInitialized = true;
        }

        public async void PlaySound(SoundsType sound)
        {
            if (!_isInitialized) return;

            if (!_soundDictionary.ContainsKey(sound)) return;

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
                await PlaySoundAsync(audioSource, config);
        }

        public void PlayMusic(SoundsType musicType)
        {
            if (!_isInitialized) return;

            if (_currentMusicType == musicType && _musicAudioSource.isPlaying) return;

            if (!_soundDictionary.ContainsKey(musicType)) return;

            var musicConfig = _soundDictionary[musicType];

            StopCurrentMusic();

            _musicAudioSource.clip = musicConfig.Clip;
            _musicAudioSource.volume = musicConfig.Volume;
            _musicAudioSource.loop = true;
            _musicAudioSource.Play();

            _currentMusicType = musicType;
        }

        public void StopCurrentMusic()
        {
            if (_musicAudioSource == null || !_musicAudioSource.isPlaying)
                return;

            _musicAudioSource.Stop();
            _musicAudioSource.clip = null;
            _currentMusicType = SoundsType.None;
        }

        public void CrossFadeMusic(SoundsType newMusicType, float fadeDuration = FadeDuration)
        {
            if (!_isInitialized) return;

            StartCoroutine(CrossFadeMusicCoroutine(newMusicType, fadeDuration));
        }

        public void PauseMusic()
        {
            if (_musicAudioSource != null && _musicAudioSource.isPlaying)
            {
                _musicAudioSource.Pause();
            }
        }

        public void ResumeMusic()
        {
            if (_musicAudioSource != null && !_musicAudioSource.isPlaying)
            {
                _musicAudioSource.Play();
            }
        }

        public void StopSound(AudioSource audioSource)
        {
            if (audioSource == null) return;

            if (!audioSource.isPlaying) return;

            audioSource.Stop();
            _availableAudioSources.Enqueue(audioSource);
        }

        public void StopAllSounds()
        {
            foreach (var audioSource in _allAudioSources)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                    if (!_availableAudioSources.Contains(audioSource))
                    {
                        _availableAudioSources.Enqueue(audioSource);
                    }
                }
            }
        }

        private async UniTask HandleCapsuleSoundSequence(AudioSource audioSource, Sound soundConfig)
        {
            PlaySoundAsync(audioSource, soundConfig).Forget();
            
            PlayDelayedSound(SoundsType.CapsuleExplosion, CapsuleExplosionDelay).Forget();

            await UniTask.WaitForSeconds(CapsuleFlightDuration);

            StopSound(audioSource);
        }

        private async UniTask PlayDelayedSound(SoundsType soundType, float delay)
        {
            await UniTask.WaitForSeconds(delay);
            PlaySound(soundType);
        }

        private IEnumerator CrossFadeMusicCoroutine(SoundsType newMusicType, float fadeDuration)
        {
            var oldSource = _musicAudioSource;

            var oldMusicConfig = _soundDictionary[_currentMusicType];

            var newSource = gameObject.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.loop = true;

            if (!_soundDictionary.ContainsKey(newMusicType)) yield break;

            var newMusicConfig = _soundDictionary[newMusicType];
            newSource.clip = newMusicConfig.Clip;
            newSource.volume = MinValue;
            newSource.Play();

            float timer = MinValue;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float progress = timer / fadeDuration;

                if (oldSource != null && oldSource.isPlaying)
                {
                    oldSource.volume = Mathf.Lerp(oldMusicConfig.Volume, MinValue, progress);
                }

                newSource.volume = Mathf.Lerp(MinValue, newMusicConfig.Volume, progress);

                yield return null;
            }

            if (oldSource != null)
            {
                oldSource.Stop();
                Destroy(oldSource);
            }

            _musicAudioSource = newSource;
            _currentMusicType = newMusicType;
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
            var builder = new AudioSoundBuilder(_resourceService)
                .AddSound(SoundsType.Gun, GunSoundPath)
                .AddSound(SoundsType.Button, ButtonSoundPath)
                .AddSound(SoundsType.Mines, MinesSoundPath)
                .AddSound(SoundsType.Stone, MiningStoneSoundPath)
                .AddSound(SoundsType.MachineGun, MachineGunSoundPath)
                .AddSound(SoundsType.CardViewButton, CardViewButtonSoundPath)
                .AddSound(SoundsType.FourBarrelMachineGun, FourBarrelMachineGunSoundPath)
                .AddSound(SoundsType.ChainLightningGun, ChainLightningGunSoundPath)
                .AddSound(SoundsType.CapsuleFlight, CapsuleFlightSoundPath)
                .AddSound(SoundsType.CapsuleExplosion, CapsuleExplosionSoundPath)
                .AddSound(SoundsType.FragGrenades, GrenadesSoundPath)
                .AddSound(SoundsType.MainMenuMusic, MainMenuMusicPath)
                .AddSound(SoundsType.MarsGameplayMusic, MarsGameplayMusicPath)
                .AddSound(SoundsType.MysteryPlanetGameplayMusic, MysteryPlanetGameplayMusicPath);

            _soundDictionary = await builder.Build();
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