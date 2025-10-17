using System.Collections;
using System.Collections.Generic;
using Project.Game.Scripts;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Bullets;
using Project.Scripts.Services;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public class FourBarrelMachineGun : PlayerWeapon
    {
        private const string ObjectPoolBulletName = "PoolFourBarrelMachineGunBullets";
        private const bool IsAutoExpandPool = true;

        private const int MinValue = 0;
        private const float DelayBetweenShots = 0.1f;
        private const float MinRandomRangePosition = -0.2f;
        private const float MaxRandomRangePosition = 0.2f;
        private const int CountBullets = 4;

        private readonly List<Vector3> _directions = new();

        [SerializeField] private FourBarrelMachineGunBullet _bulletPrefab;
        [SerializeField] private int _countBulletsForPool;
        [SerializeField] private Transform _shootPoint;

        private float _lastBurstTime;
        private int _maxCountShots;
        private bool _isReloading;

        private Coroutine _coroutine;
        private FourBarrelMachineGunBullet _bullet;

        private AudioSoundsService _audioSoundsService;
        private EnemyDetector _detector;
        
        private EnemyActor _closestEnemy;

        private ObjectPool<FourBarrelMachineGunBullet> _poolBullets;

        public MachineGunCharacteristics MachineGunCharacteristics { get; } = new();

        public void Construct(AudioSoundsService audioSoundsService, EnemyDetector detector,
            CharacteristicsWeaponData data)
        {
            _audioSoundsService = audioSoundsService;
            _detector = detector;
            MachineGunCharacteristics.SetStartingCharacteristics(data);
            Type = data.WeaponType;
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

        private void Start()
        {
            _maxCountShots = MachineGunCharacteristics.MaxCountShots;
        }

        private void FixedUpdate()
        {
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
            if (_lastBurstTime <= MinValue)
            {
                _audioSoundsService.PlaySound(Sounds.FourBarrelMachineGun);

                foreach (Vector3 direction in _directions)
                {
                    StartCoroutine(LaunchBullet(direction));
                }

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

        private IEnumerator LaunchBullet(Vector3 direction)
        {
            for (int i = 0; i < CountBullets; i++)
            {
                _bullet = _poolBullets.GetFreeElement();

                _maxCountShots--;

                _bullet.transform.position = _shootPoint.position + Vector3.one
                    * Random.Range(MinRandomRangePosition, MaxRandomRangePosition);

                _bullet.SetDirection(direction);
                _bullet.SetCharacteristics(MachineGunCharacteristics.Damage, MachineGunCharacteristics.ProjectileSpeed);

                yield return new WaitForSeconds(DelayBetweenShots);
            }
        }
    }
}