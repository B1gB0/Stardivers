namespace Project.Scripts.Weapon.Characteristics
{
    public class MachineGunCharacteristics : Characteristics
    {
        public override void SetStartingCharacteristics()
        {
            rangeAttack = 5f;
            fireRate = 3f;
            projectileSpeed = 10f;
            damage = 18f;
            maxCountBullets = 16;
            reloadTime = 6f;
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
                case CharacteristicType.ReloadTime :
                    IncreaseReloadVelocity(factor);
                    break;
            }
        }
    }
}
