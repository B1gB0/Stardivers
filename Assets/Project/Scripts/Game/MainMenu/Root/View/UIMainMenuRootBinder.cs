using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIMainMenuRootBinder : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private AudioSource _playButtonSound;

    private Subject<Unit> _exitSceneSubjectSignal;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(HandleGoToGameplayButtonClick);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(HandleGoToGameplayButtonClick);
    }

    public void HandleGoToGameplayButtonClick()
    {
        _playButtonSound.PlayOneShot(_playButtonSound.clip);
        _exitSceneSubjectSignal?.OnNext(Unit.Default);
    }

    public void Bind(Subject<Unit> exitSceneSignalSubject)
    {
        _exitSceneSubjectSignal = exitSceneSignalSubject;
    }
}
 