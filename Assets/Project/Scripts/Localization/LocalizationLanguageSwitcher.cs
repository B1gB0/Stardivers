using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.Localization
{
    public class LocalizationLanguageSwitcher : MonoBehaviour
    {
        private readonly List<string> _languages = new () {"en", "ru", "tr"};

        [SerializeField] private Button _priviousButton;
        [SerializeField] private Button _nextButton;

        private int _currentIndex;

        public event Action<string> OnLanguageChanged;

        private void OnEnable()
        {
            _priviousButton.onClick.AddListener(SetPreviousLanguage);
            _nextButton.onClick.AddListener(SetNextLanguage);
        }

        private void OnDisable()
        {
            _priviousButton.onClick.RemoveListener(SetPreviousLanguage);
            _nextButton.onClick.RemoveListener(SetNextLanguage);
        }

        private void SetNextLanguage()
        {
            if (_currentIndex == _languages.Count - 1)
                _currentIndex = 0;
            else
                _currentIndex++;
        
            SetLanguage(_currentIndex);
        }

        private void SetPreviousLanguage()
        {
            if (_currentIndex == 0)
                _currentIndex = _languages.Count - 1;
            else
                _currentIndex--;
        
            SetLanguage(_currentIndex);
        }
    
        private void SetLanguage(int index)
        {
            YandexGame.SwitchLanguage(_languages[index]);
            OnLanguageChanged?.Invoke(_languages[index]);
        }
    }
}
