using System;
using System.Collections;
using System.Collections.Generic;
using Project.Game.Scripts;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Lightning;
using Project.Scripts.Services;
using Project.Scripts.Weapon.Characteristics;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public class ChainLightningGun : PlayerWeapon
    {
        private const string ObjectPoolBulletName = "PoolChainLightningGunLinesRenderers";
        private const bool IsAutoExpandPool = true;
        private const float MinValue = 0f;
        private const int MinEnemiesInChain = 1;
        
        private readonly List<EnemyAlienActor> _enemiesInChain = new ();
        private readonly List<LightningProjectile> _spawnedLineRenderers = new ();
        
        [SerializeField] private Transform _shootPoint;
        [SerializeField] [Range(1, 10)] private int _maxEnemiesInChain = 3;
        [SerializeField] private LightningProjectile _lineRendererPrefab;
        [SerializeField] private float _refreshRate = 0.01f;
        [SerializeField] private float _delayBetweenChain = 0.2f;
        [SerializeField] private int _countCharges;
        
        private ImprovedEnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;
        
        private float _lastShotTime;
        private int _counter = 1;
        private int _maxCountShots;
        private bool _isReloading;
        
        private EnemyAlienActor _closestAlienEnemy;
        private ObjectPool<LightningProjectile> _poolLineRendererSetters;

        public GunCharacteristics GunCharacteristics { get; } = new ();
        
        public void Construct(AudioSoundsService audioSoundsService, ImprovedEnemyDetector enemyDetector)
        {
            _audioSoundsService = audioSoundsService;
            _detector = enemyDetector;
        }

        private void Awake()
        {
            _poolLineRendererSetters = new ObjectPool<LightningProjectile>(_lineRendererPrefab, _countCharges,
                new GameObject(ObjectPoolBulletName).transform)
            {
                AutoExpand = IsAutoExpandPool
            };
        }

        private void FixedUpdate()
        {
            _closestAlienEnemy = _detector.GetClosestEnemy();

            if (_detector.GetEnemiesInRange().Count > MinValue && _closestAlienEnemy != null)
            {
                if (_detector.ClosestEnemyDistance <= GunCharacteristics.RangeAttack && !_isReloading)
                {
                    Shoot();
                }
                else
                {
                    StopShooting();
                }
            }

            CheckAmmoAndReload();
        }

        public override void Shoot()
        {
            if (_lastShotTime <= MinValue && _closestAlienEnemy.Health.TargetHealth > MinValue)
            {
                _audioSoundsService.PlaySound(Sounds.Gun);
                
                CreateLightning(_shootPoint, _closestAlienEnemy.transform, true);

                if (_maxEnemiesInChain > MinEnemiesInChain)
                {
                    StartCoroutine(ChainReaction(_closestAlienEnemy));
                }

                _lastShotTime = GunCharacteristics.FireRate;
            }

            _lastShotTime -= Time.fixedDeltaTime;
        }

        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            
        }

        private void CreateLightning(Transform startPoint, Transform endPoint, bool fromPlayer = false)
        {
            LightningProjectile lightningProjectile = _poolLineRendererSetters.GetFreeElement();
            _spawnedLineRenderers.Add(lightningProjectile);
            StartCoroutine(UpdateLineRenderer(lightningProjectile, startPoint, endPoint, fromPlayer));
        }

        private IEnumerator UpdateLineRenderer(LightningProjectile lightningProjectile, Transform startPoint,
            Transform endPoint, bool fromPlayer = false)
        {
            lightningProjectile.GetComponent<LightningProjectile>().SetPosition(startPoint, endPoint);

            yield return new WaitForSeconds(_refreshRate);

            if (fromPlayer)
            {
                StartCoroutine(UpdateLineRenderer(lightningProjectile, startPoint, 
                    _detector.GetClosestEnemy().transform, true));

                if (_closestAlienEnemy != _detector.GetClosestEnemy())
                {
                    StopShooting();
                    Shoot();
                }
            }
            else
            {
                StartCoroutine(UpdateLineRenderer(lightningProjectile, startPoint, endPoint));
            }
        }

        private IEnumerator ChainReaction(EnemyAlienActor closestEnemy)
        {
            yield return new WaitForSeconds(_delayBetweenChain);

            if (_counter == _maxEnemiesInChain)
            {
                yield return null;
            }
            else
            {
                _counter++;
                
                _enemiesInChain.Add(closestEnemy);

                if (!_enemiesInChain.Contains(closestEnemy.GetComponent<ImprovedEnemyDetector>().GetClosestEnemy()))
                {
                    CreateLightning(closestEnemy.transform, closestEnemy.GetComponent<ImprovedEnemyDetector>()
                        .GetClosestEnemy().transform);
                    StartCoroutine(ChainReaction(closestEnemy.GetComponent<ImprovedEnemyDetector>().GetClosestEnemy()));
                }
            }
        }

        private void StopShooting()
        {
            _counter = 1;
            
            for (int i = 0; i < _spawnedLineRenderers.Count; i++)
            {
                _spawnedLineRenderers[i].gameObject.SetActive(false);
            }
            
            _spawnedLineRenderers.Clear();
            _enemiesInChain.Clear();
        }
        
        private void CheckAmmoAndReload()
        {
            StopShooting();
            
            if (_maxCountShots <= MinValue)
            {
                _isReloading = true;
                StartCoroutine(Reload());
            }
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(GunCharacteristics.ReloadTime);

            _maxCountShots = GunCharacteristics.MaxCountBullets;
            _isReloading = false;
        }
    }
}