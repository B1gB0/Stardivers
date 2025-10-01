using System.Collections.Generic;
using System.Linq;
using Project.Game.Scripts;
using Project.Scripts.Cards;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.Improvements;
using Project.Scripts.Weapon.Player;
using Reflex.Attributes;
using UnityEngine;
using Random = System.Random;

namespace Project.Scripts.UI.Panel
{
    public class LevelUpPanel : MonoBehaviour, IView
    {
        private const int MinIndex = 1;
        private const int Remainder = 0;
        private const int StartWeapon = 0;
        private const int Multiplicity = 3;
        private const int CountWeapons = 4;
        
        private readonly List<Card> _currentImprovementCards = new ();
        private readonly List<Card> _currentWeaponCards = new ();
        private readonly WeaponVisitor _weaponVisitor = new ();
        private readonly Random _random = new ();

        [SerializeField] private List<CardView> _cardsView = new ();

        private AudioSoundsService _audioSoundsService;
        private IPauseService _pauseService;
        private ICardService _cardService;
        private IPlayerService _playerService;

        private WeaponFactory _weaponFactory;
        private WeaponHolder _weaponHolder;

        public bool IsClosed { get; private set; }

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, IPauseService pauseService, 
            ICardService cardService, IPlayerService playerService)
        {
            _audioSoundsService = audioSoundsService;
            _pauseService = pauseService;
            _cardService = cardService;
            _playerService = playerService;
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
                cardView.GetImprovementButtonClicked += OnButtonClicked;
            }
        }

        private void OnDisable()
        {
            foreach (CardView cardView in _cardsView)
            {
                cardView.GetImprovementButtonClicked -= OnButtonClicked;
            }
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

        public void Show()
        {
            gameObject.SetActive(true);
            IsClosed = false;
        }

        public void Hide()
        {
            IsClosed = true;
            gameObject.SetActive(false);
        }

        public void OnCurrentLevelIsUpgraded(int currentLevel)
        {
            GetCardsForLevelUp(currentLevel);
            Show();
        }

        private void GetCardsForLevelUp(int currentLevel)
        {
            if (currentLevel % Multiplicity == Remainder && _weaponHolder.Weapons.Count < CountWeapons)
            {
                SortAndGetCards(_currentWeaponCards);
            }
            else
            {
                SortAndGetCards(_currentImprovementCards);
            }
            
            _pauseService.StopGame();
        }

        private void SortAndGetCards(List<Card> cards)
        {
            SortRandomCards(cards);

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

                int index = _random.Next(count + 1);
                
                (cards[index], cards[count]) = (cards[count], cards[index]);
            }
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

        private async void OnButtonClicked(Card card, CardView cardView)
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
                    foreach (var weapon in _weaponHolder.Weapons.Where(weapon => improvementCard.WeaponType == weapon.Type))
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

            Hide();
        }
    }
}