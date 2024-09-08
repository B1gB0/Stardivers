using System;
using System.Collections.Generic;
using Project.Game.Scripts;
using Project.Scripts.Cards.ScriptableObjects;
using UnityEngine;
using Random = System.Random;

namespace Project.Scripts.UI
{
    public class LevelUpPanel : MonoBehaviour, IView
    {
        private const string Damage = nameof(Damage);
        private const string RangeAttack = nameof(RangeAttack);
        private const string FireRate = nameof(FireRate);
        private const string BulletSpeed = nameof(BulletSpeed);
        
        private const string Gun = nameof(Gun);
        private const string Mines = nameof(Mines);
        
        private const int MinIndex = 1;
        private const int Remainder = 0;
        private const int StartWeapon = 0;
        private const int Multiplicity = 2;

        private readonly List<Card> _currentImprovementCards = new ();
        private readonly List<Card> _currentWeaponCards = new ();
        private readonly Random _random = new ();

        [SerializeField] private List<CardView> _cardsView = new ();
        [SerializeField] private List<ImprovementCard> _improvementCards = new ();
        [SerializeField] private List<WeaponCard> _weaponCards = new ();

        private PauseService _pauseService;
        private WeaponFactory _weaponFactory;
        private WeaponHolder _weaponHolder;

        private void Start()
        {
            foreach (WeaponCard card in _weaponCards)
            {
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

        public void GetServices(PauseService pauseService, WeaponFactory weaponFactory, WeaponHolder weaponHolder)
        {
            _pauseService = pauseService;
            _weaponFactory = weaponFactory;
            _weaponHolder = weaponHolder;
        }

        public void GetStartImprovements()
        {
            UpdateImprovementCardsByWeapon(_weaponHolder.Weapons[StartWeapon]);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnLevelUpgraded(int currentLevel)
        {
            GetCardsForLevelUp(currentLevel);
            Show();
            _pauseService.StopGame();
        }

        private void GetCardsForLevelUp(int currentLevel)
        {
            if (currentLevel % Multiplicity == Remainder)
            {
                SortAndGetCards(_currentWeaponCards);
            }
            else
            {
                SortAndGetCards(_currentImprovementCards);
            }
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

        private void UpdateImprovementCardsByWeapon(Weapon weapon)
        {
            foreach (ImprovementCard card in _improvementCards)
            {
                if (card.WeaponType == weapon.Type)
                {
                    _currentImprovementCards.Add(card);
                }
            }
        }

        private void OnButtonClicked(Card card, CardView cardView)
        {
            //_weaponCards.Remove(card);

            if (card is ImprovementCard improvementCard)
            {
                _currentImprovementCards.Remove(improvementCard);
                // switch (improvementCard.TypeCharacteristics)
                // {
                //     case Damage
                //         
                // }
            }
            else if (card is WeaponCard weaponCard)
            {
                _weaponFactory.CreateWeapon(weaponCard.Weapon.Type);
                UpdateImprovementCardsByWeapon(weaponCard.Weapon);
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