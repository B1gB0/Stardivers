using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Lightning;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using YG;

namespace Project.Scripts.Weapon.Player
{
    public class ChainLightningGun : PlayerWeapon
    {
        private const bool IsAutoExpandPool = true;
        private const string PoolName = "ChainLightningPool";
        private const float MinValue = 0f;
        private const int FirstElement = 0;
        private const int FirstTargetIndex = 1;

        [SerializeField] private Transform _shootPoint;
        [SerializeField] private LightningLineRendererProjectile lightningLineRendererPrefab;
        [SerializeField] private float _chainDelay = 0.2f;
        [SerializeField] private float _lightningDuration = 0.35f;
        [SerializeField] private float _heightOffset = 0.2f;

        private EnemyDetector _detector;
        private AudioSoundsService _audioService;
        private ObjectPool<LightningLineRendererProjectile> _lightningPool;

        private float _lastShotTime;
        private float _reloadTimer;

        private bool _isReloading;
        private bool _isShooting;
        
        private int _currentCharges;
        
        private Coroutine _chainCoroutine;
        private WeaponView _weaponView;
        
        public ChainLightningGunCharacteristics ChainLightningGunCharacteristics { get; private set; } = new();

        public void Construct(AudioSoundsService audioService, EnemyDetector detector, CharacteristicsWeaponData data,
            ChainLightningGunCharacteristics chainLightningGunCharacteristics, WeaponPanel weaponPanel)
        {
            _audioService = audioService;
            _detector = detector;
            Type = data.WeaponType;
            WeaponPanel = weaponPanel;
            
            if(chainLightningGunCharacteristics == null)
                ChainLightningGunCharacteristics.SetStartingCharacteristics(data);
            else
                ChainLightningGunCharacteristics = chainLightningGunCharacteristics;

            YG2.saves.ChainLightningGunCharacteristics = ChainLightningGunCharacteristics;
            _weaponView = WeaponPanel.GetWeaponViewByType(Type);
        }

        private void Awake()
        {
            _lightningPool = new ObjectPool<LightningLineRendererProjectile>(
                lightningLineRendererPrefab, ChainLightningGunCharacteristics.MaxCountShots,
                new GameObject(PoolName).transform)
            {
                AutoExpand = IsAutoExpandPool
            };
            
            _currentCharges = ChainLightningGunCharacteristics.MaxCountShots;
        }

        private void FixedUpdate()
        {
            _lastShotTime -= Time.deltaTime;
            
            if (_isReloading)
            {
                _weaponView.AnimateFiller(_reloadTimer, ChainLightningGunCharacteristics.ReloadTime);
            }

            if (_currentCharges <= MinValue)
            {
                StartCoroutine(Reload());
                return;
            }

            if (_detector.GetClosestEnemy() != null && _detector.ClosestEnemyDistance
                <= ChainLightningGunCharacteristics.RangeAttack && _lastShotTime <= MinValue)
            {
                Shoot();
            }
        }

        public override void Shoot()
        {
            if (_isShooting || _currentCharges <= MinValue) return;

            var firstTarget = _detector.GetClosestEnemy();
            if (firstTarget == null || firstTarget.Health.TargetHealth <= MinValue) return;

            _audioService.PlaySound(SoundsType.ChainLightningGun).Forget();
            _currentCharges--;
            _lastShotTime = ChainLightningGunCharacteristics.FireRate;

            _isShooting = true;

            CreateLightning(_shootPoint, firstTarget.transform);
            firstTarget.Health.TakeDamage(ChainLightningGunCharacteristics.Damage);

            if (_chainCoroutine != null) StopCoroutine(_chainCoroutine);
            _chainCoroutine = StartCoroutine(ChainReaction(firstTarget));
        }

        private IEnumerator ChainReaction(EnemyActor firstTarget)
        {
            var hitEnemies = new List<EnemyActor> { firstTarget };
            var currentTarget = firstTarget;

            for (int i = FirstTargetIndex; i < ChainLightningGunCharacteristics.MaxEnemiesInChain; i++)
            {
                yield return new WaitForSeconds(_chainDelay);

                var nextEnemy = FindNextEnemy(currentTarget, hitEnemies);
                if (nextEnemy == null || nextEnemy.Health.TargetHealth <= MinValue) break;

                CreateLightning(currentTarget.transform, nextEnemy.transform);
                nextEnemy.Health.TakeDamage(ChainLightningGunCharacteristics.Damage);

                hitEnemies.Add(nextEnemy);
                currentTarget = nextEnemy;
            }

            _isShooting = false;
        }

        private EnemyActor FindNextEnemy(EnemyActor lastEnemy, List<EnemyActor> alreadyHit)
        {
            if (lastEnemy == null) return null;

            var enemiesInRange = _detector.GetEnemiesInRange()
                .Where(enemy => enemy != null && enemy != lastEnemy && !alreadyHit.Contains(enemy) &&
                                Vector3.Distance(lastEnemy.transform.position, enemy.transform.position) <=
                                ChainLightningGunCharacteristics.RangeAttack)
                .OrderBy(enemy => Vector3.Distance(lastEnemy.transform.position, enemy.transform.position))
                .ToList();

            return enemiesInRange.Count > MinValue ? enemiesInRange[FirstElement] : null;
        }

        private void CreateLightning(Transform start, Transform end)
        {
            Vector3 startPoint = start.position;
            Vector3 endPoint = end.position;

            startPoint.y += _heightOffset;
            endPoint.y += _heightOffset;

            var lightning = _lightningPool.GetFreeElement();
            lightning.SetPosition(startPoint, endPoint);

            StartCoroutine(ReturnLightningToPool(lightning, _lightningDuration));
        }

        private IEnumerator ReturnLightningToPool(LightningLineRendererProjectile lightningLineRenderer, float delay)
        {
            yield return new WaitForSeconds(delay);
            lightningLineRenderer.gameObject.SetActive(false);
        }

        private IEnumerator Reload()
        {
            _reloadTimer = MinValue;
            _isReloading = true;
            
            _weaponView.ActivateFiller();

            while (_reloadTimer < ChainLightningGunCharacteristics.ReloadTime)
            {
                _reloadTimer += Time.fixedDeltaTime;
                yield return null;
            }

            _isReloading = false;
            _currentCharges = ChainLightningGunCharacteristics.MaxCountShots;
            _weaponView.DeactivateFiller();
        }

        public override void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
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