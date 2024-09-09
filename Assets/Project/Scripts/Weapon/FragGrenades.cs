using System;
using System.Collections;
using Build.Game.Scripts.ECS.EntityActors;
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
        
        private readonly FragGrenadeCharacteristics _fragGrenadeCharacteristics = new ();
        
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private FragGrenade _fragGrenade;
        [SerializeField] private Transform _shootPoint;

        private ClosestEnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;
        
        private float _lastShotTime;
        private EnemyActor closestEnemy;

        private ObjectPool<FragGrenade> _poolGrenades;
        
        public void Construct(ClosestEnemyDetector detector, AudioSoundsService audioSoundsService)
        {
            _detector = detector;
            _audioSoundsService = audioSoundsService;
        }

        private void Awake()
        {
            _poolGrenades = new ObjectPool<FragGrenade>(_fragGrenade, CountGrenades, new GameObject(ObjectPoolGrenadeName).transform);
            
            _poolGrenades.AutoExpand = IsAutoExpandPool;
        }

        private void Start()
        {
            _explosionEffect = Instantiate(_explosionEffect);
            _explosionEffect.Stop();
        }

        private void FixedUpdate()
        {
            closestEnemy = _detector.СlosestEnemy;
        
            if (closestEnemy != null)
            {
                if (Vector3.Distance(closestEnemy.transform.position, transform.position) <= _fragGrenadeCharacteristics.RangeAttack)
                {
                    Shoot();
                }
            }
        }
        
        public override void Shoot()
        {
            if (_lastShotTime <= MinValue && closestEnemy.Health.Value > MinValue)
            {
                _fragGrenade = _poolGrenades.GetFreeElement();
                _fragGrenade.GetExplosionEffects(_explosionEffect, _audioSoundsService);

                _fragGrenade.transform.position = _shootPoint.position;

                _fragGrenade.SetDirection(closestEnemy.transform);
                _fragGrenade.SetCharacteristics(_fragGrenadeCharacteristics.Damage, _fragGrenadeCharacteristics.ExplosionRadius,
                    _fragGrenadeCharacteristics.GrenadeSpeed);

                _lastShotTime = _fragGrenadeCharacteristics.FireRate;
            }

            _lastShotTime -= Time.fixedDeltaTime;
        }
    }
}