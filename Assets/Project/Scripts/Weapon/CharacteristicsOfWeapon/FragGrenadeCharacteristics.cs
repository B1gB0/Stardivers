using Project.Scripts.DataBase.Data;

namespace Project.Scripts.Weapon.CharacteristicsOfWeapon
{
    public class FragGrenadeCharacteristics : Characteristics
    {
        public override void SetStartingCharacteristics(CharacteristicsWeaponData data)
        {
            rangeAttack = data.RangeAttack;
            fireRate = data.FireRate;
            projectileSpeed = data.ProjectileSpeed;
            damage = data.Damage;
            explosionRadius = data.ExplosionRadius;
        }

        public override void ApplyImprovement(CharacteristicType type ,float factor)
        {
            switch (type)
            {
                case CharacteristicType.Damage :
                    IncreaseDamage(factor);
                    break;
                case CharacteristicType.FireRate :
                    IncreaseFireRate(factor);
                    break;
                case CharacteristicType.ProjectileSpeed :
                    IncreaseBulletSpeed(factor);
                    break;
                case CharacteristicType.RangeAttack :
                    IncreaseRangeAttack(factor);
                    break;
                case CharacteristicType.ExplosionRadius :
                    IncreaseExplosionRadius(factor);
                    break;
            }
        }
    }
}