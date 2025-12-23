using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Scripts.Cards;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Player;
using Reflex.Attributes;
using UnityEngine;
using Random = System.Random;

namespace Project.Scripts.Services
{
    public class LevelUpService : ILevelUpService
    {
        private const int Remainder = 0;
        private const int MinValue = 0;
        private const int Multiplicity = 3;
        private const int CountWeapons = 4;
        private const int CounterCorrector = 1;
        
        private readonly Random _random = new();
        private readonly List<Card> _currentImprovementCards = new();
        private readonly List<Card> _currentWeaponCards = new();
        
        private ICardService _cardService;
        
        public bool IsInitiated { get; private set; }

        [Inject]
        private void Construct(ICardService cardService)
        {
            _cardService = cardService;
        }

        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;

            RecreateCards();

            IsInitiated = true;
            
            return UniTask.CompletedTask;
        }

        public void RemoveImprovementCard(ImprovementCard improvementCard)
        {
            _currentImprovementCards.Remove(improvementCard);
        }

        public void RemoveWeaponCard(WeaponType type)
        {
            for (int i = _currentWeaponCards.Count - CounterCorrector; i >= MinValue; i--)
            {
                if (_currentWeaponCards[i].WeaponType != type)
                    continue;
                
                _currentWeaponCards.RemoveAt(i);
                break;
            }
        }
        
        public void GenerateCardsByLevel(int currentLevel, WeaponHolder weaponHolder, List<CardView> cardViews)
        {
            if (currentLevel % Multiplicity == Remainder && weaponHolder.Weapons.Count < CountWeapons)
            {
                SortRandomCards(_currentWeaponCards);
                GetCards(_currentWeaponCards, cardViews);
            }
            else
            {
                GenerateImprovements(cardViews);
            }
        }

        public void GenerateImprovements(List<CardView> cardViews)
        {
            SortRandomCards(_currentImprovementCards);
            var result = FilterDuplicateCards(_currentImprovementCards);
            GetCards(result, cardViews);
        }

        public void RecreateCards()
        {
            _currentWeaponCards.Clear();
            _currentImprovementCards.Clear();
            
            foreach (WeaponCard card in _cardService.WeaponCards)
            {
                _currentWeaponCards.Add(card);
            }
            
            UpdateImprovementCardsByTypeWeapon(WeaponType.None);
        }
        
        public void UpdateImprovementCardsByTypeWeapon(WeaponType type)
        {
            foreach (ImprovementCard card in _cardService.ImprovementCards)
            {
                if (card.WeaponType == type)
                {
                    _currentImprovementCards.Add(card);
                }
            }
        }
        
        private void GetCards(List<Card> cards, List<CardView> cardViews)
        {
            if (cards.Count >= cardViews.Count)
            {
                for (int i = 0; i < cardViews.Count; i++)
                {
                    cardViews[i].GetCard(cards[i]);
                    cardViews[i].Show();
                }
            }
            else
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    cardViews[i].GetCard(cards[i]);
                    cardViews[i].Show();
                }
            }
        }
        
        private void SortRandomCards(IList<Card> cards)
        {
            int count = cards.Count;

            while (count > CounterCorrector)
            {
                count--;

                int index = _random.Next(count + CounterCorrector);

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
    }
}