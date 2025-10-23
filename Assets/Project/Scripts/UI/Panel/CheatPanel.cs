#if UNITY_EDITOR
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Panel
{
    public class CheatPanel : MonoBehaviour, IView
    {
        private const int _goldValue = 50;
        private const int _healthValue = 60;
        
        [SerializeField] private Button _addGold;
        [SerializeField] private Button _addHealth;
        [SerializeField] private Button _exitButton;
        
        private ICurrencyService _currencyService;
        private IPlayerService _playerService;
        
        [Inject]
        private void Construct(ICurrencyService currencyService, IPlayerService playerService)
        {
            _currencyService = currencyService;
            _playerService = playerService;
        }

        private void OnEnable()
        {
            _addGold.onClick.AddListener(OnAddGoldButtonClicked);
            _addHealth.onClick.AddListener(OnAddHealthButtonClicked);
            _exitButton.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            _addGold.onClick.RemoveListener(OnAddGoldButtonClicked);
            _addHealth.onClick.RemoveListener(OnAddHealthButtonClicked);
            _exitButton.onClick.RemoveListener(Hide);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnAddGoldButtonClicked()
        {
            _currencyService.AddGold(_goldValue);
        }
        
        private void OnAddHealthButtonClicked()
        {
            _playerService.PlayerActor.Health.AddHealth(_healthValue);
        }
    }
}
#endif