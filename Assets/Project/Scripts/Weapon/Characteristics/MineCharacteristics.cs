using System;
using UnityEngine;

namespace Project.Scripts.Weapon.Characteristics
{
    [Serializable]
    public class MineCharacteristics
    {
        public float FireRate { get; private set; } = 5f;
        public float Damage { get; private set; } = 20f;
        public float ExplosionRadius { get; private set; } = 5f;
        
        public void ApplyImprovement(CharacteristicType type ,float factor)
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