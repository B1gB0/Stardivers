using System;
using Project.Scripts.DataBase.Data;
using UnityEngine;

namespace Project.Scripts.Weapon.CharacteristicsOfWeapon
{
    [Serializable]
    public abstract class Characteristics
    {
        protected float rangeAttack;
        protected float fireRate;
        protected float projectileSpeed;
        protected float damage;
        protected int maxCountBullets;
        protected float reloadTime;
        protected float explosionRadius;
        protected int maxEnemiesInChain;

        public float RangeAttack => rangeAttack;
        public float FireRate => fireRate;
        public float ProjectileSpeed => projectileSpeed;
        public float Damage => damage;
        public int MaxCountBullets => maxCountBullets;
        public int MaxEnemiesInChain => maxEnemiesInChain;
        public float ReloadTime => reloadTime;
        public float ExplosionRadius => explosionRadius;

        public abstract void SetStartingCharacteristics(CharacteristicsWeaponData data);

        public abstract void ApplyImprovement(CharacteristicType type, float factor);

        protected virtual void IncreaseDamage(float damageFactor)
        {
            damage += Mathf.Round(damage * damageFactor);
        }

        protected virtual void IncreaseFireRate(float fireRateFactor)
        {
            fireRate -= Mathf.Round(fireRate * fireRateFactor);
        }

        protected virtual void IncreaseBulletSpeed(float bulletSpeedFactor)
        {
            projectileSpeed += Mathf.Round(projectileSpeed * bulletSpeedFactor);
        }

        protected virtual void IncreaseRangeAttack(float rangeAttackFactor)
        {
            rangeAttack += Mathf.Round(rangeAttack * rangeAttackFactor);
        }

        protected virtual void IncreaseReloadVelocity(float reloadTimeFactor)
        {
            reloadTime -= Mathf.Round(reloadTime * reloadTimeFactor);
        }

        protected virtual void IncreaseMaxCountBullets(int maxCount)
        {
            maxCountBullets = maxCount;
        }
        
        protected virtual void IncreaseMaxEnemiesInChain(int maxEnemies)
        {
            maxEnemiesInChain = maxEnemies;
        }
        
        protected virtual void IncreaseExplosionRadius(float explosionRadiusFactor)
        {
            explosionRadius += Mathf.Round(explosionRadius * explosionRadiusFactor);
        }
    }
}