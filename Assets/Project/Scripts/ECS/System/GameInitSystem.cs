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
        private readonly IGoldService _goldService;
        private readonly ILevelTextService _levelTextService;
        private readonly AudioSoundsService _audioSoundsService;
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
        private readonly StoneInitData _stoneInitData;
        private readonly CapsuleInitData _capsuleInitData;
        private readonly HealingCoreInitData _healingCoreInitData;
        private readonly GoldCoreInitData _goldCoreInitData;
        private readonly LevelInitData _levelInitData;
        
        private Vector3 _playerSpawnPoint;
        private Vector3 _capsuleSpawnPoint;

        private ObjectPool<BigAlienEnemy> _bigAlienEnemyPool;
        private ObjectPool<SmallAlienEnemy> _smallAlienEnemyPool;
        private ObjectPool<GunnerAlienEnemy> _gunnerAlienEnemyPool;
        private ObjectPool<BigAlienEnemyProjectile> _bigAlienEnemyProjectilePool;
        private ObjectPool<GunnerAlienEnemyProjectile> _gunnerAlienEnemyProjectilePool;

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
        
        public SmallAlienEnemy CreateSmallAlienEnemy(PlayerActor target)
        {
            EnemyData data = _enemyService.GetEnemyDataByType(EnemyAlienActorType.SmallAlien);
            
            var smallEnemyAlienActor = _smallAlienEnemyPool.GetFreeElement();
            smallEnemyAlienActor.Construct(_experiencePoints, _textService);

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

            ref var attackComponent = ref entity.Get<EnemyMeleeAttackComponent>();
            attackComponent.Damage = data.Damage;

            return smallEnemyAlienActor;
        }

        public BigAlienEnemy CreateBigAlienEnemy(PlayerActor target)
        {
            EnemyData data = _enemyService.GetEnemyDataByType(EnemyAlienActorType.BigAlien);
            
            var bigEnemyAlienActor = _bigAlienEnemyPool.GetFreeElement();
            bigEnemyAlienActor.Construct(_experiencePoints, _textService);
            
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

            bigEnemyAlienActor.Weapon.SetData(target.transform, _bigAlienEnemyProjectilePool, data.Damage);

            return bigEnemyAlienActor;
        }

        public GunnerAlienEnemy CreateGunnerAlienEnemy(PlayerActor target)
        {
            EnemyData data = _enemyService.GetEnemyDataByType(EnemyAlienActorType.GunnerAlien);
            
            var gunnerEnemyAlienActor = _gunnerAlienEnemyPool.GetFreeElement();
            gunnerEnemyAlienActor.Construct(_experiencePoints, _textService);
            
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

            gunnerEnemyAlienActor.Weapon.SetData(target.transform, _gunnerAlienEnemyProjectilePool, data.Damage);

            return gunnerEnemyAlienActor;
        }

        public void CreateStone(Vector3 atPosition)
        {
            var stone = Object.Instantiate(_stoneInitData.StoneActorPrefab, atPosition, Quaternion.Euler(_stoneRotation));
            stone.GetExperiencePoints(_experiencePoints);

            InitResource(stone);
        }

        public void CreateHealingCore(Vector3 atPosition)
        {
            var healingCore = Object.Instantiate(_healingCoreInitData.HealingCorePrefab, atPosition, Quaternion.identity);
            healingCore.GetExperiencePoints(_experiencePoints);
            healingCore.GetServices(_textService);
            
            InitResource(healingCore);
        }
        
        public void CreateGoldCore(Vector3 atPosition)
        {
            var goldCore = Object.Instantiate(_goldCoreInitData.GoldCorePrefab, atPosition, Quaternion.identity);
            goldCore.GetExperiencePoints(_experiencePoints);
            goldCore.GetServices(_textService, _goldService);
            
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
            if(_level.SmallEnemyCountPoints > MinValue)
                _smallAlienEnemyPool = new ObjectPool<SmallAlienEnemy>(_smallAlienEnemyInitData.SmallAlienEnemyPrefab,
                    DefaultCountObjectsInPool, new GameObject(SmallEnemyAlienPool).transform)
                {
                    AutoExpand = IsAutoExpand
                };

            if (_level.BigEnemyCountPoints > MinValue)
            {
                _bigAlienEnemyPool = new ObjectPool<BigAlienEnemy>(_bigAlienEnemyData.BigAlienEnemyPrefab,
                    DefaultCountObjectsInPool, new GameObject(BigEnemyAlienPool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
                
                _bigAlienEnemyProjectilePool = new ObjectPool<BigAlienEnemyProjectile>(_bigAlienEnemyData.ProjectilePrefab, 
                    DefaultCountObjectsInPool, new GameObject(BigAlienEnemyProjectilePool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
            }

            if (_level.GunnerEnemyCountPoints > MinValue)
            {
                _gunnerAlienEnemyPool = new ObjectPool<GunnerAlienEnemy>(_gunnerAlienEnemyData.GunnerAlienEnemyPrefab, 
                    DefaultCountObjectsInPool, new GameObject(GunnerAlienEnemyPool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
                
                _gunnerAlienEnemyProjectilePool = new ObjectPool<GunnerAlienEnemyProjectile>(_gunnerAlienEnemyData.ProjectilePrefab, 
                    DefaultCountObjectsInPool, new GameObject(GunnerAlienEnemyProjectilePool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
            }
        }
    }
}