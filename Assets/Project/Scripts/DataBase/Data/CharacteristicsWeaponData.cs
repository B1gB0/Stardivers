using System;
using Project.Scripts.Weapon.Player;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class CharacteristicsWeaponData
    {
        [SerializeField] private string _id;
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private float _rangeAttack;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _damage;
        [SerializeField] private int _maxCountShots;
        [SerializeField] private float _reloadTime;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private int _maxEnemiesInChain;
        
        public string Id => _id;
        public WeaponType WeaponType => _weaponType;
        public float RangeAttack => _rangeAttack;
        public float FireRate => _fireRate;
        public float ProjectileSpeed => _projectileSpeed;
        public float Damage => _damage;
        public int MaxCountShots => _maxCountShots;
        public float ReloadTime => _reloadTime;
        public float ExplosionRadius => _explosionRadius;
        public int MaxEnemiesInChain => _maxEnemiesInChain;
    }
}