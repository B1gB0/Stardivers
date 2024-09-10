using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Projectiles.Bullets;
using UnityEngine;

public class FourBarrelMachineGun : Weapon
{
    private const string ObjectPoolBulletName = "PoolFourBarrelMachineGunBullets";
    private const bool IsAutoExpandPool = true;
    
    private const float MinValue = 0f;
    private const float DelayBetweenShots = 0.1f;
    private const int CountBullets = 4;

    private readonly List<Vector3> _directions = new ();
    private readonly MachineGunCharacteristics _machineGunCharacteristics = new();
    
    [SerializeField] private FourBarrelMachineGunBullet _bulletPrefab;
    [SerializeField] private int _countBulletsForPool;
    [SerializeField] private Transform _shootPoint;

    private float _lastBurstTime;
    private int _maxCountShots;
    private bool _isShooting = true;

    private Coroutine _coroutine;
    private FourBarrelMachineGunBullet _bullet;
    
    private AudioSoundsService _audioSoundsService;
    private ObjectPool<FourBarrelMachineGunBullet> _poolBullets;

    public void Construct(AudioSoundsService audioSoundsService)
    {
        _audioSoundsService = audioSoundsService;
    }

    private void Awake()
    {
        _poolBullets = new ObjectPool<FourBarrelMachineGunBullet>(_bulletPrefab, _countBulletsForPool, new GameObject(ObjectPoolBulletName).transform);
        _poolBullets.AutoExpand = IsAutoExpandPool;
        
        _directions.Add(transform.forward);
        _directions.Add(transform.right);
        _directions.Add(-transform.forward);
        _directions.Add(-transform.right);
    }

    private void Start()
    {
        _maxCountShots = _machineGunCharacteristics.MaxCountShots;
    }

    private void FixedUpdate()
    {
        if (_isShooting)
            Shoot();

        CheckAmmoAndReload();
    }
    
    public override void Shoot()
    {
        if (_lastBurstTime <= MinValue)
        {
            _audioSoundsService.PlaySound(this.Type);

            foreach (Vector3 direction in _directions)
            {
                StartCoroutine(LaunchBullet(direction));
            }
            
            _lastBurstTime = _machineGunCharacteristics.FireRate;
        }

        _lastBurstTime -= Time.fixedDeltaTime;
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
        yield return new WaitForSeconds(_machineGunCharacteristics.ReloadTime);

        _maxCountShots = _machineGunCharacteristics.MaxCountShots;
        _isShooting = true;
    }

    private IEnumerator LaunchBullet(Vector3 direction)
    {
        for (int i = 0; i < CountBullets; i++)
        {
            _bullet = _poolBullets.GetFreeElement();

            _maxCountShots--;
        
            _bullet.transform.position = _shootPoint.position + Vector3.one * Random.Range(-0.2f, 0.2f);
        
            _bullet.SetDirection(direction);
            _bullet.SetCharacteristics(_machineGunCharacteristics.Damage, _machineGunCharacteristics.BulletSpeed);

            yield return new WaitForSeconds(DelayBetweenShots);
        }
    }
}
