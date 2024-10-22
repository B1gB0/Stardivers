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

        [field: SerializeField] public int _seconds { get; private set; }
        
        [SerializeField] private TMP_Text _text;

        private Coroutine _coroutine;
        private int _minutes;

        private void Awake()
        {
            _minutes = _seconds / SecondsInMinute;

            if (_minutes != MinValue)
            {
                _seconds /= _minutes;
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
                _seconds--;

                if (_seconds == LastSecondOfMinute)
                {
                    _minutes--;
                }

                DisplayCountdown();

                yield return waitForSeconds;
            }
        }
    }
}
