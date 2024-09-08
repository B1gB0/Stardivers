using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationLanguageSwitcher : MonoBehaviour
{
    private readonly List<string> _languages = new List<string>() {"English", "Russian", "Turkey"};

    [SerializeField] private Button _priviousButton;
    [SerializeField] private Button _nextButton;

    private int _currentIndex = 0;

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
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll(_languages[index]);
    }
}
