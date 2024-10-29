using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Project.Scripts
{
    public class Timer : MonoBehaviour, IView
    {
        private const int LastSecondOfMinute = 59;
        private const int SecondsInMinute = 60;
        private const int MinValue = 0;
        
        private const float Delay = 1f;

        [field: SerializeField] public int _timeInSeconds { get; private set; }
        
        [SerializeField] private TMP_Text _text;

        private Coroutine _coroutine;
        private int _minutes;
        private int _seconds;

        private void Awake()
        {
            _minutes = _timeInSeconds / SecondsInMinute;
            _seconds = _timeInSeconds;

            if (_minutes != MinValue)
            {
                _seconds -= _minutes * SecondsInMinute;
            }

            DisplayCountdown();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnLaunchTimer()
        {
            _coroutine = StartCoroutine(LaunchTimer());
        }
        
        public void OffLaunchTimer()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private void DisplayCountdown()
        {
            if (_seconds <= 9)
            {
                _text.text = "0" + _minutes + " : 0" + _seconds;
            }
            else
            {
                _text.text = "0" + _minutes + " : " + _seconds;
            }
        }

        private IEnumerator LaunchTimer()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(Delay);
        
            while (enabled)
            {
                _timeInSeconds--;

                if (_seconds == MinValue)
                {
                    _seconds = LastSecondOfMinute;
                    _minutes--;
                    
                    DisplayCountdown();
                    
                    yield return waitForSeconds;
                }
                
                _seconds--;

                DisplayCountdown();

                yield return waitForSeconds;
            }
        }
    }
}
