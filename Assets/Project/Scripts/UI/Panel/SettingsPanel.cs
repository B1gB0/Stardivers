using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Scripts.UI
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

        private PauseService _pauseService;

        private void OnEnable()
        {
            _backToSceneButton.onClick.AddListener(Hide);
            _backToSceneButton.onClick.AddListener(PlayGame);
            _settingsButton.gameObject.SetActive(false);

            _musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
            _effectsVolumeSlider.onValueChanged.AddListener(ChangeEffectsVolume);
        }

        private void OnDisable()
        {
            _backToSceneButton.onClick.RemoveListener(Hide);
            _backToSceneButton.onClick.RemoveListener(PlayGame);
            _settingsButton.gameObject.SetActive(true);

            _musicVolumeSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
            _effectsVolumeSlider.onValueChanged.RemoveListener(ChangeEffectsVolume);
        }

        public void GetPauseService(PauseService pauseService)
        {
            _pauseService = pauseService;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetValuesVolume()
        {
            _musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolume);
            _effectsVolumeSlider.value = PlayerPrefs.GetFloat(EffectsVolume);

            if (PlayerPrefs.GetFloat(MusicVolume) == MinValueSlider &&
                PlayerPrefs.GetFloat(EffectsVolume) == MinValueSlider)
            {
                _musicVolumeSlider.value = StartValueSlider;
                _effectsVolumeSlider.value = StartValueSlider;
            }
        }
        
        public void StopGame()
        {
            if(SceneManager.GetActiveScene().name == Scenes.Gameplay)
                _pauseService.StopGame();
        }
        
        private void PlayGame()
        {
            if(SceneManager.GetActiveScene().name == Scenes.Gameplay)
                _pauseService.PlayGame();
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
