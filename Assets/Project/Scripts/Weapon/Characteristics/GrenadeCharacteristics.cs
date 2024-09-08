namespace Project.Game.Scripts
{
    public class GrenadeCharacteristics
    {
        public float RangeAttack { get; private set; } = 5f;

        public float FireRate { get; private set; } = 2f;

        public float GrenadeSpeed { get; private set; } = 10f;
        
        public float ExplosionRadius { get; private set; } = 3f;

        public float Damage { get; private set; } = 1f;
        
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