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
        private const float MinValue = 0f;
        
        private readonly List<EnemyAlienActor> _enemiesInChain = new ();
        private readonly List<GameObject> _spawnedLineRenderers = new ();
        
        [SerializeField] private Transform _shootPoint;
        [SerializeField] [Range(1, 10)] private int _maximumEnemiesInChain = 3;
        [SerializeField] private GameObject _lineRendererPrefab;
        [SerializeField] private float _refreshRate = 0.01f;
        [SerializeField] private float _delayBetweenChain = 0.2f;
        
        private NewEnemyDetector _detector;
        private AudioSoundsService _audioSoundsService;
        
        private float _lastShotTime;
        private int _counter = 1;
        private int _maxCountShots;
        private bool _isReloading;
        
        private EnemyAlienActor _currentClosestEnemy;

        public GunCharacteristics GunCharacteristics { get; } = new ();
        
        public void Construct(AudioSoundsService audioSoundsService, NewEnemyDetector enemyDetector)
        {
            _audioSoundsService = audioSoundsService;
            _detector = enemyDetector;
        }

        private void FixedUpdate()
        {
            _currentClosestEnemy = _detector.GetClosestEnemy();

            if (_detector.GetEnemiesInRange().Count > 0)
            {
                if(!_isReloading)
                    Shoot();
            }
            
            CheckAmmoAndReload();
        }

        public override void Shoot()
        {
            if (_lastShotTime <= MinValue && _currentClosestEnemy.Health.TargetHealth > MinValue)
            {
                _audioSoundsService.PlaySound(Sounds.Gun);
                
                CreateLightning(_shootPoint, _currentClosestEnemy.transform, true);

                if (_maximumEnemiesInChain > 1)
                {
                    StartCoroutine(ChainReaction(_currentClosestEnemy));
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
            GameObject lineRenderer = Instantiate(_lineRendererPrefab);
            _spawnedLineRenderers.Add(lineRenderer);
            StartCoroutine(UpdateLineRenderer(lineRenderer, startPoint, endPoint, fromPlayer));
        }

        private IEnumerator UpdateLineRenderer(GameObject lineRenderer, Transform startPoint, Transform endPoint,
            bool fromPlayer = false)
        {
            lineRenderer.GetComponent<LineRendererController>().SetPosition(startPoint, endPoint);

            yield return new WaitForSeconds(_refreshRate);

            if (fromPlayer)
            {
                StartCoroutine(UpdateLineRenderer(lineRenderer, startPoint, 
                    _detector.GetClosestEnemy().transform, true));

                if (_currentClosestEnemy != _detector.GetClosestEnemy())
                {
                    StopShooting();
                    Shoot();
                }
            }
            else
            {
                StartCoroutine(UpdateLineRenderer(lineRenderer, startPoint, endPoint));
            }
        }

        private IEnumerator ChainReaction(EnemyAlienActor closestEnemy)
        {
            yield return new WaitForSeconds(_delayBetweenChain);

            if (_counter == _maximumEnemiesInChain)
            {
                yield return null;
            }
            else
            {
                _counter++;
                
                _enemiesInChain.Add(closestEnemy);

                if (!_enemiesInChain.Contains(closestEnemy.GetComponent<NewEnemyDetector>().GetClosestEnemy()))
                {
                    CreateLightning(closestEnemy.transform, closestEnemy.GetComponent<NewEnemyDetector>().GetClosestEnemy().transform);
                    StartCoroutine(ChainReaction(closestEnemy.GetComponent<NewEnemyDetector>().GetClosestEnemy()));
                }
            }
        }

        private void StopShooting()
        {
            _counter = 1;
            
            for (int i = 0; i < _spawnedLineRenderers.Count; i++)
            {
                Destroy(_spawnedLineRenderers[i]);
            }
            
            _spawnedLineRenderers.Clear();
            _enemiesInChain.Clear();
        }
        
        private void CheckAmmoAndReload()
        {
            if (_maxCountShots <= MinValue)
            {
                _isReloading = true;
                StartCoroutine(Reload());
            }
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(GunCharacteristics.ReloadTime);

            _maxCountShots = GunCharacteristics.MaxCountShots;
            _isReloading = false;
        }
    }
}