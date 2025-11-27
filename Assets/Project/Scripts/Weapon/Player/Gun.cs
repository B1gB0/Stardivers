using System.Collections;
using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Bullets;
using Project.Scripts.Services;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;
using YG;

namespace Project.Scripts.Weapon.Player
{
    public class Gun : PlayerWeapon
    {
        private const string ObjectPoolBulletName = "PoolGunBullets";
        private const bool IsAutoExpandPool = true;
        private const float MinValue = 0f;
        private const int MinCountShots = 0;

        [SerializeField] private GunBullet _bulletPrefab;
        [SerializeField] private int _countBullets;
        [SerializeField] private Transform _shootPoint;

        private float _lastShotTime;
        private int _currentCountShots;
        private bool _isReloading;
    
        private GunBullet _bullet;
        private EnemyActor _closestEnemy;
        private ObjectPool<GunBullet> _poolBullets;

        private EnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;

        public GunCharacteristics GunCharacteristics { get; private set; } = new ();
        
        public void Construct(EnemyDetector detector, AudioSoundsService audioSoundsService,
            CharacteristicsWeaponData data, GunCharacteristics gunCharacteristics)
        {
            _detector = detector;
            _audioSoundsService = audioSoundsService;
            Type = data.WeaponType;

            if (gunCharacteristics == null)
                GunCharacteristics.SetStartingCharacteristics(data);
            else
                GunCharacteristics = gunCharacteristics;
            
            YG2.saves.GunCharacteristics = GunCharacteristics;
            
            _currentCountShots = GunCharacteristics.MaxCountShots;
        }

        private void Awake()
        {
            _poolBullets = new ObjectPool<GunBullet>(_bulletPrefab, _countBullets,
                new GameObject(ObjectPoolBulletName).transform)
            {
                AutoExpand = IsAutoExpandPool
            };
        }

        private void FixedUpdate()
        {
            _closestEnemy = _detector.GetClosestEnemy();

            if (_closestEnemy == null) return;

            if (_detector.ClosestEnemyDistance <= GunCharacteristics.RangeAttack && _currentCountShots > MinCountShots
                && !_isReloading)
            {
                Shoot();
            }
        
            CheckAmmoAndReload();
            
            _lastShotTime -= Time.fixedDeltaTime;
        }
    
        public override void Shoot()
        {
            if (_lastShotTime <= MinValue && _closestEnemy.Health.TargetHealth > MinValue)
            {
                _currentCountShots--;
                _bullet = _poolBullets.GetFreeElement();
            
                _audioSoundsService.PlaySound(SoundsType.Gun).Forget();

                _bullet.transform.position = _shootPoint.position;

                _bullet.SetDirection(_closestEnemy.transform.position);
                _bullet.SetCharacteristics(GunCharacteristics.Damage, GunCharacteristics.ProjectileSpeed);

                _lastShotTime = GunCharacteristics.FireRate;
            }
        }

        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
        }
    
        private void CheckAmmoAndReload()
        {
            if (_currentCountShots <= MinValue && !_isReloading)
            {
                StartCoroutine(Reload());
            }
        }

        private IEnumerator Reload()
        {
            _isReloading = true;
            yield return new WaitForSeconds(GunCharacteristics.ReloadTime);

            _currentCountShots = GunCharacteristics.MaxCountShots;
            _isReloading = false;
        }
    }
}
