using Project.Scripts.DataBase.Data;

namespace Project.Scripts.Weapon.CharacteristicsOfWeapon
{
    public class ChainLightningGunCharacteristics : Characteristics
    {
        public override void SetStartingCharacteristics(CharacteristicsWeaponData data)
        {
            rangeAttack = data.RangeAttack;
            fireRate = data.FireRate;
            damage = data.Damage;
            maxCountShots = data.MaxCountShots;
            reloadTime = data.ReloadTime;
            maxEnemiesInChain = data.MaxEnemiesInChain;
        }

        public override void ApplyImprovement(CharacteristicType type, float factor)
        {
            switch (type)
            {
                case CharacteristicType.Damage:
                    IncreaseDamage(factor);
                    break;
                case CharacteristicType.FireRate:
                    IncreaseFireRate(factor);
                    break;
                case CharacteristicType.RangeAttack:
                    IncreaseRangeAttack(factor);
                    break;
                case CharacteristicType.ReloadTime:
                    IncreaseReloadVelocity(factor);
                    break;
                case CharacteristicType.MaxCountShots:
                    IncreaseMaxCountBullets((int)factor);
                    break;
                case CharacteristicType.MaxCountEnemiesInChain:
                    IncreaseMaxEnemiesInChain((int)factor);
                    break;
            }
        }
    }
}