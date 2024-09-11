using System.Collections;
using Build.Game.Scripts.ECS.EntityActors;
using Project.Game.Scripts;
using Project.Game.Scripts.Improvements;
using UnityEngine;

public class Gun : Weapon
{
    private const string ObjectPoolBulletName = "PoolGunBullets";
    private const bool IsAutoExpandPool = true;
    private const float MinValue = 0f;

    [SerializeField] private GunBullet _bulletPrefab;
    [SerializeField] private int _countBullets;
    [SerializeField] private Transform _shootPoint;

    private float _lastShotTime;
    private int _maxCountShots;
    private bool _isShooting = true;
    
    private GunBullet _bullet;
    private EnemyActor closestEnemy;
    private ObjectPool<GunBullet> _poolBullets;

    private ClosestEnemyDetector _detector;
    private AudioSoundsService _audioSoundsService;

    public GunCharacteristics GunCharacteristics { get; } = new();

    public void Construct(ClosestEnemyDetector detector, AudioSoundsService audioSoundsService)
    {
        _detector = detector;
        _audioSoundsService = audioSoundsService;
    }

    private void Awake()
    {
        _poolBullets = new ObjectPool<GunBullet>(_bulletPrefab, _countBullets, new GameObject(ObjectPoolBulletName).transform);
        _poolBullets.AutoExpand = IsAutoExpandPool;
    }

    private void FixedUpdate()
    {
        closestEnemy = _detector.СlosestEnemy;

        if (closestEnemy == null) return;
        
        if (Vector3.Distance(closestEnemy.transform.position, transform.position) <= GunCharacteristics.RangeAttack && _isShooting)
        {
            Shoot();
        }
        
        CheckAmmoAndReload();
    }
    
    public override void Shoot()
    {
        if (_lastShotTime <= MinValue && closestEnemy.Health.Value > MinValue)
        {
            _bullet = _poolBullets.GetFreeElement();
            
            _audioSoundsService.PlaySound(Sounds.Gun);

            _bullet.transform.position = _shootPoint.position;

            _bullet.SetDirection(closestEnemy.transform);
            _bullet.SetCharacteristics(GunCharacteristics.Damage, GunCharacteristics.BulletSpeed);

            _lastShotTime = GunCharacteristics.FireRate;
        }

        _lastShotTime -= Time.fixedDeltaTime;
    }

    public override void Accept(IWeaponVisitor weaponVisitor, CharacteristicsTypes type, float value)
    {
        weaponVisitor.Visit(this, type, value);
    }
    
    private void CheckAmmoAndReload()
    {
        if (_maxCountShots <= MinValue)
        {
            _isShooting = false;
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(GunCharacteristics.ReloadTime);

        _maxCountShots = GunCharacteristics.MaxCountShots;
        _isShooting = true;
    }
}
