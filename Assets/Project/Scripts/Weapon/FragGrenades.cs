using System;
using System.Collections;
using Build.Game.Scripts.ECS.EntityActors;
using Project.Game.Scripts.Improvements;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Grenades;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Game.Scripts
{
    public class FragGrenades : Weapon
    {
        private const string ObjectPoolGrenadeName = "PoolGrenades";
        private const int CountGrenades = 1;
        private const bool IsAutoExpandPool = true;
        private const float MinValue = 0f;

        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private FragGrenade _fragGrenade;
        [SerializeField] private Transform _shootPoint;

        private ClosestEnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;
        
        private float _lastShotTime;
        private EnemyAlienActor closestSmallAlienEnemy;

        private ObjectPool<FragGrenade> _poolGrenades;

        public FragGrenadeCharacteristics FragGrenadeCharacteristics { get; } = new ();

        public void Construct(ClosestEnemyDetector detector, AudioSoundsService audioSoundsService)
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
            closestSmallAlienEnemy = _detector.СlosestAlienEnemy;
        
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
        
        public override void Accept(IWeaponVisitor weaponVisitor, CharacteristicsTypes type, float value)
        {
            weaponVisitor.Visit(this, type, value);
        }
    }
}