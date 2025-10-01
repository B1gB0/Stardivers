using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Grenades;
using Project.Scripts.Services;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
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
        private EnemyAlienActor _closestAlienEnemy;

        private ObjectPool<FragGrenade> _poolGrenades;

        public FragGrenadeCharacteristics FragGrenadeCharacteristics { get; } = new ();

        public void Construct(EnemyDetector detector, AudioSoundsService audioSoundsService,
            CharacteristicsWeaponData data)
        {
            _detector = detector;
            _audioSoundsService = audioSoundsService;
            FragGrenadeCharacteristics.SetStartingCharacteristics(data);
            Type = data.WeaponType;
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
            _closestAlienEnemy = _detector.GetClosestEnemy();

            if (_closestAlienEnemy == null) return;
            
            if (_detector.ClosestEnemyDistance <= FragGrenadeCharacteristics.RangeAttack)
            {
                Shoot();
            }
        }
        
        public override void Shoot()
        {
            if (_lastShotTime <= MinValue && _closestAlienEnemy.Health.TargetHealth > MinValue)
            {
                _fragGrenade = _poolGrenades.GetFreeElement();
                _fragGrenade.GetExplosionEffects(_explosionEffect, _audioSoundsService);

                _fragGrenade.transform.position = _shootPoint.position;

                _fragGrenade.SetDirection(_closestAlienEnemy.transform.position);
                _fragGrenade.SetCharacteristics(FragGrenadeCharacteristics.Damage, FragGrenadeCharacteristics.ExplosionRadius,
                    FragGrenadeCharacteristics.ProjectileSpeed);

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