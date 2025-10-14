using System;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Project.Scripts.UI.Panel
{
    public class SettingsPanel : MonoBehaviour, IView
    {
        private const string MusicVolume = nameof(MusicVolume);
        private const string EffectsVolume = nameof(EffectsVolume);

        private const float StartValueSlider = 0.8f;
        private const float MinValueSlider = 0f;

        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _backToSceneButton;

        [SerializeField] private AudioMixerGroup _mixer;

        [SerializeField] private float _minVolume = -80f;
        [SerializeField] private float _maxVolume;

        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _effectsVolumeSlider;

        private ITweenAnimationService _tweenAnimationService;

        public event Action OnBackToSceneButtonPressed;
        
        [Inject]
        private void Construct(ITweenAnimationService tweenAnimationService)
        {
            _tweenAnimationService = tweenAnimationService;
        }

        private void Start()
        {
            SetValuesVolume();
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _backToSceneButton.onClick.AddListener(MoveBackToScene);
            _settingsButton.gameObject.SetActive(false);

            _musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
            _effectsVolumeSlider.onValueChanged.AddListener(ChangeEffectsVolume);
        }

        private void OnDisable()
        {
            _backToSceneButton.onClick.RemoveListener(MoveBackToScene);
            _settingsButton.gameObject.SetActive(true);

            _musicVolumeSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
            _effectsVolumeSlider.onValueChanged.RemoveListener(ChangeEffectsVolume);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _tweenAnimationService.AnimateScale(transform);
        }

        public void Hide()
        {
            _tweenAnimationService.AnimateScale(transform, true);
        }

        private void SetValuesVolume()
        {
            _musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolume);
            _effectsVolumeSlider.value = PlayerPrefs.GetFloat(EffectsVolume);

            if (PlayerPrefs.GetFloat(MusicVolume) != MinValueSlider ||
                PlayerPrefs.GetFloat(EffectsVolume) != MinValueSlider)
                return;
            
            _musicVolumeSlider.value = StartValueSlider;
            _effectsVolumeSlider.value = StartValueSlider;
            
            ChangeMusicVolume(StartValueSlider);
            ChangeEffectsVolume(StartValueSlider);
        }

        private void MoveBackToScene()
        {
            OnBackToSceneButtonPressed?.Invoke();
        }

        private void ChangeMusicVolume(float volume)
        {
            _mixer.audioMixer.SetFloat(MusicVolume, Mathf.Lerp(_minVolume, _maxVolume, volume));

            PlayerPrefs.SetFloat(MusicVolume, volume);
        }

        private void ChangeEffectsVolume(float volume)
        {
            _mixer.audioMixer.SetFloat(EffectsVolume, Mathf.Lerp(_minVolume, _maxVolume, volume));

            PlayerPrefs.SetFloat(EffectsVolume, volume);
        }
    }
}