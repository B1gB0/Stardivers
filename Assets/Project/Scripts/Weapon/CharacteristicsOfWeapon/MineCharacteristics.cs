using Project.Scripts.DataBase.Data;

namespace Project.Scripts.Weapon.CharacteristicsOfWeapon
{
    public class MineCharacteristics : Characteristics
    {
        public override void SetStartingCharacteristics(CharacteristicsWeaponData data)
        {
            fireRate = data.FireRate;
            damage = data.Damage;
            explosionRadius = data.ExplosionRadius;
            maxCountShots = data.MaxCountShots;
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
                case CharacteristicType.ExplosionRadius :
                    IncreaseExplosionRadius(factor);
                    break;
                case CharacteristicType.MaxCountShots :
                    IncreaseMaxCountBullets((int)factor);
                    break;
            }
        }
    }
}