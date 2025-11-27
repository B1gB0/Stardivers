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
        private int _currentCountShots;
        private bool _isReloading;

        private Coroutine _coroutine;
        private MachineGunBullet _bullet;

        private EnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;
    
        private EnemyActor _closestEnemy;
        private ObjectPool<MachineGunBullet> _poolBullets;

        public MachineGunCharacteristics MachineGunCharacteristics { get; private set; } = new();

        public void Construct(EnemyDetector detector, AudioSoundsService audioSoundsService,
            CharacteristicsWeaponData data, MachineGunCharacteristics machineGunCharacteristics)
        {
            _detector = detector;
            _audioSoundsService = audioSoundsService;
            Type = data.WeaponType;
            
            if(machineGunCharacteristics == null)
                MachineGunCharacteristics.SetStartingCharacteristics(data);
            else
                MachineGunCharacteristics = machineGunCharacteristics;

            YG2.saves.MachineGunCharacteristics = MachineGunCharacteristics;
            
            _currentCountShots = MachineGunCharacteristics.MaxCountShots;
        }

        private void Awake()
        {
            _poolBullets = new ObjectPool<MachineGunBullet>(_bulletPrefab, _countBulletsForPool, new GameObject(ObjectPoolBulletName).transform)
            {
                AutoExpand = IsAutoExpandPool
            };
        }

        private void FixedUpdate()
        {
            Debug.Log(_currentCountShots);
            Debug.Log(_isReloading);
            
            _closestEnemy = _detector.GetClosestEnemy();

            if (_closestEnemy == null) return;
        
            if (_detector.ClosestEnemyDistance <= MachineGunCharacteristics.RangeAttack && !_isReloading)
            {
                Shoot();
            }
        
            CheckAmmoAndReload();
        }
    
        public override void Shoot()
        {
            if (_lastBurstTime <= MinValue && _closestEnemy.Health.TargetHealth > MinValue)
            {
                _audioSoundsService.PlaySound(SoundsType.MachineGun).Forget();
            
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
            if (_currentCountShots <= MinValue && !_isReloading)
            {
                StartCoroutine(Reload());
            }
        }

        private IEnumerator Reload()
        {
            _isReloading = true;
            yield return new WaitForSeconds(MachineGunCharacteristics.ReloadTime);

            _currentCountShots = MachineGunCharacteristics.MaxCountShots;
            _isReloading = false;
        }

        private IEnumerator LaunchBullet()
        {
            foreach (var shootPoint in _shootPoints)
            {
                _bullet = _poolBullets.GetFreeElement();

                _currentCountShots--;
            
                _bullet.transform.position = shootPoint.position;

                if (_closestEnemy == null)
                {
                    _bullet.gameObject.SetActive(false);
                    continue;
                }
                
                _bullet.SetDirection(_closestEnemy.transform.position);
                _bullet.SetCharacteristics(MachineGunCharacteristics.Damage, MachineGunCharacteristics.ProjectileSpeed);

                yield return new WaitForSeconds(DelayBetweenShots);
            }

            yield return null;
        }
    }
}
