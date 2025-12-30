using System.Collections;
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
    public class MachineGun : PlayerWeapon
    {
        private const string ObjectPoolBulletName = "PoolMachineGunBullets";
        private const bool IsAutoExpandPool = true;
        private const float DelayBetweenShots = 0.2f;

        [SerializeField] private MachineGunBullet _bulletPrefab;
        [SerializeField] private int _countBulletsForPool;
        [SerializeField] private Transform[] _shootPoints;

        private MachineGunBullet _bullet;
        private EnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;
        private EnemyActor _closestEnemy;

        private ObjectPool<MachineGunBullet> _poolBullets;

        public MachineGunCharacteristics MachineGunCharacteristics { get; private set; } = new();

        private void Awake()
        {
            _poolBullets = new ObjectPool<MachineGunBullet>(
                _bulletPrefab,
                _countBulletsForPool,
                new GameObject(ObjectPoolBulletName).transform)
            {
                AutoExpand = IsAutoExpandPool,
            };
        }

        private void Update()
        {
            _closestEnemy = _detector.GetClosestEnemy();

            CheckAmmoAndReload();

            if (_closestEnemy == null)
                return;

            if (_detector.ClosestEnemyDistance <= MachineGunCharacteristics.RangeAttack && !IsReloading)
            {
                Shoot();
            }
        }

        public void Construct(
            EnemyDetector detector,
            AudioSoundsService audioSoundsService,
            CharacteristicsWeaponData data,
            MachineGunCharacteristics machineGunCharacteristics,
            WeaponPanel weaponPanel)
        {
            _detector = detector;
            _audioSoundsService = audioSoundsService;
            Type = data.WeaponType;
            WeaponPanel = weaponPanel;

            if (machineGunCharacteristics == null)
                MachineGunCharacteristics.SetStartingCharacteristics(data);
            else
                MachineGunCharacteristics = machineGunCharacteristics;

            YG2.saves.MachineGunCharacteristics = MachineGunCharacteristics;

            WeaponCharacteristics = MachineGunCharacteristics;
            CurrentCountShots = MachineGunCharacteristics.MaxCountShots;
            WeaponView = WeaponPanel.GetWeaponViewByType(Type);
            WeaponView.SetText(CurrentCountShots, MachineGunCharacteristics.MaxCountShots);
        }

        public override void Shoot()
        {
            if (LastShotTime <= MinValue && _closestEnemy.Health.TargetHealth > MinValue)
            {
                _audioSoundsService.PlaySound(SoundsType.MachineGun).Forget();

                StartCoroutine(LaunchBullet());

                LastShotTime = MachineGunCharacteristics.FireRate;
            }

            LastShotTime -= Time.deltaTime;
        }

        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
            WeaponView.SetText(CurrentCountShots, MachineGunCharacteristics.MaxCountShots);
        }

        private IEnumerator LaunchBullet()
        {
            foreach (var shootPoint in _shootPoints)
            {
                _bullet = _poolBullets.GetFreeElement();

                CurrentCountShots--;
                WeaponView.SetText(CurrentCountShots, MachineGunCharacteristics.MaxCountShots);

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