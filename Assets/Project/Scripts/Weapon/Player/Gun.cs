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
    public class Gun : PlayerWeapon
    {
        private const string ObjectPoolBulletName = "PoolGunBullets";
        private const bool IsAutoExpandPool = true;

        [SerializeField] private GunBullet _bulletPrefab;
        [SerializeField] private int _countBullets;
        [SerializeField] private Transform _shootPoint;

        private GunBullet _bullet;
        private EnemyActor _closestEnemy;
        private ObjectPool<GunBullet> _poolBullets;

        private EnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;

        public GunCharacteristics GunCharacteristics { get; private set; } = new ();
        
        public void Construct(EnemyDetector detector, AudioSoundsService audioSoundsService,
            CharacteristicsWeaponData data, GunCharacteristics gunCharacteristics, WeaponPanel weaponPanel)
        {
            _detector = detector;
            _audioSoundsService = audioSoundsService;
            Type = data.WeaponType;
            WeaponPanel = weaponPanel;

            if (gunCharacteristics == null)
                GunCharacteristics.SetStartingCharacteristics(data);
            else
                GunCharacteristics = gunCharacteristics;
            
            YG2.saves.GunCharacteristics = GunCharacteristics;
            
            WeaponCharacteristics = GunCharacteristics;
            CurrentCountShots = GunCharacteristics.MaxCountShots;
            WeaponView = WeaponPanel.GetWeaponViewByType(Type);
            WeaponView.SetText(CurrentCountShots, GunCharacteristics.MaxCountShots);
        }

        private void Awake()
        {
            _poolBullets = new ObjectPool<GunBullet>(_bulletPrefab, _countBullets,
                new GameObject(ObjectPoolBulletName).transform)
            {
                AutoExpand = IsAutoExpandPool
            };
        }

        private void Update()
        {
            _closestEnemy = _detector.GetClosestEnemy();

            CheckAmmoAndReload();
            
            if (_closestEnemy == null) return;

            if (_detector.ClosestEnemyDistance <= GunCharacteristics.RangeAttack && CurrentCountShots > MinCountShots
                && !IsReloading)
            {
                Shoot();
            }
            
            LastShotTime -= Time.deltaTime;
        }
    
        public override void Shoot()
        {
            if (LastShotTime <= MinValue && _closestEnemy.Health.TargetHealth > MinValue)
            {
                CurrentCountShots--;
                _bullet = _poolBullets.GetFreeElement();
            
                _audioSoundsService.PlaySound(SoundsType.Gun).Forget();

                _bullet.transform.position = _shootPoint.position;

                _bullet.SetDirection(_closestEnemy.transform.position);
                _bullet.SetCharacteristics(GunCharacteristics.Damage, GunCharacteristics.ProjectileSpeed);

                LastShotTime = GunCharacteristics.FireRate;
                
                WeaponView.SetText(CurrentCountShots, GunCharacteristics.MaxCountShots);
            }
        }

        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
            WeaponView.SetText(CurrentCountShots, GunCharacteristics.MaxCountShots);
        }
    }
}
