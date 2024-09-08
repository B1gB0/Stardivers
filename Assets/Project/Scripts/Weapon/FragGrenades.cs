using System;
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
        
        private readonly GrenadeCharacteristics _grenadeCharacteristics = new ();
        
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private FragGrenade _fragGrenade;
        [SerializeField] private Transform _shootPoint;

        private float _lastShotTime;
        private ClosestEnemyDetector _detector;
        private EnemyActor closestEnemy;

        private ObjectPool<FragGrenade> _poolGrenades;
        
        public void Construct(ClosestEnemyDetector detector)
        {
            _detector = detector;
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
                if (Vector3.Distance(closestEnemy.transform.position, transform.position) <= _grenadeCharacteristics.RangeAttack)
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
                _fragGrenade.GetEffect(_explosionEffect);

                _fragGrenade.transform.position = _shootPoint.position;

                _fragGrenade.SetDirection(closestEnemy.transform);
                _fragGrenade.SetCharacteristics(_grenadeCharacteristics.Damage, _grenadeCharacteristics.ExplosionRadius,
                    _grenadeCharacteristics.GrenadeSpeed);

                _lastShotTime = _grenadeCharacteristics.FireRate;
            }

            _lastShotTime -= Time.fixedDeltaTime;
            
        }
    }
}