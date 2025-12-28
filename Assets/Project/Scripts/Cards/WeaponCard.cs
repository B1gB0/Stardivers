using Project.Scripts.DataBase.Data;

namespace Project.Scripts.Cards
{
    public class WeaponCard : Card
    {
        private CharacteristicsWeaponData _characteristicsWeaponData;
        
        public WeaponLocalizationData WeaponLocalizationData { get; private set; }

        public void SetData(WeaponLocalizationData weaponLocalizationData, CharacteristicsWeaponData characteristicsWeaponData)
        {
            WeaponLocalizationData = weaponLocalizationData;
            _characteristicsWeaponData = characteristicsWeaponData;
            WeaponType = weaponLocalizationData.Type;
        }
    }
}
