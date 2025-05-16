using System;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Panel
{
    [RequireComponent(typeof(Button))]
    public class AdviserMessagePanel : MonoBehaviour, IView
    {
        [SerializeField] private Text _text;

        private IPauseService _pauseService;
        
        public Button ContinueButton { get; private set; }
        
        [Inject]
        private void Construct(IPauseService pauseService)
        {
            _pauseService = pauseService;
        }

        private void Awake()
        {
            ContinueButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            ContinueButton.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            ContinueButton.onClick.RemoveListener(Hide);
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void Show()
        {
            _pauseService.StopGame();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _pauseService.PlayGame();
            gameObject.SetActive(false);
        }
    }
}
