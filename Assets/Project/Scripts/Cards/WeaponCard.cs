using Project.Scripts.DataBase.Data;

namespace Project.Scripts.Cards
{
    public class WeaponCard : Card
    {
        public WeaponLocalizationData WeaponLocalizationData { get; private set; }

        public void SetData(WeaponLocalizationData weaponLocalizationData)
        {
            WeaponLocalizationData = weaponLocalizationData;
            WeaponType = weaponLocalizationData.Type;
        }
    }
}
