using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Cards;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Player;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class CardService : ICardService
    {
        private Dictionary<WeaponType, WeaponLocalizationData> _weaponsLocalizationData = new ();
        private Dictionary<CharacteristicType, CharacteristicsLocalizationData> _characteristicsLocalizationData = new ();
        private Dictionary<string, ImprovementData> _improvementsData = new ();
        
        private IDataBaseService _dataBaseService;
        private ICharacteristicsWeaponDataService _characteristicsWeaponDataService;
        
        public List<ImprovementCard> ImprovementCards { get; } = new();
        public List<WeaponCard> WeaponCards { get; } = new();
        public bool IsInitiated { get; private set; }
        
        [Inject]
        private void Construct(IDataBaseService dataBaseService, ICharacteristicsWeaponDataService characteristicsWeaponDataService)
        {
            _dataBaseService = dataBaseService;
            _characteristicsWeaponDataService = characteristicsWeaponDataService;
        }
        
        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;
            
            foreach (var weapon in _dataBaseService.Content.WeaponsLocalization)
            {
                _weaponsLocalizationData.TryAdd(weapon.Type, weapon);
            }

            foreach (var characteristic in _dataBaseService.Content.CharacteristicsLocalization)
            {
                _characteristicsLocalizationData.TryAdd(characteristic.Type, characteristic);
            }

            foreach (var improvement in _dataBaseService.Content.Improvements)
            {
                _improvementsData.TryAdd(improvement.Id, improvement);
            }
            
            CreateImprovementCards();
            CreateWeaponsCard();

            IsInitiated = true;
            
            return UniTask.CompletedTask;
        }

        public void RecreateAllCards()
        {
            WeaponCards.Clear();
            ImprovementCards.Clear();
            CreateWeaponsCard();
            CreateImprovementCards();
        }

        private void CreateWeaponsCard()
        {
            foreach (var weaponLocalizationData in _weaponsLocalizationData)
            {
                WeaponCard weaponCard = new WeaponCard();
                CharacteristicsWeaponData characteristicsWeaponData = 
                    _characteristicsWeaponDataService.GetWeaponDataByType(weaponLocalizationData.Value.Type);
                weaponCard.SetData(weaponLocalizationData.Value, characteristicsWeaponData);
                WeaponCards.Add(weaponCard);
            }
        }
        
        private void CreateImprovementCards()
        {
            foreach (var improvement in _improvementsData)
            {
                ImprovementData improvementData = improvement.Value;
                ImprovementCard improvementCard = new ImprovementCard();
                improvementCard.SetData(improvementData, _characteristicsLocalizationData[improvementData.CharacteristicType]);
                ImprovementCards.Add(improvementCard);
            }
        }
    }
}