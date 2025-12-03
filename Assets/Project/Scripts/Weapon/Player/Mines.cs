using System.Collections;
using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Projectiles.Mines;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.Weapon.Player
{
    public class Mines : PlayerWeapon
    {
        private const bool IsAutoExpandPool = true;
        
        [SerializeField] private int _countMines;
        [SerializeField] private Mine _mine;
        [SerializeField] private Transform _installPoint;
        
        private ObjectPool<Mine> _pool;
        private Button _minesButton;
        private AudioSoundsService _audioSoundsService;
        private ParticleEffectsService _particleEffectsService;

        public MineCharacteristics MineCharacteristics { get; private set; } = new ();

        public void Construct(Button button, AudioSoundsService audioSoundsService, 
            CharacteristicsWeaponData data, MineCharacteristics mineCharacteristics, 
            ParticleEffectsService particleEffectsService, WeaponPanel weaponPanel)
        {
            _minesButton = button;
            _audioSoundsService = audioSoundsService;
            _particleEffectsService = particleEffectsService;
            Type = data.WeaponType;
            WeaponPanel = weaponPanel;

            if (mineCharacteristics == null)
                MineCharacteristics.SetStartingCharacteristics(data);
            else
                MineCharacteristics = mineCharacteristics;

            YG2.saves.MinesCharacteristics = MineCharacteristics;

            WeaponCharacteristics = MineCharacteristics;
            CurrentCountShots = MineCharacteristics.MaxCountShots;
            WeaponView = WeaponPanel.GetWeaponViewByType(Type);
            WeaponView.SetText(CurrentCountShots, MineCharacteristics.MaxCountShots);
        }

        private void Awake()
        {
            _pool = new ObjectPool<Mine>(_mine, _countMines, new GameObject("PoolMines").transform)
            {
                AutoExpand = IsAutoExpandPool
            };
        }

        private void Start()
        {
            _minesButton.onClick.AddListener(Shoot);
        }

        private void Update()
        {
            CheckAmmoAndReload();
            
            LastShotTime -= Time.deltaTime;
        }

        private void OnDestroy()
        {
            _minesButton.onClick.RemoveListener(Shoot);
        }

        public override void Shoot()
        {
            if (LastShotTime <= MinValue && CurrentCountShots > MinValue && !IsReloading)
            {
                _mine = _pool.GetFreeElement();
                CurrentCountShots--;
                
                _audioSoundsService.PlaySound(SoundsType.Button).Forget();

                _mine.GetExplosionEffects(_particleEffectsService, _audioSoundsService);
                
                _mine.transform.position = _installPoint.position;
                _mine.SetCharacteristics(MineCharacteristics.Damage, MineCharacteristics.ExplosionRadius);

                LastShotTime = MineCharacteristics.FireRate;
                
                WeaponView.SetText(CurrentCountShots, MineCharacteristics.MaxCountShots);
            }
        }
        
        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
            WeaponView.SetText(CurrentCountShots, MineCharacteristics.MaxCountShots);
        }
    }
}