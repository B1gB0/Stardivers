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
    public class MachineGun : PlayerWeapon
    {
        private const string ObjectPoolBulletName = "PoolMachineGunBullets";
        private const bool IsAutoExpandPool = true;
    
        private const float MinValue = 0f;
        private const float DelayBetweenShots = 0.2f;

        [SerializeField] private MachineGunBullet _bulletPrefab;
        [SerializeField] private int _countBulletsForPool;
        [SerializeField] private Transform[] _shootPoints;

        private float _lastBurstTime;
        private int _maxCountShots;
        private bool _isReloading;

        private Coroutine _coroutine;
        private MachineGunBullet _bullet;

        private ImprovedEnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;
    
        private EnemyAlienActor closestAlienEnemy;
        private ObjectPool<MachineGunBullet> _poolBullets;

        public MachineGunCharacteristics MachineGunCharacteristics { get; } = new();

        public void Construct(ImprovedEnemyDetector detector, AudioSoundsService audioSoundsService)
        {
            _detector = detector;
            _audioSoundsService = audioSoundsService;
        }

        private void Awake()
        {
            _poolBullets = new ObjectPool<MachineGunBullet>(_bulletPrefab, _countBulletsForPool, new GameObject(ObjectPoolBulletName).transform)
            {
                AutoExpand = IsAutoExpandPool
            };
        }

        private void Start()
        {
            _maxCountShots = MachineGunCharacteristics.MaxCountShots;
        }

        private void FixedUpdate()
        {
            closestAlienEnemy = _detector.GetClosestEnemy();

            if (closestAlienEnemy == null) return;
        
            if (_detector.ClosestEnemyDistance <= MachineGunCharacteristics.RangeAttack && !_isReloading)
            {
                Shoot();
            }
        
            CheckAmmoAndReload();
        }
    
        public override void Shoot()
        {
            if (_lastBurstTime <= MinValue && closestAlienEnemy.Health.TargetHealth > MinValue)
            {
                _audioSoundsService.PlaySound(Sounds.MachineGun);
            
                StartCoroutine(LaunchBullet());
            
                _lastBurstTime = MachineGunCharacteristics.FireRate;
            }

            _lastBurstTime -= Time.fixedDeltaTime;
        }
    
        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
        }

        private void CheckAmmoAndReload()
        {
            if (_maxCountShots <= MinValue)
            {
                _isReloading = true;
                StartCoroutine(Reload());
            }
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(MachineGunCharacteristics.ReloadTime);

            _maxCountShots = MachineGunCharacteristics.MaxCountShots;
            _isReloading = false;
        }

        private IEnumerator LaunchBullet()
        {
            foreach (var shootPoint in _shootPoints)
            {
                _bullet = _poolBullets.GetFreeElement();

                _maxCountShots--;
            
                _bullet.transform.position = shootPoint.position;
                
                _bullet.SetDirection(closestAlienEnemy.transform);
                _bullet.SetCharacteristics(MachineGunCharacteristics.Damage, MachineGunCharacteristics.BulletSpeed);

                yield return new WaitForSeconds(DelayBetweenShots);
            }

            yield return null;
        }
    }
}
