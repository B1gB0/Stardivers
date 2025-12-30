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
        private readonly Dictionary<WeaponType, WeaponLocalizationData> _weaponsLocalizationData = new ();

        private readonly Dictionary<CharacteristicType, CharacteristicsLocalizationData>
            _characteristicsLocalizationData = new ();

        private readonly Dictionary<string, ImprovementData> _improvementsData = new ();

        private IDataBaseService _dataBaseService;

        public List<ImprovementCard> ImprovementCards { get; } = new ();
        public List<WeaponCard> WeaponCards { get; } = new ();
        public bool IsInitiated { get; private set; }

        [Inject]
        private void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
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

        private void CreateWeaponsCard()
        {
            foreach (var weaponLocalizationData in _weaponsLocalizationData)
            {
                WeaponCard weaponCard = new WeaponCard();
                weaponCard.SetData(weaponLocalizationData.Value);
                WeaponCards.Add(weaponCard);
            }
        }

        private void CreateImprovementCards()
        {
            foreach (var improvement in _improvementsData)
            {
                ImprovementData improvementData = improvement.Value;
                ImprovementCard improvementCard = new ImprovementCard();
                improvementCard.SetData(improvementData,
                    _characteristicsLocalizationData[improvementData.CharacteristicType]);
                ImprovementCards.Add(improvementCard);
            }
        }
    }
}