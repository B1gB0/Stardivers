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
        protected int maxCountShots;
        protected float reloadTime;
        protected float explosionRadius;
        protected int maxEnemiesInChain;
        protected float health;
        protected float diggingSpeed;
        protected float moveSpeed;

        public float RangeAttack => rangeAttack;
        public float FireRate => fireRate;
        public float ProjectileSpeed => projectileSpeed;
        public float Damage => damage;
        public int MaxCountShots => maxCountShots;
        public int MaxEnemiesInChain => maxEnemiesInChain;
        public float ReloadTime => reloadTime;
        public float ExplosionRadius => explosionRadius;
        public float Health => health;
        public float DiggingSpeed => diggingSpeed;
        public float MoveSpeed => moveSpeed;

        public abstract void ApplyImprovement(CharacteristicType type, float factor);

        public virtual void SetStartingCharacteristics(CharacteristicsWeaponData data) { }

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
            maxCountShots = maxCount;
        }
        
        protected virtual void IncreaseMaxEnemiesInChain(int maxEnemies)
        {
            maxEnemiesInChain = maxEnemies;
        }
        
        protected virtual void IncreaseExplosionRadius(float explosionRadiusFactor)
        {
            explosionRadius += Mathf.Round(explosionRadius * explosionRadiusFactor);
        }

        protected virtual void IncreaseHealth(float healthValue)
        {
            health += healthValue;
        }

        protected virtual void IncreaseDiggingSpeed(float diggingSpeedFactor)
        {
            diggingSpeed += Mathf.Round(diggingSpeed * diggingSpeedFactor);
        }
        
        protected virtual void IncreaseMoveSpeed(float moveSpeedFactor)
        {
            moveSpeed += Mathf.Round(moveSpeed * moveSpeedFactor);
        }
    }
}