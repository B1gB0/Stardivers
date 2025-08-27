namespace Project.Scripts.Weapon.Characteristics
{
    public class GunCharacteristics : Characteristics
    {
        public override void SetStartingCharacteristics()
        {
            rangeAttack = 5f;
            fireRate = 1f;
            projectileSpeed = 10f;
            damage = 15f;
            maxCountBullets = 4;
            reloadTime = 3f;
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
                case CharacteristicType.MaxCountBullets:
                    IncreaseMaxCountBullets((int)factor);
                    break;
            }
        }
    }
}