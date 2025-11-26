using System.Collections;
using System.Collections.Generic;
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
    public class FourBarrelMachineGun : PlayerWeapon
    {
        private const string ObjectPoolBulletName = "PoolFourBarrelMachineGunBullets";
        private const bool IsAutoExpandPool = true;

        private const int MinValue = 0;
        private const int CountBullets = 4;
        
        private const float DelayBetweenShots = 0.1f;
        private const float MinRandomRangePosition = -0.2f;
        private const float MaxRandomRangePosition = 0.2f;

        private readonly List<Vector3> _directions = new();

        [SerializeField] private FourBarrelMachineGunBullet _bulletPrefab;
        [SerializeField] private int _countBulletsForPool;
        [SerializeField] private Transform _shootPoint;

        private float _lastBurstTime;
        private int _currentCountShots;
        private bool _isReloading;

        private Coroutine _coroutine;
        private FourBarrelMachineGunBullet _bullet;

        private AudioSoundsService _audioSoundsService;
        private EnemyDetector _detector;

        private EnemyActor _closestEnemy;

        private ObjectPool<FourBarrelMachineGunBullet> _poolBullets;

        public FourBarrelMachineGunCharacteristics FourBarrelMachineGunCharacteristics { get; private set; } = new();

        public void Construct(AudioSoundsService audioSoundsService, EnemyDetector detector,
            CharacteristicsWeaponData data, FourBarrelMachineGunCharacteristics fourBarrelMachineGunCharacteristics)
        {
            _audioSoundsService = audioSoundsService;
            _detector = detector;
            Type = data.WeaponType;

            if (fourBarrelMachineGunCharacteristics == null)
                FourBarrelMachineGunCharacteristics.SetStartingCharacteristics(data);
            
            else
                FourBarrelMachineGunCharacteristics = fourBarrelMachineGunCharacteristics;

            YG2.saves.FourBarrelMachineGunCharacteristics = FourBarrelMachineGunCharacteristics;
            
            _currentCountShots = FourBarrelMachineGunCharacteristics.MaxCountShots;
        }

        private void Awake()
        {
            _poolBullets = new ObjectPool<FourBarrelMachineGunBullet>(_bulletPrefab, _countBulletsForPool,
                new GameObject(ObjectPoolBulletName).transform);
            _poolBullets.AutoExpand = IsAutoExpandPool;

            _directions.Add(transform.forward);
            _directions.Add(transform.right);
            _directions.Add(-transform.forward);
            _directions.Add(-transform.right);
        }

        private void FixedUpdate()
        {
            _closestEnemy = _detector.GetClosestEnemy();

            if (_closestEnemy == null) return;

            if (_detector.ClosestEnemyDistance <= FourBarrelMachineGunCharacteristics.RangeAttack && !_isReloading)
            {
                Shoot();
            }

            CheckAmmoAndReload();
        }

        public override void Shoot()
        {
            if (_lastBurstTime <= MinValue)
            {
                _audioSoundsService.PlaySound(SoundsType.FourBarrelMachineGun).Forget();

                foreach (Vector3 direction in _directions)
                {
                    StartCoroutine(LaunchBullet(direction));
                }

                _lastBurstTime = FourBarrelMachineGunCharacteristics.FireRate;
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
            yield return new WaitForSeconds(FourBarrelMachineGunCharacteristics.ReloadTime);

            _currentCountShots = FourBarrelMachineGunCharacteristics.MaxCountShots;
            _isReloading = false;
        }

        private IEnumerator LaunchBullet(Vector3 direction)
        {
            for (int i = MinValue; i < CountBullets; i++)
            {
                _bullet = _poolBullets.GetFreeElement();

                _currentCountShots--;

                _bullet.transform.position = _shootPoint.position + Vector3.one
                    * Random.Range(MinRandomRangePosition, MaxRandomRangePosition);

                _bullet.SetDirection(direction);
                _bullet.SetCharacteristics(FourBarrelMachineGunCharacteristics.Damage,
                    FourBarrelMachineGunCharacteristics.ProjectileSpeed);

                yield return new WaitForSeconds(DelayBetweenShots);
            }
        }
    }
}