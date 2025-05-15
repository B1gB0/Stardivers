using System;
using UnityEngine;

namespace Project.Scripts.Weapon.Characteristics
{
    [Serializable]
    public class GunCharacteristics
    {
        [SerializeField] private float _rangeAttack = 5f;
        [SerializeField] private float _fireRate = 1f;
        [SerializeField] private float _bulletSpeed = 10f;
        [SerializeField] private float _damage = 15f;
        [SerializeField] private int _maxCountShots = 7;
        [SerializeField] private float _reloadTime = 3f;
        
        public float RangeAttack => _rangeAttack;
        public float FireRate => _fireRate;
        public float BulletSpeed => _bulletSpeed;
        public float Damage => _damage;
        public int MaxCountShots => _maxCountShots;
        public float ReloadTime => _reloadTime;

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
                case CharacteristicType.ReloadTime :
                    IncreaseReloadVelocity(factor);
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
            _bulletSpeed += Mathf.Round(_bulletSpeed * bulletSpeedFactor);
        }

        private void IncreaseRangeAttack(float rangeAttackFactor)
        {
            _rangeAttack += Mathf.Round(_rangeAttack * rangeAttackFactor);
        }
        
        private void IncreaseReloadVelocity(float reloadTimeFactor)
        {
            _reloadTime += Mathf.Round(_reloadTime * reloadTimeFactor);
        }
    }
}