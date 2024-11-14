using System.Collections;
using Project.Game.Scripts;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Bullets;
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
        private bool _isShooting = true;
    
        private GunBullet _bullet;
        private EnemyAlienActor closestAlienEnemy;
        private ObjectPool<GunBullet> _poolBullets;

        private ClosestEnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;

        public GunCharacteristics GunCharacteristics { get; } = new();

        public void Construct(ClosestEnemyDetector detector, AudioSoundsService audioSoundsService)
        {
            _detector = detector;
            _audioSoundsService = audioSoundsService;
        }

        private void Awake()
        {
            _poolBullets = new ObjectPool<GunBullet>(_bulletPrefab, _countBullets, new GameObject(ObjectPoolBulletName).transform)
            {
                AutoExpand = IsAutoExpandPool
            };
        }

        private void FixedUpdate()
        {
            closestAlienEnemy = _detector.СlosestAlienEnemy;

            if (closestAlienEnemy == null) return;
        
            if (Vector3.Distance(closestAlienEnemy.transform.position, transform.position) <= GunCharacteristics.RangeAttack && _isShooting)
            {
                Shoot();
            }
        
            CheckAmmoAndReload();
        }
    
        public override void Shoot()
        {
            if (_lastShotTime <= MinValue && closestAlienEnemy.Health.TargetHealth > MinValue)
            {
                _bullet = _poolBullets.GetFreeElement();
            
                _audioSoundsService.PlaySound(Sounds.Gun);

                _bullet.transform.position = _shootPoint.position;

                _bullet.SetDirection(closestAlienEnemy.transform);
                _bullet.SetCharacteristics(GunCharacteristics.Damage, GunCharacteristics.BulletSpeed);

                _lastShotTime = GunCharacteristics.FireRate;
            }

            _lastShotTime -= Time.fixedDeltaTime;
        }

        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicsTypes type, float value)
        {
            weaponVisitor.Visit(this, type, value);
        }
    
        private void CheckAmmoAndReload()
        {
            if (_maxCountShots <= MinValue)
            {
                _isShooting = false;
                StartCoroutine(Reload());
            }
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(GunCharacteristics.ReloadTime);

            _maxCountShots = GunCharacteristics.MaxCountShots;
            _isShooting = true;
        }
    }
}
