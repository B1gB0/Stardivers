using System;
using Project.Scripts.DataBase.Data;
using UnityEngine;

namespace Project.Scripts.Weapon.CharacteristicsOfWeapon
{
    [Serializable]
    public abstract class WeaponCharacteristics
    {
        public float RangeAttack;
        public float FireRate;
        public float ProjectileSpeed;
        public float Damage;
        public int MaxCountShots;
        public int MaxEnemiesInChain;
        public float ReloadTime;
        public float ExplosionRadius ;

        public abstract void ApplyImprovement(CharacteristicType type, float factor);

        public virtual void SetStartingCharacteristics(CharacteristicsWeaponData data) { }

        protected virtual void IncreaseDamage(float damageFactor)
        {
            Damage += Mathf.Round(Damage * damageFactor);
        }

        protected virtual void IncreaseFireRate(float fireRateFactor)
        {
            FireRate -= Mathf.Round(FireRate * fireRateFactor);
        }

        protected virtual void IncreaseBulletSpeed(float bulletSpeedFactor)
        {
            ProjectileSpeed += Mathf.Round(ProjectileSpeed * bulletSpeedFactor);
        }

        protected virtual void IncreaseRangeAttack(float rangeAttackFactor)
        {
            RangeAttack += Mathf.Round(RangeAttack * rangeAttackFactor);
        }

        protected virtual void IncreaseReloadVelocity(float reloadTimeFactor)
        {
            ReloadTime -= Mathf.Round(ReloadTime * reloadTimeFactor);
        }

        protected virtual void IncreaseMaxCountBullets(int maxCount)
        {
            MaxCountShots += maxCount;
        }

        protected virtual void IncreaseMaxEnemiesInChain(int maxEnemies)
        {
            MaxEnemiesInChain += maxEnemies;
        }

        protected virtual void IncreaseExplosionRadius(float explosionRadiusFactor)
        {
            ExplosionRadius += Mathf.Round(ExplosionRadius * explosionRadiusFactor);
        }
    }
}