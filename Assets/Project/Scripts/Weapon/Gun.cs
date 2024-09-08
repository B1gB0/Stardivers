using Build.Game.Scripts.ECS.EntityActors;
using Project.Game.Scripts;
using UnityEngine;

public class Gun : Weapon
{
    private const string ObjectPoolBulletName = "PoolBullets";
    private const string ObjectPoolSoundsOfShotsName = "PoolAssaultRifleSoundsOfShots";
    private const bool IsAutoExpandPool = true;
    private const float MinValue = 0f;
    
    private readonly GunCharacteristics _gunCharacteristics = new();
    
    [SerializeField] private GunBullet _bulletPrefab;
    [SerializeField] private AssaultRifleSoundOfShot soundPrefab;
    
    [SerializeField] private int _countBullets;
    [SerializeField] private Transform _shootPoint;

    private float _lastShotTime;
    private GunBullet _bullet;

    private ClosestEnemyDetector _detector;
    private EnemyActor closestEnemy;

    private ObjectPool<GunBullet> _poolBullets;
    private ObjectPool<AssaultRifleSoundOfShot> _poolSoundsOfShots;


    public void Construct(ClosestEnemyDetector detector)
    {
        _detector = detector;
    }

    private void Awake()
    {
        _poolBullets = new ObjectPool<GunBullet>(_bulletPrefab, _countBullets, new GameObject(ObjectPoolBulletName).transform);
        _poolSoundsOfShots = new ObjectPool<AssaultRifleSoundOfShot>(soundPrefab, _countBullets,
            new GameObject(ObjectPoolSoundsOfShotsName).transform);

        _poolSoundsOfShots.AutoExpand = IsAutoExpandPool;
        _poolBullets.AutoExpand = IsAutoExpandPool;
    }

    private void FixedUpdate()
    {
        closestEnemy = _detector.СlosestEnemy;
        
        if (closestEnemy != null)
        {
            if (Vector3.Distance(closestEnemy.transform.position, transform.position) <= _gunCharacteristics.RangeAttack)
            {
                Shoot();
            }
        }
    }
    
    public override void Shoot()
    {
        if (_lastShotTime <= MinValue && closestEnemy.Health.Value > MinValue)
        {
            _bullet = _poolBullets.GetFreeElement();

            AssaultRifleSoundOfShot sound = _poolSoundsOfShots.GetFreeElement();
            
            sound.AudioSource.PlayOneShot(sound.AudioSource.clip);
            
            StartCoroutine(sound.OffSound());

            _bullet.transform.position = _shootPoint.position;

            _bullet.SetDirection(closestEnemy.transform);
            _bullet.SetCharacteristics(_gunCharacteristics.Damage, _gunCharacteristics.BulletSpeed);

            _lastShotTime = _gunCharacteristics.FireRate;
        }

        _lastShotTime -= Time.fixedDeltaTime;
    }
}
