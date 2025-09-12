using Project.Scripts.DataBase.Data;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;

namespace Project.Scripts.Cards
{
    public class ImprovementCard : Card
    {
        public ImprovementData ImprovementData { get; private set; }
        public CharacteristicsLocalizationData CharacteristicsLocalizationData { get; private set; }
        public CharacteristicType CharacteristicType { get; private set; }
        public float Value { get; private set; }

        public void SetData(ImprovementData improvementData, CharacteristicsLocalizationData characteristicsLocalizationData)
        {
            ImprovementData = improvementData;
            CharacteristicsLocalizationData = characteristicsLocalizationData;
            CharacteristicType = improvementData.CharacteristicType;
            Value = improvementData.Value;
            WeaponType = improvementData.WeaponType;
        }
    }
}