using Project.Scripts.DataBase.Data;

namespace Project.Scripts.Cards
{
    public class WeaponCard : Card
    {
        public CharacteristicsWeaponData CharacteristicsWeaponData { get; private set; }
        public WeaponLocalizationData WeaponLocalizationData { get; private set; }

        public void SetData(WeaponLocalizationData weaponLocalizationData, CharacteristicsWeaponData characteristicsWeaponData)
        {
            WeaponLocalizationData = weaponLocalizationData;
            CharacteristicsWeaponData = characteristicsWeaponData;
            WeaponType = weaponLocalizationData.Type;
        }
    }
}
