using System;
using UnityEngine;

namespace Project.Scripts.Weapon.Characteristics
{
    [Serializable]
    public class FragGrenadeCharacteristics
    {
        [SerializeField] private float _rangeAttack = 5f;
        [SerializeField] private float _fireRate = 5f;
        [SerializeField] private float _grenadeSpeed = 10f;
        [SerializeField] private float _explosionRadius = 3f;
        [SerializeField] private float _damage = 10f;
        
        public float RangeAttack => _rangeAttack;
        public float FireRate => _fireRate;
        public float GrenadeSpeed => _grenadeSpeed;
        public float ExplosionRadius => _explosionRadius;
        public float Damage => _damage;
        
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

        private void IncreaseDamage(float damageFactor)
        {
            _damage += Mathf.Round(_damage * damageFactor);
        }

        private void IncreaseFireRate(float fireRateFactor)
        {
            _fireRate -= Mathf.Round(_fireRate * fireRateFactor);
        }

        private void IncreaseBulletSpeed(float bulletSpeedFactor)
        {
            _grenadeSpeed += Mathf.Round(_grenadeSpeed * bulletSpeedFactor);
        }

        private void IncreaseRangeAttack(float rangeAttackFactor)
        {
            _rangeAttack += Mathf.Round(_rangeAttack * rangeAttackFactor);
        }
        
        private void IncreaseExplosionRadius(float explosionRadiusFactor)
        {
            _explosionRadius += Mathf.Round(_explosionRadius * explosionRadiusFactor);
        }
    }
}