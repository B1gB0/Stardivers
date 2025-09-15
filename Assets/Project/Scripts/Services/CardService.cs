using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Cards;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Player;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class CardService : Service, ICardService
    {
        private Dictionary<WeaponType, WeaponLocalizationData> _weaponsLocalizationData = new ();
        private Dictionary<CharacteristicType, CharacteristicsLocalizationData> _characteristicsLocalizationData = new ();
        private Dictionary<string, ImprovementData> _improvementsData = new ();

        public List<ImprovementCard> ImprovementCards { get; private set; } = new();
        public List<WeaponCard> WeaponCards { get; private set; } = new();

        private IDataBaseService _dataBaseService;
        private ICharacteristicsWeaponDataService _characteristicsWeaponDataService;
        
        [Inject]
        private void Construct(IDataBaseService dataBaseService, ICharacteristicsWeaponDataService characteristicsWeaponDataService)
        {
            _dataBaseService = dataBaseService;
            _characteristicsWeaponDataService = characteristicsWeaponDataService;
        }
        
        public override UniTask Init()
        {
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
            
            return UniTask.CompletedTask;
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

    public interface ICardService
    {
        public List<ImprovementCard> ImprovementCards { get; }
        public List<WeaponCard> WeaponCards { get; }
        public UniTask Init();
    }
}