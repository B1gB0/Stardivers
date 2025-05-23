﻿using Project.Game.Scripts;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Grenades;
using Project.Scripts.Services;
using Project.Scripts.Weapon.Characteristics;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public class FragGrenades : PlayerWeapon
    {
        private const string ObjectPoolGrenadeName = "PoolGrenades";
        private const int CountGrenades = 1;
        private const bool IsAutoExpandPool = true;
        private const float MinValue = 0f;

        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private FragGrenade _fragGrenade;
        [SerializeField] private Transform _shootPoint;

        private EnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;
        
        private float _lastShotTime;
        private EnemyAlienActor closestSmallAlienEnemy;

        private ObjectPool<FragGrenade> _poolGrenades;

        public FragGrenadeCharacteristics FragGrenadeCharacteristics { get; } = new ();

        public void Construct(EnemyDetector detector, AudioSoundsService audioSoundsService)
        {
            _detector = detector;
            _audioSoundsService = audioSoundsService;
        }

        private void Awake()
        {
            _poolGrenades = new ObjectPool<FragGrenade>(_fragGrenade, CountGrenades, new GameObject(ObjectPoolGrenadeName).transform)
            {
                AutoExpand = IsAutoExpandPool
            };
        }

        private void Start()
        {
            _explosionEffect = Instantiate(_explosionEffect);
            _explosionEffect.Stop();
        }

        private void FixedUpdate()
        {
            closestSmallAlienEnemy = _detector.NearestAlienEnemy;
        
            if (closestSmallAlienEnemy != null)
            {
                if (Vector3.Distance(closestSmallAlienEnemy.transform.position, transform.position) <= FragGrenadeCharacteristics.RangeAttack)
                {
                    Shoot();
                }
            }
        }
        
        public override void Shoot()
        {
            if (_lastShotTime <= MinValue && closestSmallAlienEnemy.Health.TargetHealth > MinValue)
            {
                _fragGrenade = _poolGrenades.GetFreeElement();
                _fragGrenade.GetExplosionEffects(_explosionEffect, _audioSoundsService);

                _fragGrenade.transform.position = _shootPoint.position;

                _fragGrenade.SetDirection(closestSmallAlienEnemy.transform);
                _fragGrenade.SetCharacteristics(FragGrenadeCharacteristics.Damage, FragGrenadeCharacteristics.ExplosionRadius,
                    FragGrenadeCharacteristics.GrenadeSpeed);

                _lastShotTime = FragGrenadeCharacteristics.FireRate;
            }

            _lastShotTime -= Time.fixedDeltaTime;
        }
        
        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
        }
    }
}