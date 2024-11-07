using UnityEngine;

namespace Project.Game.Scripts
{
    public class FragGrenadeCharacteristics
    {
        public float RangeAttack { get; private set; } = 5f;

        public float FireRate { get; private set; } = 5f;

        public float GrenadeSpeed { get; private set; } = 10f;
        
        public float ExplosionRadius { get; private set; } = 3f;

        public float Damage { get; private set; } = 10f;
        
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

        private void IncreaseDamage(float damageFactor)
        {
            Damage += Mathf.Round(Damage * damageFactor);
        }

        private void IncreaseFireRate(float fireRateFactor)
        {
            FireRate -= Mathf.Round(FireRate * fireRateFactor);
        }

        private void IncreaseBulletSpeed(float bulletSpeedFactor)
        {
            GrenadeSpeed += Mathf.Round(GrenadeSpeed * bulletSpeedFactor);
        }

        private void IncreaseRangeAttack(float rangeAttackFactor)
        {
            RangeAttack += Mathf.Round(RangeAttack * rangeAttackFactor);
        }
        
        private void IncreaseExplosionRadius(float explosionRadiusFactor)
        {
            ExplosionRadius += Mathf.Round(ExplosionRadius * explosionRadiusFactor);
        }
    }
}