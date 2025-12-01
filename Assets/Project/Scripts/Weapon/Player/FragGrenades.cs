using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Grenades;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;
using YG;

namespace Project.Scripts.Weapon.Player
{
    public class FragGrenades : PlayerWeapon
    {
        private const string ObjectPoolGrenadeName = "PoolGrenades";
        private const int CountGrenades = 1;
        private const bool IsAutoExpandPool = true;

        [SerializeField] private FragGrenade _fragGrenade;
        [SerializeField] private Transform _shootPoint;

        private EnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;
        private ParticleEffectsService _particleEffectsService;
        private EnemyActor _closestEnemy;

        private ObjectPool<FragGrenade> _poolGrenades;

        public FragGrenadeCharacteristics FragGrenadeCharacteristics { get; private set; } = new ();

        public void Construct(EnemyDetector detector, AudioSoundsService audioSoundsService,
            CharacteristicsWeaponData data, FragGrenadeCharacteristics fragGrenadeCharacteristics, 
            ParticleEffectsService particleEffectsService, WeaponPanel weaponPanel)
        {
            _detector = detector;
            _audioSoundsService = audioSoundsService;
            _particleEffectsService = particleEffectsService;
            Type = data.WeaponType;
            WeaponPanel = weaponPanel;

            WeaponCharacteristics = FragGrenadeCharacteristics;
            
            if(fragGrenadeCharacteristics == null)
                FragGrenadeCharacteristics.SetStartingCharacteristics(data);
            else
                FragGrenadeCharacteristics = fragGrenadeCharacteristics;

            YG2.saves.FragGrenadeCharacteristics = FragGrenadeCharacteristics;
            
            CurrentCountShots = FragGrenadeCharacteristics.MaxCountShots;
            WeaponView = WeaponPanel.GetWeaponViewByType(Type);
            WeaponView.SetText(CurrentCountShots, FragGrenadeCharacteristics.MaxCountShots);
        }

        private void Awake()
        {
            _poolGrenades = new ObjectPool<FragGrenade>(_fragGrenade, CountGrenades, new GameObject(ObjectPoolGrenadeName).transform)
            {
                AutoExpand = IsAutoExpandPool
            };
        }

        private void Update()
        {
            _closestEnemy = _detector.GetClosestEnemy();
            
            if (IsReloading)
            {
                WeaponView.AnimateFiller(ReloadTimer, FragGrenadeCharacteristics.ReloadTime);
            }
            
            CheckAmmoAndReload();

            if (_closestEnemy == null) return;
            
            if (_detector.ClosestEnemyDistance <= FragGrenadeCharacteristics.RangeAttack && !IsReloading)
            {
                Shoot();
            }
        }
        
        public override void Shoot()
        {
            if (LastShotTime <= MinValue && _closestEnemy.Health.TargetHealth > MinValue)
            {
                CurrentCountShots--;
                WeaponView.SetText(CurrentCountShots, FragGrenadeCharacteristics.MaxCountShots);
                
                _fragGrenade = _poolGrenades.GetFreeElement();
                _fragGrenade.GetExplosionEffects(_particleEffectsService, _audioSoundsService);

                _fragGrenade.transform.position = _shootPoint.position;

                _fragGrenade.SetDirection(_closestEnemy.transform.position);
                _fragGrenade.SetCharacteristics(FragGrenadeCharacteristics.Damage, FragGrenadeCharacteristics.ExplosionRadius,
                    FragGrenadeCharacteristics.ProjectileSpeed);

                LastShotTime = FragGrenadeCharacteristics.FireRate;
            }

            LastShotTime -= Time.deltaTime;
        }
        
        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
            WeaponView.SetText(CurrentCountShots, FragGrenadeCharacteristics.MaxCountShots);
        }
    }
}