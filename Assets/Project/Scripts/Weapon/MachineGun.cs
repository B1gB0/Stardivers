using System;
using System.Collections;
using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using Project.Game.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class MachineGun : Weapon
{
    private const string ObjectPoolBulletName = "PoolBullets";
    private const bool IsAutoExpandPool = true;
    
    private const float MinValue = 0f;
    private const float DelayBetweenShots = 0.2f;
    
    private readonly MachineGunCharacteristics _machineGunCharacteristics = new();
    
    [SerializeField] private MachineGunBullet _bulletPrefab;

    [SerializeField] private int _countBulletsForPool;
    [SerializeField] private Transform[] _shootPoint;

    private float _lastBurstTime;
    private int _maxCountShots;
    private bool _isShooting = true;

    private Coroutine _coroutine;
    private MachineGunBullet _bullet;

    private ClosestEnemyDetector _detector;
    private EnemyActor closestEnemy;
    private ObjectPool<MachineGunBullet> _poolBullets;

    public void Construct(ClosestEnemyDetector detector)
    {
        _detector = detector;
    }

    private void Awake()
    {
        _poolBullets = new ObjectPool<MachineGunBullet>(_bulletPrefab, _countBulletsForPool, new GameObject(ObjectPoolBulletName).transform);
        _poolBullets.AutoExpand = IsAutoExpandPool;
    }

    private void Start()
    {
        _maxCountShots = _machineGunCharacteristics.MaxCountShots;
    }

    private void FixedUpdate()
    {
        closestEnemy = _detector.СlosestEnemy;

        if (closestEnemy == null) return;
        
        if (Vector3.Distance(closestEnemy.transform.position, transform.position) <= _machineGunCharacteristics.RangeAttack && _isShooting)
        {
            Shoot();
        }
        
        CheckAmmo();
        
        Debug.Log(_maxCountShots);
    }
    
    public override void Shoot()
    {
        if (_lastBurstTime <= MinValue && closestEnemy.Health.Value > MinValue)
        {
            StartCoroutine(LaunchBullet());
            
            _lastBurstTime = _machineGunCharacteristics.FireRate;
        }

        _lastBurstTime -= Time.fixedDeltaTime;
    }

    private void CheckAmmo()
    {
        if (_maxCountShots <= MinValue)
        {
            _isShooting = false;
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(_machineGunCharacteristics.ReloadTime);

        _maxCountShots = _machineGunCharacteristics.MaxCountShots;
        _isShooting = true;
    }

    private IEnumerator LaunchBullet()
    {
        foreach (var shootPoint in _shootPoint)
        {
            _bullet = _poolBullets.GetFreeElement();

            _maxCountShots--;
            
            _bullet.transform.position = shootPoint.position;
                
            _bullet.SetDirection(closestEnemy.transform);
            _bullet.SetCharacteristics(_machineGunCharacteristics.Damage, _machineGunCharacteristics.BulletSpeed);

            yield return new WaitForSeconds(DelayBetweenShots);
        }

        yield return null;
    }
}
