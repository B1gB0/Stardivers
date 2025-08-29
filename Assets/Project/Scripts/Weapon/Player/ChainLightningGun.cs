using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Project.Game.Scripts;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Lightning;
using Project.Scripts.Services;
using Project.Scripts.Weapon.Characteristics;
using Project.Scripts.Weapon.Improvements;

namespace Project.Scripts.Weapon.Player
{
    public class ChainLightningGun : PlayerWeapon
    {
        private const bool IsAutoExpandPool = true;

        [SerializeField] private Transform _shootPoint;
        [SerializeField] [Range(1, 10)] private int _maxEnemiesInChain = 3;
        [SerializeField] private LightningLineRendererProjectile lightningLineRendererPrefab;
        [SerializeField] private float _chainDelay = 0.2f;
        [SerializeField] private float _lightningDuration = 0.35f;
        [SerializeField] private float _heightOffset = 0.2f;

        private ImprovedEnemyDetector _detector;
        private AudioSoundsService _audioService;
        private ObjectPool<LightningLineRendererProjectile> _lightningPool;

        private float _lastShotTime;
        private bool _isShooting;
        private int _currentCharges;
        private Coroutine _chainCoroutine;

        private const string PoolName = "ChainLightningPool";

        public GunCharacteristics GunCharacteristics { get; } = new();

        public void Construct(AudioSoundsService audioService, ImprovedEnemyDetector detector)
        {
            _audioService = audioService;
            _detector = detector;
        }

        private void Awake()
        {
            _lightningPool = new ObjectPool<LightningLineRendererProjectile>(
                lightningLineRendererPrefab, GunCharacteristics.MaxCountBullets, new GameObject(PoolName).transform)
            {
                AutoExpand = IsAutoExpandPool
            };

            GunCharacteristics.SetStartingCharacteristics();
            _currentCharges = GunCharacteristics.MaxCountBullets;
        }

        private void FixedUpdate()
        {
            _lastShotTime -= Time.deltaTime;

            if (_currentCharges <= 0)
            {
                StartCoroutine(Reload());
                return;
            }

            if (_detector.GetClosestEnemy() != null && _detector.ClosestEnemyDistance
                <= GunCharacteristics.RangeAttack && _lastShotTime <= 0)
            {
                Shoot();
            }
        }

        public override void Shoot()
        {
            if (_isShooting || _currentCharges <= 0) return;

            var firstTarget = _detector.GetClosestEnemy();
            if (firstTarget == null || firstTarget.Health.TargetHealth <= 0) return;

            _audioService.PlaySound(Sounds.Gun);
            _currentCharges--;
            _lastShotTime = GunCharacteristics.FireRate;

            _isShooting = true;

            CreateLightning(_shootPoint, firstTarget.transform);
            firstTarget.Health.TakeDamage(GunCharacteristics.Damage);

            if (_chainCoroutine != null) StopCoroutine(_chainCoroutine);
            _chainCoroutine = StartCoroutine(ChainReaction(firstTarget));
        }

        private IEnumerator ChainReaction(EnemyAlienActor firstTarget)
        {
            var hitEnemies = new List<EnemyAlienActor> { firstTarget };
            var currentTarget = firstTarget;

            for (int i = 1; i < _maxEnemiesInChain; i++)
            {
                yield return new WaitForSeconds(_chainDelay);

                var nextEnemy = FindNextEnemy(currentTarget, hitEnemies);
                if (nextEnemy == null || nextEnemy.Health.TargetHealth <= 0) break;

                CreateLightning(currentTarget.transform, nextEnemy.transform);
                nextEnemy.Health.TakeDamage(GunCharacteristics.Damage);

                hitEnemies.Add(nextEnemy);
                currentTarget = nextEnemy;
            }

            _isShooting = false;
        }

        private EnemyAlienActor FindNextEnemy(EnemyAlienActor lastEnemy, List<EnemyAlienActor> alreadyHit)
        {
            if (lastEnemy == null) return null;

            var enemiesInRange = _detector.GetEnemiesInRange()
                .Where(enemy => enemy != null && enemy != lastEnemy && !alreadyHit.Contains(enemy) &&
                                Vector3.Distance(lastEnemy.transform.position, enemy.transform.position) <=
                                GunCharacteristics.RangeAttack)
                .OrderBy(enemy => Vector3.Distance(lastEnemy.transform.position, enemy.transform.position))
                .ToList();

            return enemiesInRange.Count > 0 ? enemiesInRange[0] : null;
        }

        private void CreateLightning(Transform start, Transform end)
        {
            Vector3 startPoint = start.position;
            Vector3 endPoint = end.position;

            startPoint.y += _heightOffset;
            endPoint.y += _heightOffset;

            var lightning = _lightningPool.GetFreeElement();
            lightning.SetPosition(startPoint, endPoint);
            lightning.SetCharacteristics(GunCharacteristics.Damage, GunCharacteristics.ProjectileSpeed);

            StartCoroutine(ReturnLightningToPool(lightning, _lightningDuration));
        }

        private IEnumerator ReturnLightningToPool(LightningLineRendererProjectile lightningLineRenderer, float delay)
        {
            yield return new WaitForSeconds(delay);
            lightningLineRenderer.gameObject.SetActive(false);
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(GunCharacteristics.ReloadTime);
            _currentCharges = GunCharacteristics.MaxCountBullets;
        }

        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            switch (type)
            {
                // case CharacteristicType.Damage:
                //     // Implement damage improvement
                //     break;
                // case CharacteristicType.Range:
                //     _attackRange += value;
                //     break;
                // case CharacteristicType.ChainCount:
                //     _maxEnemiesInChain = Mathf.Clamp(_maxEnemiesInChain + (int)value, 1, 10);
                //     break;
                // case CharacteristicType.ChainRange:
                //     _chainRange += value;
                //     break;
            }
        }

        private void OnDisable()
        {
            if (_chainCoroutine != null)
            {
                StopCoroutine(_chainCoroutine);
                _chainCoroutine = null;
            }

            _isShooting = false;
        }
    }
}