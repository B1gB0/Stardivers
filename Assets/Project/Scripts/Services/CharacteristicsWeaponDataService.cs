using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Weapon.Player;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class CharacteristicsWeaponDataService : Service, ICharacteristicsWeaponDataService
    {
        private readonly Dictionary<WeaponType, CharacteristicsWeaponData> _characteristicsData = new ();

        private IDataBaseService _dataBaseService;

        [Inject]
        private void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public override UniTask Init()
        {
            foreach (var data in _dataBaseService.Content.CharacteristicsWeaponsData)
            {
                _characteristicsData.Add(data.WeaponType, data);
            }
            
            return UniTask.CompletedTask;
        }

        public CharacteristicsWeaponData GetWeaponDataByType(WeaponType type)
        {
            return _characteristicsData[type];
        }
    }
}