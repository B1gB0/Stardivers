using System;
using UnityEngine;

namespace Project.Scripts.Weapon.Characteristics
{
    [Serializable]
    public abstract class Characteristics
    {
        [SerializeField] protected float rangeAttack = 5f;
        [SerializeField] protected float fireRate = 1f;
        [SerializeField] protected float projectileSpeed = 10f;
        [SerializeField] protected float damage = 15f;
        [SerializeField] protected int maxCountBullets = 4;
        [SerializeField] protected float reloadTime = 3f;
        [SerializeField] protected float explosionRadius = 3f;
        [SerializeField] protected int maxEnemiesInChain = 3;

        public float RangeAttack => rangeAttack;
        public float FireRate => fireRate;
        public float ProjectileSpeed => projectileSpeed;
        public float Damage => damage;
        public int MaxCountBullets => maxCountBullets;
        public float ReloadTime => reloadTime;
        public float ExplosionRadius => explosionRadius;

        public abstract void SetStartingCharacteristics();

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