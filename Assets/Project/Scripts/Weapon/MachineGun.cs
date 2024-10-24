using System.Collections;
using Build.Game.Scripts.ECS.EntityActors;
using Project.Game.Scripts;
using Project.Game.Scripts.Improvements;
using UnityEngine;

public class MachineGun : Weapon
{
    private const string ObjectPoolBulletName = "PoolMachineGunBullets";
    private const bool IsAutoExpandPool = true;
    
    private const float MinValue = 0f;
    private const float DelayBetweenShots = 0.2f;

    [SerializeField] private MachineGunBullet _bulletPrefab;

    [SerializeField] private int _countBulletsForPool;
    [SerializeField] private Transform[] _shootPoints;

    private float _lastBurstTime;
    private int _maxCountShots;
    private bool _isShooting = true;

    private Coroutine _coroutine;
    private MachineGunBullet _bullet;

    private ClosestEnemyDetector _detector;
    private AudioSoundsService _audioSoundsService;
    
    private SmallAlienEnemyActor closestSmallAlienEnemy;
    private ObjectPool<MachineGunBullet> _poolBullets;

    public MachineGunCharacteristics MachineGunCharacteristics { get; } = new();

    public void Construct(ClosestEnemyDetector detector, AudioSoundsService audioSoundsService)
    {
        _detector = detector;
        _audioSoundsService = audioSoundsService;
    }

    private void Awake()
    {
        _poolBullets = new ObjectPool<MachineGunBullet>(_bulletPrefab, _countBulletsForPool, new GameObject(ObjectPoolBulletName).transform);
        _poolBullets.AutoExpand = IsAutoExpandPool;
    }

    private void Start()
    {
        _maxCountShots = MachineGunCharacteristics.MaxCountShots;
    }

    private void FixedUpdate()
    {
        closestSmallAlienEnemy = _detector.СlosestSmallAlienEnemy;

        if (closestSmallAlienEnemy == null) return;
        
        if (Vector3.Distance(closestSmallAlienEnemy.transform.position, transform.position) <= MachineGunCharacteristics.RangeAttack && _isShooting)
        {
            Shoot();
        }
        
        CheckAmmoAndReload();
    }
    
    public override void Shoot()
    {
        if (_lastBurstTime <= MinValue && closestSmallAlienEnemy.Health.TargetHealth > MinValue)
        {
            _audioSoundsService.PlaySound(Sounds.MachineGun);
            
            StartCoroutine(LaunchBullet());
            
            _lastBurstTime = MachineGunCharacteristics.FireRate;
        }

        _lastBurstTime -= Time.fixedDeltaTime;
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
        yield return new WaitForSeconds(MachineGunCharacteristics.ReloadTime);

        _maxCountShots = MachineGunCharacteristics.MaxCountShots;
        _isShooting = true;
    }

    private IEnumerator LaunchBullet()
    {
        foreach (var shootPoint in _shootPoints)
        {
            _bullet = _poolBullets.GetFreeElement();

            _maxCountShots--;
            
            _bullet.transform.position = shootPoint.position;
                
            _bullet.SetDirection(closestSmallAlienEnemy.transform);
            _bullet.SetCharacteristics(MachineGunCharacteristics.Damage, MachineGunCharacteristics.BulletSpeed);

            yield return new WaitForSeconds(DelayBetweenShots);
        }

        yield return null;
    }
}
