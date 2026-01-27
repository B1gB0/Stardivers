#if UNITY_EDITOR

using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Experience;
using Project.Scripts.Services;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Panel
{
    public class CheatPanel : View.View, IAcceptable
    {
        private const int _goldValue = 50;
        private const int _healthValue = 60;
        private const int _expValue = 100;

        [SerializeField] private Button _addGold;
        [SerializeField] private Button _addHealth;
        [SerializeField] private Button _addExp;
        [SerializeField] private Button _exitButton;

        private ICurrencyService _currencyService;
        private IPlayerService _playerService;
        private ExperiencePoints _experiencePoints;

        public int ExpValue => _expValue;

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
            _addExp.onClick.AddListener(OnAddExpButtonClicked);
            _exitButton.onClick.AddListener(Deactivate);
        }

        private void OnDisable()
        {
            _addGold.onClick.RemoveListener(OnAddGoldButtonClicked);
            _addHealth.onClick.RemoveListener(OnAddHealthButtonClicked);
            _addExp.onClick.RemoveListener(OnAddExpButtonClicked);
            _exitButton.onClick.RemoveListener(Deactivate);
        }

        public void GetServices(ExperiencePoints experiencePoints)
        {
            _experiencePoints = experiencePoints;
        }

        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void OnAddGoldButtonClicked()
        {
            _currencyService.AddGold(_goldValue);
        }

        private void OnAddHealthButtonClicked()
        {
            _playerService.PlayerActor.Health.AddHealth(_healthValue);
        }

        private void OnAddExpButtonClicked()
        {
            _experiencePoints.OnKill(this);
        }
    }
}
#endif