using Project.Game.Scripts;
using Project.Scripts.Projectiles.Mines;
using Project.Scripts.Services;
using Project.Scripts.Weapon.Characteristics;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Weapon.Player
{
    public class Mines : PlayerWeapon
    {
        private const float MinValue = 0f;
        private const bool IsAutoExpandPool = true;

        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private int _countMines;
        [SerializeField] private Mine _mine;
        [SerializeField] private Transform _installPoint;
        
        private ObjectPool<Mine> _pool;
        private Button _minesButton;
        private AudioSoundsService _audioSoundsService;
        
        private float _lastShotTime;

        public MineCharacteristics MineCharacteristics { get; } = new ();

        public void Construct(Button button, AudioSoundsService audioSoundsService)
        {
            _minesButton = button;
            _audioSoundsService = audioSoundsService;
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
            _explosionEffect = Instantiate(_explosionEffect);
            _explosionEffect.Stop();
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
                _audioSoundsService.PlaySound(Sounds.Button);
                
                _mine = _pool.GetFreeElement();

                _mine.GetExplosionEffects(_explosionEffect, _audioSoundsService);
                
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