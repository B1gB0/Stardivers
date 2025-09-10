using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Weapon.Player;

namespace Project.Scripts.Services
{
    public interface ICharacteristicsWeaponDataService
    {
        UniTask Init();
        public CharacteristicsWeaponData GetWeaponDataByType(WeaponType type);
    }
}