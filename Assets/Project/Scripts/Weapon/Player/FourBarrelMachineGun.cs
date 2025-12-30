using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Bullets;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
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
        private const int CountBullets = 4;

        private const float DelayBetweenShots = 0.1f;
        private const float MinRandomRangePosition = -0.2f;
        private const float MaxRandomRangePosition = 0.2f;

        private readonly List<Vector3> _directions = new();

        [SerializeField] private FourBarrelMachineGunBullet _bulletPrefab;
        [SerializeField] private int _countBulletsForPool;
        [SerializeField] private Transform _shootPoint;

        private FourBarrelMachineGunBullet _bullet;
        private AudioSoundsService _audioSoundsService;
        private EnemyDetector _detector;
        private EnemyActor _closestEnemy;

        private ObjectPool<FourBarrelMachineGunBullet> _poolBullets;

        public FourBarrelMachineGunCharacteristics FourBarrelMachineGunCharacteristics { get; private set; } = new();

        public void Construct(
            AudioSoundsService audioSoundsService,
            EnemyDetector detector,
            CharacteristicsWeaponData data,
            FourBarrelMachineGunCharacteristics fourBarrelMachineGunCharacteristics,
            WeaponPanel weaponPanel)
        {
            _audioSoundsService = audioSoundsService;
            _detector = detector;
            Type = data.WeaponType;
            WeaponPanel = weaponPanel;

            if (fourBarrelMachineGunCharacteristics == null)
                FourBarrelMachineGunCharacteristics.SetStartingCharacteristics(data);
            else
                FourBarrelMachineGunCharacteristics = fourBarrelMachineGunCharacteristics;

            YG2.saves.FourBarrelMachineGunCharacteristics = FourBarrelMachineGunCharacteristics;

            WeaponCharacteristics = FourBarrelMachineGunCharacteristics;
            CurrentCountShots = FourBarrelMachineGunCharacteristics.MaxCountShots;
            WeaponView = WeaponPanel.GetWeaponViewByType(Type);
            WeaponView.SetText(CurrentCountShots, FourBarrelMachineGunCharacteristics.MaxCountShots);
        }

        private void Awake()
        {
            _poolBullets = new ObjectPool<FourBarrelMachineGunBullet>(
                _bulletPrefab,
                _countBulletsForPool,
                new GameObject(ObjectPoolBulletName).transform)
            {
                AutoExpand = IsAutoExpandPool,
            };

            _directions.Add(transform.forward);
            _directions.Add(transform.right);
            _directions.Add(-transform.forward);
            _directions.Add(-transform.right);
        }

        private void Update()
        {
            _closestEnemy = _detector.GetClosestEnemy();

            CheckAmmoAndReload();

            if (_closestEnemy == null)
                return;

            if (_detector.ClosestEnemyDistance <= FourBarrelMachineGunCharacteristics.RangeAttack && !IsReloading)
            {
                Shoot();
            }
        }

        public override void Shoot()
        {
            if (LastShotTime <= MinValue)
            {
                _audioSoundsService.PlaySound(SoundsType.FourBarrelMachineGun).Forget();

                foreach (Vector3 direction in _directions)
                {
                    StartCoroutine(LaunchBullet(direction));
                }

                LastShotTime = FourBarrelMachineGunCharacteristics.FireRate;
            }

            LastShotTime -= Time.deltaTime;
        }

        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
            WeaponView.SetText(CurrentCountShots, FourBarrelMachineGunCharacteristics.MaxCountShots);
        }

        private IEnumerator LaunchBullet(Vector3 direction)
        {
            for (int i = MinCountShots; i < CountBullets; i++)
            {
                _bullet = _poolBullets.GetFreeElement();

                CurrentCountShots--;
                WeaponView.SetText(CurrentCountShots, FourBarrelMachineGunCharacteristics.MaxCountShots);

                _bullet.transform.position = _shootPoint.position +
                                             (Vector3.one * Random.Range(
                                                 MinRandomRangePosition,
                                                 MaxRandomRangePosition));

                _bullet.SetDirection(direction);

                _bullet.SetCharacteristics(
                    FourBarrelMachineGunCharacteristics.Damage,
                    FourBarrelMachineGunCharacteristics.ProjectileSpeed);

                yield return new WaitForSeconds(DelayBetweenShots);
            }
        }
    }
}