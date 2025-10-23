using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Game.Scripts;
using Project.Scripts.Cards;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using Project.Scripts.Weapon.Player;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Project.Scripts.UI.Panel
{
    public class LevelUpPanel : MonoBehaviour, IView
    {
        private const int MinIndex = 1;
        private const int MinValue = 0;
        private const int Remainder = 0;
        private const int StartWeapon = 0;
        private const int Multiplicity = 3;
        private const int CountWeapons = 4;
        private const int CorrectFactorCounter = 1;
        
        private const float LevelUpDelay = 0.3f;

        private readonly List<Card> _currentImprovementCards = new();
        private readonly List<Card> _currentWeaponCards = new();
        private readonly WeaponVisitor _weaponVisitor = new();
        private readonly Random _random = new();

        [SerializeField] private List<CardView> _cardsView = new();
        [SerializeField] private Button _rollButton;
        [SerializeField] private int _priceOfRoll;

        private AudioSoundsService _audioSoundsService;
        private IPauseService _pauseService;
        private ICardService _cardService;
        private IPlayerService _playerService;
        private ICurrencyService _currencyService;
        private ITweenAnimationService _tweenAnimationService;

        private WeaponFactory _weaponFactory;
        private WeaponHolder _weaponHolder;

        private Queue<int> _pendingLevels = new ();

        private int _currentLevel;
        private bool _isShowing;

        public bool IsClosed { get; private set; } = true;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, IPauseService pauseService,
            ICardService cardService, IPlayerService playerService, ICurrencyService currencyService, 
            ITweenAnimationService tweenAnimationService)
        {
            _audioSoundsService = audioSoundsService;
            _pauseService = pauseService;
            _cardService = cardService;
            _playerService = playerService;
            _currencyService = currencyService;
            _tweenAnimationService = tweenAnimationService;
        }

        private void Start()
        {
            foreach (WeaponCard card in _cardService.WeaponCards)
            {
                if (card.WeaponType == _weaponHolder.Weapons[StartWeapon].Type)
                    continue;

                _currentWeaponCards.Add(card);
            }
        }

        private void OnEnable()
        {
            foreach (CardView cardView in _cardsView)
            {
                cardView.GetImprovementButtonClicked += OnCardViewButtonClicked;
            }

            _rollButton.onClick.AddListener(OnRollButtonClicked);
        }

        private void OnDisable()
        {
            foreach (CardView cardView in _cardsView)
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

        public void GetStartImprovements()
        {
            UpdateImprovementCardsByTypeWeapon(_weaponHolder.Weapons[StartWeapon].Type);
            UpdateImprovementCardsByTypeWeapon(WeaponType.None);
        }

        public async UniTask ShowAsync()
        {
            if (gameObject.activeSelf)
            {
                await ForceHideAsync();
            }
            
            gameObject.SetActive(true);
            await _tweenAnimationService.AnimateScaleAsync(transform);
            IsClosed = false;
        }

        public async UniTask HideAsync()
        {
            if(IsClosed)
                return;
            
            IsClosed = true;
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

        private async UniTask ProcessPendingLevels()
        {
            _isShowing = true;
        
            while (_pendingLevels.Count > MinValue)
            {
                int level = _pendingLevels.Dequeue();
                
                await ShowForLevel(level);
                await UniTask.WaitUntil(() => IsClosed);
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
            IsClosed = true;
            transform.DOKill();
            gameObject.SetActive(false);
            await UniTask.NextFrame();
        }

        private void GetCardsForLevelUp(int currentLevel)
        {
            GenerateCards(currentLevel);

            _pauseService.StopGame();
        }

        private void GenerateCards(int currentLevel)
        {
            if (currentLevel % Multiplicity == Remainder && _weaponHolder.Weapons.Count < CountWeapons)
            {
                SortRandomCards(_currentWeaponCards);
                GetCards(_currentWeaponCards);
            }
            else
            {
                SortRandomCards(_currentImprovementCards);
                var result = FilterDuplicateCards(_currentImprovementCards);
                GetCards(result);
            }
        }

        private void GetCards(List<Card> cards)
        {
            if (cards.Count >= _cardsView.Count)
            {
                for (int i = 0; i < _cardsView.Count; i++)
                {
                    _cardsView[i].GetCard(cards[i]);
                    _cardsView[i].Show();
                }
            }
            else
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    _cardsView[i].GetCard(cards[i]);
                    _cardsView[i].Show();
                }
            }
        }

        private void SortRandomCards(IList<Card> cards)
        {
            int count = cards.Count;

            while (count > MinIndex)
            {
                count--;

                int index = _random.Next(count + MinIndex);

                (cards[index], cards[count]) = (cards[count], cards[index]);
            }
        }

        private List<Card> FilterDuplicateCards(List<Card> cards)
        {
            List<ImprovementCard> improvementCards = cards.Cast<ImprovementCard>().ToList();
            var result = new List<ImprovementCard>();
            var encounteredCombinations = new HashSet<(WeaponType, CharacteristicType)>();

            foreach (var card in improvementCards)
            {
                var combination = (card.WeaponType, card.CharacteristicType);
                
                if (encounteredCombinations.Add(combination))
                {
                    result.Add(card);
                }
            }

            return result.Cast<Card>().ToList();
        }

        private void UpdateImprovementCardsByTypeWeapon(WeaponType type)
        {
            foreach (ImprovementCard card in _cardService.ImprovementCards)
            {
                if (card.WeaponType == type)
                {
                    _currentImprovementCards.Add(card);
                }
            }
        }

        private async void OnCardViewButtonClicked(Card card, CardView cardView)
        {
            _audioSoundsService.PlaySound(Sounds.CardViewButton);

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

                _currentImprovementCards.Remove(improvementCard);
            }
            else if (card is WeaponCard weaponCard)
            {
                PlayerWeapon weapon = await _weaponFactory.CreateWeapon(weaponCard.WeaponType);
                UpdateImprovementCardsByTypeWeapon(weapon.Type);
                _currentWeaponCards.Remove(weaponCard);
            }

            foreach (CardView view in _cardsView)
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
            GenerateCards(_currentLevel);
        }

        private void AnimateCardsView()
        {
            foreach (var cardView in _cardsView)
            {
                _tweenAnimationService.AnimateScale(cardView.transform);
            }
        }
    }
}