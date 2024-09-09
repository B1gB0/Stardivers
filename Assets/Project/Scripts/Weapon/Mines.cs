using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Game.Scripts
{
    public class Mines : Weapon
    {
        private const float MinValue = 0f;
        private const bool IsAutoExpandPool = true;
        
        private readonly MineCharacteristics _mineCharacteristics = new ();

        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private int _countMines;
        [SerializeField] private Mine _mine;
        [SerializeField] private Transform _installPoint;
        
        private ObjectPool<Mine> _pool;
        
        private Button _minesButton;
        private AudioSoundsService _audioSoundsService;
        
        private float _lastShotTime;

        public void Construct(Button button, AudioSoundsService audioSoundsService)
        {
            _minesButton = button;
            _audioSoundsService = audioSoundsService;
        }

        private void Awake()
        {
            _pool = new ObjectPool<Mine>(_mine, _countMines, new GameObject("PoolMines").transform);
            _pool.AutoExpand = IsAutoExpandPool;
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
                _mine = _pool.GetFreeElement();

                _mine.GetExplosionEffects(_explosionEffect, _audioSoundsService);
                
                _mine.transform.position = _installPoint.position;
                _mine.SetCharacteristics(_mineCharacteristics.Damage, _mineCharacteristics.ExplosionRadius);

                _lastShotTime = _mineCharacteristics.FireRate;
            }
        }
    }
}