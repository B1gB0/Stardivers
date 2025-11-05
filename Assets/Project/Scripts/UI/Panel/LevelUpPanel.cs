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
        [SerializeField] private int _priceOfRoll;

        private AudioSoundsService _audioSoundsService;
        private IPauseService _pauseService;
        private IPlayerService _playerService;
        private ICurrencyService _currencyService;
        private ITweenAnimationService _tweenAnimationService;
        private ILevelUpService _levelUpService;

        private WeaponFactory _weaponFactory;
        private WeaponHolder _weaponHolder;

        private Queue<int> _pendingLevels = new ();

        private int _currentLevel;
        private bool _isShowing;
        private bool _isClosed;

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

            _rollButton.onClick.AddListener(OnRollButtonClicked);
        }

        private void OnDisable()
        {
            foreach (CardView cardView in _cardViews)
            {
                cardView.GetImprovementButtonClicked -= OnCardViewButtonClicked;
            }

            _rollButton.onClick.RemoveListener(OnRollButtonClicked);
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
            if(_isClosed)
                return;
            
            _isClosed = true;
            await _tweenAnimationService.AnimateScaleAsync(transform, true);

            await UniTask.NextFrame();
        }

        public async void OnCurrentLevelIsUpgraded(int currentLevel)
        {
            _pendingLevels.Enqueue(currentLevel);

            if (!_isShowing)
            {
                await ProcessPendingLevels();
            }
        }

        public void OnLanguageChanged()
        {
            if(!gameObject.activeSelf)
                return;
            
            foreach (var cardView in _cardViews)
            {
                cardView.SetData();
            }
        }

        private async UniTask ProcessPendingLevels()
        {
            _isShowing = true;
        
            while (_pendingLevels.Count > MinValue)
            {
                int level = _pendingLevels.Dequeue();
                
                await ShowForLevel(level);
                await UniTask.WaitUntil(() => _isClosed);
                await UniTask.Delay(TimeSpan.FromSeconds(LevelUpDelay));
            }
        
            _isShowing = false;
        }
        
        private async UniTask ShowForLevel(int level)
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
            _levelUpService.GenerateCards(currentLevel, _weaponHolder, _cardViews);

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
            _levelUpService.GenerateCards(_currentLevel, _weaponHolder, _cardViews);
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