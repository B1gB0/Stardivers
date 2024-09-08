namespace Project.Game.Scripts
{
    public class MineCharacteristics
    {
        public float FireRate { get; private set; } = 5f;
        
        public float Damage { get; private set; } = 4f;
        
        public float ExplosionRadius { get; private set; } = 5f;
        
        public void IncreaseDamage(float damageFactor)
        {
            Damage += Damage * damageFactor;
        }
        
        public void IncreaseFireRate(float fireRateFactor)
        {
            FireRate -= FireRate * fireRateFactor;
        }
        
        public void IncreaseExplosionRadius(float explosionRadiusFactor)
        {
            ExplosionRadius += ExplosionRadius * explosionRadiusFactor;
        }
    }
}