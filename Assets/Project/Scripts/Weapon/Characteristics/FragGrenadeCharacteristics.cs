namespace Project.Scripts.Weapon.Characteristics
{
    public class FragGrenadeCharacteristics : Characteristics
    {
        public override void SetStartingCharacteristics()
        {
            rangeAttack = 4f;
            fireRate = 5f;
            projectileSpeed = 10f;
            explosionRadius = 3f;
            damage = 10f;
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