using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Weapon.Player;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class CharacteristicsWeaponDataService : ICharacteristicsWeaponDataService
    {
        private readonly Dictionary<WeaponType, CharacteristicsWeaponData> _characteristicsData = new();

        private IDataBaseService _dataBaseService;

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

            LoadAllWeaponsData();

            IsInitiated = true;

            return UniTask.CompletedTask;
        }

        public CharacteristicsWeaponData GetWeaponDataByType(WeaponType type)
        {
            return _characteristicsData[type];
        }

        private void LoadAllWeaponsData()
        {
            foreach (var data in _dataBaseService.Content.CharacteristicsWeaponsData)
            {
                _characteristicsData.TryAdd(data.WeaponType, data);
            }
        }
    }
}