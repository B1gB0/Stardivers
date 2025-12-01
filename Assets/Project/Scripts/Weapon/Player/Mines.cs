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
        private const float MinValue = 0f;
        private const bool IsAutoExpandPool = true;
        
        [SerializeField] private int _countMines;
        [SerializeField] private Mine _mine;
        [SerializeField] private Transform _installPoint;
        
        private ObjectPool<Mine> _pool;
        private Button _minesButton;
        private AudioSoundsService _audioSoundsService;
        private ParticleEffectsService _particleEffectsService;
        
        private float _lastShotTime;
        private float _reloadTimer;
        
        private int _currentCountShots;
        private bool _isReloading;
        private WeaponView _weaponView;

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
            
            _currentCountShots = MineCharacteristics.MaxCountShots;
            _weaponView = WeaponPanel.GetWeaponViewByType(Type);
            _weaponView.SetText(_currentCountShots, MineCharacteristics.MaxCountShots);
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

        private void FixedUpdate()
        {
            if (_isReloading)
            {
                _weaponView.AnimateFiller(_reloadTimer, MineCharacteristics.ReloadTime);
            }
            
            CheckAmmoAndReload();
            
            _lastShotTime -= Time.fixedDeltaTime;
        }

        private void OnDestroy()
        {
            _minesButton.onClick.RemoveListener(Shoot);
        }

        public override void Shoot()
        {
            if (_lastShotTime <= MinValue && _currentCountShots > MinValue && !_isReloading)
            {
                _mine = _pool.GetFreeElement();
                _currentCountShots--;
                
                _audioSoundsService.PlaySound(SoundsType.Button).Forget();

                _mine.GetExplosionEffects(_particleEffectsService, _audioSoundsService);
                
                _mine.transform.position = _installPoint.position;
                _mine.SetCharacteristics(MineCharacteristics.Damage, MineCharacteristics.ExplosionRadius);

                _lastShotTime = MineCharacteristics.FireRate;
                
                _weaponView.SetText(_currentCountShots, MineCharacteristics.MaxCountShots);
            }
        }
        
        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
            _weaponView.SetText(_currentCountShots, MineCharacteristics.MaxCountShots);
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
            _reloadTimer = MinValue;
            
            _isReloading = true;
            _weaponView.ActivateFiller();

            while (_reloadTimer < MineCharacteristics.ReloadTime)
            {
                _reloadTimer += Time.fixedDeltaTime;
                yield return null;
            }

            _currentCountShots = MineCharacteristics.MaxCountShots;
            _isReloading = false;
            _weaponView.DeactivateFiller();
            _weaponView.SetText(_currentCountShots, MineCharacteristics.MaxCountShots);
            
            Debug.Log(_reloadTimer + " время перезарядки");
        }
    }
}