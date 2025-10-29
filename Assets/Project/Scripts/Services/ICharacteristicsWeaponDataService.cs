using Project.Scripts.DataBase.Data;
using Project.Scripts.Weapon.Player;

namespace Project.Scripts.Services
{
    public interface ICharacteristicsWeaponDataService : IService
    {
        public CharacteristicsWeaponData GetWeaponDataByType(WeaponType type);
    }
}