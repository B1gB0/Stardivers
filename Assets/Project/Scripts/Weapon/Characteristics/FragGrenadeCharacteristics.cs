namespace Project.Game.Scripts
{
    public class FragGrenadeCharacteristics
    {
        public float RangeAttack { get; private set; } = 5f;

        public float FireRate { get; private set; } = 5f;

        public float GrenadeSpeed { get; private set; } = 10f;
        
        public float ExplosionRadius { get; private set; } = 3f;

        public float Damage { get; private set; } = 1f;
        
        public void ApplyImprovement(CharacteristicsTypes type ,float factor)
        {
            switch (type)
            {
                case CharacteristicsTypes.Damage :
                    IncreaseDamage(factor);
                    break;
                case CharacteristicsTypes.FireRate :
                    IncreaseFireRate(factor);
                    break;
                case CharacteristicsTypes.ProjectileSpeed :
                    IncreaseBulletSpeed(factor);
                    break;
                case CharacteristicsTypes.RangeAttack :
                    IncreaseRangeAttack(factor);
                    break;
                case CharacteristicsTypes.ExplosionRadius :
                    IncreaseExplosionRadius(factor);
                    break;
            }
        }

        public void IncreaseDamage(float damageFactor)
        {
            Damage += Damage * damageFactor;
        }

        public void IncreaseFireRate(float fireRateFactor)
        {
            FireRate -= FireRate * fireRateFactor;
        }

        public void IncreaseBulletSpeed(float bulletSpeedFactor)
        {
            GrenadeSpeed += GrenadeSpeed * bulletSpeedFactor;
        }

        public void IncreaseRangeAttack(float rangeAttackFactor)
        {
            RangeAttack += RangeAttack * rangeAttackFactor;
        }
        
        public void IncreaseExplosionRadius(float explosionRadiusFactor)
        {
            ExplosionRadius += ExplosionRadius * explosionRadiusFactor;
        }
    }
}