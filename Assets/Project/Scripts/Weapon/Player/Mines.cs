using Project.Scripts.Audio.Sounds;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Projectiles.Mines;
using Project.Scripts.Services;
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

        public MineCharacteristics MineCharacteristics { get; private set; } = new ();

        public void Construct(Button button, AudioSoundsService audioSoundsService, 
            CharacteristicsWeaponData data, MineCharacteristics mineCharacteristics, 
            ParticleEffectsService particleEffectsService)
        {
            _minesButton = button;
            _audioSoundsService = audioSoundsService;
            _particleEffectsService = particleEffectsService;
            Type = data.WeaponType;

            if (mineCharacteristics == null)
                MineCharacteristics.SetStartingCharacteristics(data);
            else
                MineCharacteristics = mineCharacteristics;

            YG2.saves.MinesCharacteristics = MineCharacteristics;
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
            _lastShotTime -= Time.fixedDeltaTime;
        }

        private void OnDestroy()
        {
            _minesButton.onClick.RemoveListener(Shoot);
        }

        public override void Shoot()
        {
            if (_lastShotTime <= MinValue)
            {
                _audioSoundsService.PlaySound(SoundsType.Button);
                
                _mine = _pool.GetFreeElement();

                _mine.GetExplosionEffects(_particleEffectsService, _audioSoundsService);
                
                _mine.transform.position = _installPoint.position;
                _mine.SetCharacteristics(MineCharacteristics.Damage, MineCharacteristics.ExplosionRadius);

                _lastShotTime = MineCharacteristics.FireRate;
            }
        }
        
        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
        }
    }
}