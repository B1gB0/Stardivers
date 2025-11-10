using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.Cards;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.Improvements;
using Project.Scripts.Weapon.Player;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Panel
{
    public class LevelUpPanel : MonoBehaviour, IView
    {
        private const int MinValue = 0;

        private const float LevelUpDelay = 0.3f;

        private readonly WeaponVisitor _weaponVisitor = new();

        [SerializeField] private List<CardView> _cardViews = new();

        [SerializeField] private Button _rollButton;
        [SerializeField] private Button _healButton;
        [SerializeField] private Button _continueButton;

        [SerializeField] private GameObject _priceRoot;

        [SerializeField] private int _priceOfRoll;
        [SerializeField] private int _priceOfHeal;

        [SerializeField] private float _healthFactor;

        private AudioSoundsService _audioSoundsService;
        private IPauseService _pauseService;
        private IPlayerService _playerService;
        private ICurrencyService _currencyService;
        private ITweenAnimationService _tweenAnimationService;
        private ILevelUpService _levelUpService;

        private WeaponFactory _weaponFactory;
        private WeaponHolder _weaponHolder;

        private Queue<int> _pendingLevels = new();

        private int _currentLevel;
        private bool _isShowing;
        private bool _isClosed;

        public event Action OnContinueButtonIsClicked;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, IPauseService pauseService,
            IPlayerService playerService, ICurrencyService currencyService,
            ITweenAnimationService tweenAnimationService, ILevelUpService levelUpService)
        {
            _audioSoundsService = audioSoundsService;
            _pauseService = pauseService;
            _playerService = playerService;
            _currencyService = currencyService;
            _tweenAnimationService = tweenAnimationService;
            _levelUpService = levelUpService;
        }

        private void OnEnable()
        {
            foreach (CardView cardView in _cardViews)
            {
                cardView.GetImprovementButtonClicked += OnCardViewButtonClicked;
            }

            _healButton.onClick.AddListener(OnHealButtonClicked);
            _rollButton.onClick.AddListener(OnRollButtonClicked);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnDisable()
        {
            foreach (CardView cardView in _cardViews)
            {
                cardView.GetImprovementButtonClicked -= OnCardViewButtonClicked;
            }

            _healButton.onClick.RemoveListener(OnHealButtonClicked);
            _rollButton.onClick.RemoveListener(OnRollButtonClicked);
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        public void GetServices(WeaponFactory weaponFactory, WeaponHolder weaponHolder)
        {
            _weaponFactory = weaponFactory;
            _weaponHolder = weaponHolder;
        }

        public async UniTask ShowAsync()
        {
            if (gameObject.activeSelf)
            {
                await ForceHideAsync();
            }

            gameObject.SetActive(true);
            await _tweenAnimationService.AnimateScaleAsync(transform);
            _isClosed = false;
        }

        public async UniTask HideAsync()
        {
            if (_isClosed)
                return;

            _isClosed = true;
            await _tweenAnimationService.AnimateScaleAsync(transform, true);

            await UniTask.NextFrame();
        }

        public async void OnCurrentLevelIsUpgraded(int currentLevel)
        {
            _healButton.gameObject.SetActive(false);
            _continueButton.gameObject.SetActive(false);

            HidePriceRoot();

            _pendingLevels.Enqueue(currentLevel);

            if (!_isShowing)
            {
                await ProcessPendingLevels();
            }
        }

        public async void OnEndGameTriggerIsReached()
        {
            _healButton.gameObject.SetActive(true);
            _continueButton.gameObject.SetActive(true);
            
            ShowPriceRoot();

            GetImprovements();
            await ShowAsync();
        }

        public void OnLanguageChanged()
        {
            if (!gameObject.activeSelf)
                return;

            foreach (var cardView in _cardViews)
            {
                cardView.SetData();
            }
        }
        
        private void ShowPriceRoot()
        {
            _priceRoot.gameObject.SetActive(true);
        }

        private void HidePriceRoot()
        {
            _priceRoot.gameObject.SetActive(false);
        }

        private async UniTask ProcessPendingLevels()
        {
            _isShowing = true;

            while (_pendingLevels.Count > MinValue)
            {
                int level = _pendingLevels.Dequeue();

                await ShowForLevelUp(level);
                await UniTask.WaitUntil(() => _isClosed);
                await UniTask.Delay(TimeSpan.FromSeconds(LevelUpDelay));
            }

            _isShowing = false;
        }

        private async UniTask ShowForLevelUp(int level)
        {
            _currentLevel = level;
            GetCardsForLevelUp(level);

            await ShowAsync();
        }

        private async UniTask ForceHideAsync()
        {
            _isClosed = true;
            transform.DOKill();
            gameObject.SetActive(false);
            await UniTask.NextFrame();
        }

        private void GetCardsForLevelUp(int currentLevel)
        {
            _levelUpService.GenerateCardsByLevel(currentLevel, _weaponHolder, _cardViews);

            _pauseService.StopGame();
        }

        private void GetImprovements()
        {
            _levelUpService.GenerateImprovements(_cardViews);

            _pauseService.StopGame();
        }

        private async void OnCardViewButtonClicked(Card card, CardView cardView)
        {
            _audioSoundsService.PlaySound(SoundsType.CardViewButton);

            if (card is ImprovementCard improvementCard)
            {
                if (improvementCard.WeaponType == WeaponType.None)
                {
                    _playerService.PlayerActor.AcceptImprovement(_weaponVisitor,
                        improvementCard.CharacteristicType, improvementCard.Value);
                }
                else
                {
                    foreach (var weapon in _weaponHolder.Weapons.Where(weapon =>
                                 improvementCard.WeaponType == weapon.Type))
                    {
                        weapon.AcceptWeaponImprovement(_weaponVisitor, improvementCard.CharacteristicType,
                            improvementCard.Value);
                    }
                }

                _levelUpService.RemoveImprovementCard(improvementCard);
            }
            else if (card is WeaponCard weaponCard)
            {
                PlayerWeapon weapon = await _weaponFactory.CreateWeapon(weaponCard.WeaponType);
                _levelUpService.UpdateImprovementCardsByTypeWeapon(weapon.Type);
            }

            foreach (CardView view in _cardViews)
            {
                view.Hide();
            }

            _pauseService.PlayGame();

            await HideAsync();
        }

        private void OnRollButtonClicked()
        {
            if (_currencyService.Gold < _priceOfRoll)
                return;

            _currencyService.SpendGold(_priceOfRoll);

            AnimateCardsView();
            _levelUpService.GenerateCardsByLevel(_currentLevel, _weaponHolder, _cardViews);
        }

        private void OnHealButtonClicked()
        {
            if (_currencyService.Gold < _priceOfHeal)
                return;

            _currencyService.SpendGold(_priceOfHeal);

            _playerService.AddHealthByFactor(_healthFactor);
        }

        private async void OnContinueButtonClicked()
        {
            OnContinueButtonIsClicked?.Invoke();
            await HideAsync();
        }

        private void AnimateCardsView()
        {
            foreach (var cardView in _cardViews)
            {
                _tweenAnimationService.AnimateScale(cardView.transform);
            }
        }
    }
}