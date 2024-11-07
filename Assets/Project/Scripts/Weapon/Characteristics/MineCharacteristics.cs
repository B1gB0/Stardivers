using UnityEngine;

namespace Project.Game.Scripts
{
    public class MineCharacteristics
    {
        public float FireRate { get; private set; } = 5f;
        
        public float Damage { get; private set; } = 20f;
        
        public float ExplosionRadius { get; private set; } = 5f;
        
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
        
        private void IncreaseExplosionRadius(float explosionRadiusFactor)
        {
            ExplosionRadius += Mathf.Round(ExplosionRadius * explosionRadiusFactor);
        }
    }
}