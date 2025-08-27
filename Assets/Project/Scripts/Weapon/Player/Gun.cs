using System.Collections;
using Project.Game.Scripts;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Bullets;
using Project.Scripts.Services;
using Project.Scripts.Weapon.Characteristics;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public class Gun : PlayerWeapon
    {
        private const string ObjectPoolBulletName = "PoolGunBullets";
        private const bool IsAutoExpandPool = true;
        private const float MinValue = 0f;

        [SerializeField] private GunBullet _bulletPrefab;
        [SerializeField] private int _countBullets;
        [SerializeField] private Transform _shootPoint;

        private float _lastShotTime;
        private int _maxCountShots;
        private bool _isReloading;
    
        private GunBullet _bullet;
        private EnemyAlienActor _closestAlienEnemy;
        private ObjectPool<GunBullet> _poolBullets;

        private ImprovedEnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;

        public GunCharacteristics GunCharacteristics { get; } = new();

        public void Construct(ImprovedEnemyDetector detector, AudioSoundsService audioSoundsService)
        {
            _detector = detector;
            _audioSoundsService = audioSoundsService;
        }

        private void Awake()
        {
            _poolBullets = new ObjectPool<GunBullet>(_bulletPrefab, _countBullets,
                new GameObject(ObjectPoolBulletName).transform)
            {
                AutoExpand = IsAutoExpandPool
            };
            
            GunCharacteristics.SetStartingCharacteristics();
        }

        private void FixedUpdate()
        {
            _closestAlienEnemy = _detector.GetClosestEnemy();

            if (_closestAlienEnemy == null) return;

            if (_detector.ClosestEnemyDistance <= GunCharacteristics.RangeAttack && !_isReloading)
            {
                Shoot();
            }
        
            CheckAmmoAndReload();
        }
    
        public override void Shoot()
        {
            if (_lastShotTime <= MinValue && _closestAlienEnemy.Health.TargetHealth > MinValue)
            {
                _bullet = _poolBullets.GetFreeElement();
            
                _audioSoundsService.PlaySound(Sounds.Gun);

                _bullet.transform.position = _shootPoint.position;

                _bullet.SetDirection(_closestAlienEnemy.transform);
                _bullet.SetCharacteristics(GunCharacteristics.Damage, GunCharacteristics.ProjectileSpeed);

                _lastShotTime = GunCharacteristics.FireRate;
            }

            _lastShotTime -= Time.fixedDeltaTime;
        }

        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
        }
    
        private void CheckAmmoAndReload()
        {
            if (_maxCountShots <= MinValue && _isReloading)
            {
                _isReloading = false;
                StartCoroutine(Reload());
            }
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(GunCharacteristics.ReloadTime);

            _maxCountShots = GunCharacteristics.MaxCountBullets;
            _isReloading = true;
        }
    }
}
