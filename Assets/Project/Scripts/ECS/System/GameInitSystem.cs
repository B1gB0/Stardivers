using System;
using Leopotam.Ecs;
using Project.Game.Scripts;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.Components;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.EnemyAnimation;
using Project.Scripts.Experience;
using Project.Scripts.Levels;
using Project.Scripts.Levels.Mars.SecondLevel;
using Project.Scripts.Levels.MysteryPlanet.SecondLevel;
using Project.Scripts.Projectiles.Enemy;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Scripts.ECS.System
{
    public class GameInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const string SmallEnemyAlienPool = nameof(SmallEnemyAlienPool);
        private const string BigEnemyAlienPool = nameof(BigEnemyAlienPool);
        private const string BigAlienEnemyProjectilePool = nameof(BigAlienEnemyProjectilePool);
        private const string GunnerAlienEnemyPool = nameof(GunnerAlienEnemyPool);
        private const string GunnerAlienEnemyProjectilePool = nameof(GunnerAlienEnemyProjectilePool);
        private const string AlienEnemyTurretProjectilePool = nameof(AlienEnemyTurretProjectilePool);

        private const bool IsAutoExpand = true;
        
        private const float CapsuleHeight = 20f;
        private const int MinValue = 0;
        private const int DefaultCountObjectsInPool = 3;

        private readonly Vector3 _stoneRotation = new (0f, 90f, 0f);
        private readonly EcsWorld _world;
        
        private readonly IFloatingTextService _textService;
        private readonly IDataBaseService _dataBaseService;
        private readonly IResourceService _resourceService;
        private readonly IEnemyService _enemyService;
        private readonly IPlayerService _playerService;
        private readonly ICurrencyService _currencyService;
        private readonly ILevelTextService _levelTextService;
        private readonly AudioSoundsService _audioSoundsService;
        private readonly ICoreService _coreService;
        
        private readonly ExperiencePoints _experiencePoints;
        private readonly Timer _timer;
        private readonly PauseService _pauseService;
        private readonly DialoguePanel _dialoguePanel;
        private readonly MissionProgressBar _missionProgressBar;
        private readonly Level _level;

        private readonly PlayerInitData _playerInitData;
        private readonly SmallAlienEnemyInitData _smallAlienEnemyInitData;
        private readonly BigAlienEnemyInitData _bigAlienEnemyData;
        private readonly GunnerAlienEnemyInitData _gunnerAlienEnemyData;
        private readonly AlienTurretEnemyInitData _alienTurretEnemyData;
        private readonly AlienCocoonInitData _alienCocoonData;
        private readonly StoneInitData _stoneInitData;
        private readonly CapsuleInitData _capsuleInitData;
        private readonly HealingCoreInitData _healingCoreInitData;
        private readonly GoldCoreInitData _goldCoreInitData;
        private readonly LevelInitData _levelInitData;
        
        private Vector3 _playerSpawnPoint;
        private Vector3 _capsuleSpawnPoint;

        private ObjectPool<BigEnemy> _bigAlienEnemyPool;
        private ObjectPool<SmallEnemy> _smallAlienEnemyPool;
        private ObjectPool<GunnerEnemy> _gunnerAlienEnemyPool;
        private ObjectPool<BigAlienEnemyProjectile> _bigAlienEnemyProjectilePool;
        private ObjectPool<GunnerAlienEnemyProjectile> _gunnerAlienEnemyProjectilePool;
        private ObjectPool<AlienEnemyTurretProjectile> _alienEnemyTurretProjectilePool;

        public CapsuleActor Capsule { get; private set; }
        public PlayerActor Player { get; private set; }
        public Health.Health PlayerHealth { get; private set; }
        public Transform PlayerTransform { get; private set; }

        public event Action PlayerIsSpawned;

        public void Init()
        {
            _playerSpawnPoint = _levelInitData.PlayerSpawnPosition;
            Player = CreatePlayer();
            PlayerHealth = Player.Health;
            Player.gameObject.SetActive(false);

            _level.GetServices(this, _timer, _dialoguePanel, _pauseService, _levelInitData,
                _levelTextService);
            
            switch (_level)
            {
                case SecondMarsLevel secondMarsLevel:
                    secondMarsLevel.GetBallisticProgressBar(_missionProgressBar);
                    break;
                case SecondMysteryPlanetLevel secondMysteryPlanetLevel:
                    secondMysteryPlanetLevel.GetRadioTowerProgressBar(_missionProgressBar);
                    break;
            }

            CreateEnemyObjectPools();
        }

        public void Run()
        {
            if (Capsule != null)
            {
                LaunchPlayerCapsule();
            }
        }

        public void CreateCapsule()
        {
            _audioSoundsService.PlaySound(Sounds.CapsuleFlight);
            
            _capsuleSpawnPoint = Player.transform.position;
            _capsuleSpawnPoint.y += CapsuleHeight;
            
            Capsule = Object.Instantiate(_capsuleInitData.Prefab, _capsuleSpawnPoint, Quaternion.identity);
        }

        public void SpawnPlayer()
        {
            PlayerData data = _dataBaseService.Content.Players[0];
            
            Player.gameObject.SetActive(true);
            
            if (Player.Health.TargetHealth <= MinValue)
            {
                Player.Health.SetHealthValue(data.Health);
            }
            
            PlayerIsSpawned?.Invoke();
        }

        private void LaunchPlayerCapsule()
        {
            Capsule.transform.position = Vector3.MoveTowards(Capsule.transform.position, Player.transform.position,
                _capsuleInitData.DefaultMoveSpeed * Time.deltaTime);

            if (Capsule.transform.position == Player.transform.position)
            {
                SpawnPlayer();
                Capsule.Destroy();
            }
        }

        private PlayerActor CreatePlayer()
        {
            var data = _playerService.GetPlayerDataByType(PlayerActorType.CommonStardiver);
            PlayerActor playerActor = Object.Instantiate(_playerInitData.Prefab, _playerSpawnPoint, Quaternion.identity);

            MiningToolActor miningToolActor = playerActor.GetComponentInChildren<MiningToolActor>();
            miningToolActor.Construct(_audioSoundsService, data.DiggingSpeed);

            PlayerTransform = playerActor.transform;
            
            InitPlayer(playerActor, data);

            return playerActor;
        }
        
        public SmallEnemy CreateSmallAlienEnemy(PlayerActor target)
        {
            var data = _enemyService.GetEnemyDataByType(EnemyActorType.SmallAlien);
            
            var smallEnemyAlienActor = _smallAlienEnemyPool.GetFreeElement();
            smallEnemyAlienActor.Construct(_experiencePoints, _textService, data);

            if (smallEnemyAlienActor.Health.TargetHealth <= MinValue)
            {
                smallEnemyAlienActor.Health.SetHealthValue(data.Health);
            }
            
            var entity = _world.NewEntity();
            
            ref var enemyComponent = ref entity.Get<EnemyComponent>();
            enemyComponent.Health = smallEnemyAlienActor.Health;

            ref var enemyMovableComponent = ref entity.Get<EnemyMovableComponent>();
            enemyMovableComponent.NavMeshAgent = smallEnemyAlienActor.NavMeshAgent;
            enemyMovableComponent.Transform = smallEnemyAlienActor.transform;
            enemyMovableComponent.MoveSpeed = data.Speed;
            enemyMovableComponent.IsMoving = true;

            ref var enemyAnimationsComponent = ref entity.Get<AnimatedComponent>();
            enemyAnimationsComponent.Animator = smallEnemyAlienActor.Animator;

            ref var followComponent = ref entity.Get<FollowPlayerComponent>();
            followComponent.Target = target;

            ref var patrolComponent = ref entity.Get<PatrolComponent>();
            patrolComponent.Points = _levelInitData.EnemyPatrolPositions;

            ref var attackComponent = ref entity.Get<EnemySmallAlienAttackComponent>();
            attackComponent.Damage = data.Damage;
            attackComponent.FireRate = data.FireRate;
            attackComponent.RangeAttack = data.RangeAttack;
            
            enemyMovableComponent.NavMeshAgent.stoppingDistance = attackComponent.RangeAttack;

            return smallEnemyAlienActor;
        }

        public BigEnemy CreateBigAlienEnemy(PlayerActor target)
        {
            var data = _enemyService.GetEnemyDataByType(EnemyActorType.BigAlien);
            
            var bigEnemyAlienActor = _bigAlienEnemyPool.GetFreeElement();
            bigEnemyAlienActor.Construct(_experiencePoints, _textService, data);
            
            if (bigEnemyAlienActor.Health.TargetHealth <= MinValue)
            {
                bigEnemyAlienActor.Health.SetHealthValue(data.Health);
            }
            
            var entity = _world.NewEntity();
            
            ref var enemyComponent = ref entity.Get<EnemyComponent>();
            enemyComponent.Health = bigEnemyAlienActor.Health;

            ref var enemyMovableComponent = ref entity.Get<EnemyMovableComponent>();
            enemyMovableComponent.NavMeshAgent = bigEnemyAlienActor.NavMeshAgent;
            enemyMovableComponent.Transform = bigEnemyAlienActor.transform;
            enemyMovableComponent.MoveSpeed = data.Speed;
            enemyMovableComponent.IsMoving = true;

            ref var enemyAnimationsComponent = ref entity.Get<AnimatedComponent>();
            AnimatedStateMachine animatedStateMachine = new(bigEnemyAlienActor.Animator);
            enemyAnimationsComponent.AnimatedStateMachine = animatedStateMachine;
            enemyAnimationsComponent.Animator = bigEnemyAlienActor.Animator;

            ref var followComponent = ref entity.Get<FollowPlayerComponent>();
            followComponent.Target = target;
            
            ref var patrolComponent = ref entity.Get<PatrolComponent>();
            patrolComponent.Points = _levelInitData.EnemyPatrolPositions;

            ref var attackComponent = ref entity.Get<EnemyBigAlienAttackComponent>();
            attackComponent.FireRate = data.FireRate;
            attackComponent.RangeAttack = data.RangeAttack;
            
            enemyMovableComponent.NavMeshAgent.stoppingDistance = attackComponent.RangeAttack;

            bigEnemyAlienActor.Weapon.SetData(target.transform, _bigAlienEnemyProjectilePool, data.Damage);

            return bigEnemyAlienActor;
        }

        public GunnerEnemy CreateGunnerAlienEnemy(PlayerActor target)
        {
            var data = _enemyService.GetEnemyDataByType(EnemyActorType.GunnerAlien);
            
            var gunnerEnemyAlienActor = _gunnerAlienEnemyPool.GetFreeElement();
            gunnerEnemyAlienActor.Construct(_experiencePoints, _textService, data);
            
            if (gunnerEnemyAlienActor.Health.TargetHealth <= MinValue)
            {
                gunnerEnemyAlienActor.Health.SetHealthValue(data.Health);
            }
            
            var entity = _world.NewEntity();
            
            ref var enemyComponent = ref entity.Get<EnemyComponent>();
            enemyComponent.Health = gunnerEnemyAlienActor.Health;

            ref var enemyMovableComponent = ref entity.Get<EnemyMovableComponent>();
            enemyMovableComponent.NavMeshAgent = gunnerEnemyAlienActor.NavMeshAgent;
            enemyMovableComponent.Transform = gunnerEnemyAlienActor.transform;
            enemyMovableComponent.MoveSpeed = data.Speed;
            enemyMovableComponent.IsMoving = true;

            ref var enemyAnimationsComponent = ref entity.Get<AnimatedComponent>();
            AnimatedStateMachine animatedStateMachine = new(gunnerEnemyAlienActor.Animator);
            enemyAnimationsComponent.AnimatedStateMachine = animatedStateMachine;
            enemyAnimationsComponent.Animator = gunnerEnemyAlienActor.Animator;

            ref var followComponent = ref entity.Get<FollowPlayerComponent>();
            followComponent.Target = target;
            
            ref var patrolComponent = ref entity.Get<PatrolComponent>();
            patrolComponent.Points = _levelInitData.EnemyPatrolPositions;

            ref var attackComponent = ref entity.Get<EnemyGunnerAlienAttackComponent>();
            attackComponent.FireRate = data.FireRate;
            attackComponent.RangeAttack = data.RangeAttack;

            enemyMovableComponent.NavMeshAgent.stoppingDistance = attackComponent.RangeAttack;

            gunnerEnemyAlienActor.Weapon.SetData(target.transform, _gunnerAlienEnemyProjectilePool, data.Damage);

            return gunnerEnemyAlienActor;
        }

        public EnemyTurret CreateEnemyTurret(PlayerActor target, Vector3 atPosition)
        {
            var data = _enemyService.GetEnemyDataByType(EnemyActorType.TurretAlien);
            
            var enemyTurret = Object.Instantiate(_alienTurretEnemyData.AlienTurretEnemyPrefab, atPosition,
                Quaternion.identity);
            enemyTurret.Construct(_experiencePoints, _textService, data);
            
            if (enemyTurret.Health.TargetHealth <= MinValue)
            {
                enemyTurret.Health.SetHealthValue(data.Health);
            }
            
            var entity = _world.NewEntity();
            
            ref var enemyComponent = ref entity.Get<EnemyComponent>();
            enemyComponent.Health = enemyTurret.Health;
            
            ref var enemyMovableComponent = ref entity.Get<EnemyMovableComponent>();
            enemyMovableComponent.Transform = enemyTurret.transform;
            enemyMovableComponent.MoveSpeed = data.Speed;

            ref var enemyAnimationsComponent = ref entity.Get<AnimatedComponent>();
            AnimatedStateMachine animatedStateMachine = new(enemyTurret.Animator);
            enemyAnimationsComponent.AnimatedStateMachine = animatedStateMachine;
            enemyAnimationsComponent.Animator = enemyTurret.Animator;

            ref var followComponent = ref entity.Get<FollowPlayerComponent>();
            followComponent.Target = target;

            ref var attackComponent = ref entity.Get<EnemyAlienTurretAttackComponent>();
            attackComponent.FireRate = data.FireRate;
            attackComponent.RangeAttack = data.RangeAttack;

            enemyTurret.Weapon.SetData(target.transform, _alienEnemyTurretProjectilePool, data.Damage);

            return enemyTurret;
        }

        public void CreateStone(Vector3 atPosition)
        {
            var data = _coreService.GetCoreDataByType(CoreType.Stone);
            
            var stone = Object.Instantiate(_stoneInitData.StoneActorPrefab, atPosition,
                Quaternion.Euler(_stoneRotation));
            stone.Construct(_experiencePoints, data);
            stone.Health.SetHealthValue(data.Health);

            InitResource(stone);
        }

        public void CreateAlienCocoon(Vector3 atPosition)
        {
            var data = _coreService.GetCoreDataByType(CoreType.AlienCocoon);
            
            var alienCocoon = Object.Instantiate(_alienCocoonData.AlienCocoonPrefab, atPosition, 
                Quaternion.identity);
            alienCocoon.Construct(_experiencePoints, data);
            alienCocoon.GetServices(_currencyService, _textService);
            alienCocoon.Health.SetHealthValue(data.Health);

            InitResource(alienCocoon);
        }

        public void CreateHealingCore(Vector3 atPosition)
        {
            var data = _coreService.GetCoreDataByType(CoreType.Healing);
            
            var healingCore = Object.Instantiate(_healingCoreInitData.HealingCorePrefab, atPosition, Quaternion.identity);
            healingCore.Construct(_experiencePoints, data);
            healingCore.GetServices(_textService);
            healingCore.Health.SetHealthValue(data.Health);
            
            InitResource(healingCore);
        }
        
        public void CreateGoldCore(Vector3 atPosition)
        {
            var data = _coreService.GetCoreDataByType(CoreType.Gold);
            
            var goldCore = Object.Instantiate(_goldCoreInitData.GoldCorePrefab, atPosition, Quaternion.identity);
            goldCore.Construct(_experiencePoints, data);
            goldCore.GetServices(_textService, _currencyService);
            goldCore.Health.SetHealthValue(data.Health);
            
            InitResource(goldCore);
        }

        private void InitPlayer(PlayerActor playerActor, PlayerData data)
        {
            var player = _world.NewEntity();

            ref var inputEventComponent = ref player.Get<InputEventComponent>();
            inputEventComponent.PlayerInputController = playerActor.PlayerInputController;
            
            ref var playerComponent = ref player.Get<PlayerComponent>();
            playerComponent.MiningTool = playerActor.MiningToolActor;

            ref var movableComponent = ref player.Get<PlayerMovableComponent>();
            movableComponent.MoveSpeed = data.MoveSpeed;
            movableComponent.RotationSpeed = data.RotationSpeed;
            movableComponent.Transform = playerActor.transform;
            movableComponent.Rigidbody = playerActor.Rigidbody;

            ref var animationsComponent = ref player.Get<AnimatedComponent>();
            animationsComponent.Animator = playerActor.Animator;
            
            playerActor.Construct(_playerService);
            _playerService.GetPlayer(playerActor, movableComponent);
        }

        private void InitResource(ResourceActor resource)
        {
            var entity = _world.NewEntity();

            ref var resourceComponent = ref entity.Get<ResourceComponent>();
            resourceComponent.Health = resource.Health;

            ref var animatedComponent = ref entity.Get<AnimatedComponent>();
            animatedComponent.Animator = resource.Animator;
        }
        
        private void CreateEnemyObjectPools()
        {
            if(_levelInitData.FirstWaveSmallEnemyAlienSpawnPositions.Count > MinValue)
                _smallAlienEnemyPool = new ObjectPool<SmallEnemy>(_smallAlienEnemyInitData.SmallEnemyPrefab,
                    DefaultCountObjectsInPool, new GameObject(SmallEnemyAlienPool).transform)
                {
                    AutoExpand = IsAutoExpand
                };

            if (_levelInitData.FirstWaveBigEnemyAlienSpawnPositions.Count > MinValue)
            {
                _bigAlienEnemyPool = new ObjectPool<BigEnemy>(_bigAlienEnemyData.BigEnemyPrefab,
                    DefaultCountObjectsInPool, new GameObject(BigEnemyAlienPool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
                
                _bigAlienEnemyProjectilePool = 
                    new ObjectPool<BigAlienEnemyProjectile>(_bigAlienEnemyData.ProjectilePrefab, 
                    DefaultCountObjectsInPool, new GameObject(BigAlienEnemyProjectilePool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
            }

            if (_levelInitData.FirstWaveGunnerEnemyAlienSpawnPositions.Count > MinValue)
            {
                _gunnerAlienEnemyPool = new ObjectPool<GunnerEnemy>(_gunnerAlienEnemyData.GunnerEnemyPrefab, 
                    DefaultCountObjectsInPool, new GameObject(GunnerAlienEnemyPool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
                
                _gunnerAlienEnemyProjectilePool = 
                    new ObjectPool<GunnerAlienEnemyProjectile>(_gunnerAlienEnemyData.ProjectilePrefab, 
                    DefaultCountObjectsInPool, new GameObject(GunnerAlienEnemyProjectilePool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
            }

            if (_levelInitData.EnemyTurretsSpawnPoints.Count > MinValue)
            {
                _alienEnemyTurretProjectilePool = 
                    new ObjectPool<AlienEnemyTurretProjectile>(_alienTurretEnemyData.ProjectilePrefab, 
                    DefaultCountObjectsInPool, new GameObject(AlienEnemyTurretProjectilePool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
            }
        }
    }
}