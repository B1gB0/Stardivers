namespace Project.Game.Scripts
{
    public class GunCharacteristics
    {
        public float RangeAttack { get; private set; } = 5f;

        public float FireRate { get; private set; } = 1f;

        public float BulletSpeed { get; private set; } = 10f;

        public float Damage { get; private set; } = 2f;
        
        public int MaxCountShots { get; private set; } = 7;
    
        public float ReloadTime { get; private set; } = 3f;
        
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
            BulletSpeed += BulletSpeed * bulletSpeedFactor;
        }

        public void IncreaseRangeAttack(float rangeAttackFactor)
        {
            RangeAttack += RangeAttack * rangeAttackFactor;
        }
    }
}