using Project.Scripts.DataBase.Data;

namespace Project.Scripts.Weapon.CharacteristicsOfWeapon
{
    public class GunCharacteristics : WeaponCharacteristics
    {
        public override void SetStartingCharacteristics(CharacteristicsWeaponData data)
        {
            RangeAttack = data.RangeAttack;
            FireRate = data.FireRate;
            ProjectileSpeed = data.ProjectileSpeed;
            Damage = data.Damage;
            MaxCountShots = data.MaxCountShots;
            ReloadTime = data.ReloadTime;
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
                case CharacteristicType.ProjectileSpeed:
                    IncreaseBulletSpeed(factor);
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
            }
        }
    }
}