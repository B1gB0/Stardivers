using System;
using System.Collections.Generic;
using Build.Game.Scripts.ECS.Data;
using Leopotam.Ecs;
using Project.Game.Scripts;
using Project.Scripts.ECS.Components;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.EnemyAnimation;
using Project.Scripts.Experience;
using Project.Scripts.Levels;
using Project.Scripts.Levels.Mars.SecondLevel;
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
        private const int CountAlienEnemyProjectile = 3;

        private readonly Vector3 _stoneRotation = new (0f, 90f, 0f);
        private readonly EcsWorld _world;
        
        private readonly FloatingTextService _textService;
        private readonly GoldView _goldView;
        private readonly AudioSoundsService _audioSoundsService;
        private readonly ExperiencePoints _experiencePoints;
        private readonly Timer _timer;
        private readonly AdviserMessagePanel _adviserMessagePanel;
        private readonly BallisticRocketProgressBar _ballisticRocketProgressBar;
        
        private readonly PlayerInitData _playerInitData;
        private readonly SmallAlienEnemyInitData _smallAlienEnemyInitData;
        private readonly BigAlienEnemyInitData _bigAlienEnemyData;
        private readonly GunnerAlienEnemyInitData _gunnerAlienEnemyData;
        private readonly StoneInitData _stoneInitData;
        private readonly CapsuleInitData _capsuleInitData;
        private readonly HealingCoreInitData _healingCoreInitData;
        private readonly GoldCoreInitData _goldCoreInitData;

        private readonly Vector3 _playerSpawnPoint;
        
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
        
        public Level Level { get; private set; }

        public List<Vector3> SmallEnemyAlienSpawnPoints { get; private set; }
        
        public List<Vector3> BigEnemyAlienSpawnPoints { get; private set; }
        
        public List<Vector3> GunnerEnemyAlienSpawnPoints { get; private set; }
        
        public List<Vector3> StoneSpawnPoints { get; private set; }
        
        public List<Vector3> GoldCoreSpawnPoints { get; private set; }
        
        public List<Vector3> HealingCoreSpawnPoints { get; private set; }

        public event Action PlayerIsSpawned;

        public GameInitSystem(PlayerInitData playerData, SmallAlienEnemyInitData smallAlienEnemyData,
            BigAlienEnemyInitData bigAlienEnemyData, GunnerAlienEnemyInitData gunnerAlienEnemyData,
            StoneInitData stoneData, CapsuleInitData capsuleData, LevelInitData levelData,
            HealingCoreInitData healingCoreData, GoldCoreInitData goldCoreData)
        {
            _playerInitData = playerData;
            _smallAlienEnemyInitData = smallAlienEnemyData;
            _bigAlienEnemyData = bigAlienEnemyData;
            _gunnerAlienEnemyData = gunnerAlienEnemyData;
            _stoneInitData = stoneData;
            _healingCoreInitData = healingCoreData;
            _goldCoreInitData = goldCoreData;
            _capsuleInitData = capsuleData;
            
            Level = Object.Instantiate(levelData.LevelPrefab);
            SmallEnemyAlienSpawnPoints = levelData.SmallEnemyAlienSpawnPoints;
            BigEnemyAlienSpawnPoints = levelData.BigEnemyAlienSpawnPoints;
            GunnerEnemyAlienSpawnPoints = levelData.GunnerEnemyAlienSpawnPoints;
            StoneSpawnPoints = levelData.StoneSpawnPoints;
            GoldCoreSpawnPoints = levelData.GoldCoreSpawnPoints;
            HealingCoreSpawnPoints = levelData.HealingCoreSpawnPoints;
            _playerSpawnPoint = levelData.PlayerSpawnPoint;
        }

        public void Init()
        {
            Player = CreatePlayer();
            PlayerHealth = Player.Health;
            Player.gameObject.SetActive(false);

            Level.GetServices(this, _timer, _adviserMessagePanel);
            
            if (Level is SecondMarsLevel secondMarsLevel)
            {
                secondMarsLevel.GetBallisticProgressBar(_ballisticRocketProgressBar);
            }

            CreateEnemyObjectPools();
        }

        public void Run()
        {
            // if (Capsule != null)
            // {
            //     LaunchPlayerCapsule();
            // }
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
            Player.gameObject.SetActive(true);
            PlayerIsSpawned?.Invoke();
            
            if (Player.Health.TargetHealth <= MinValue)
            {
                Player.Health.SetHealthValue();
            }
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
            var playerActor = Object.Instantiate(_playerInitData.Prefab, _playerSpawnPoint, Quaternion.identity);
            
            playerActor.MiningToolActor.GetAudioService(_audioSoundsService);

            PlayerTransform = playerActor.transform;
            
            var player = _world.NewEntity();

            ref var inputEventComponent = ref player.Get<InputEventComponent>();
            inputEventComponent.PlayerInputController = playerActor.PlayerInputController;
            
            ref var playerComponent = ref player.Get<PlayerComponent>();
            playerComponent.MiningTool = playerActor.MiningToolActor;

            ref var movableComponent = ref player.Get<PlayerMovableComponent>();
            movableComponent.MoveSpeed = _playerInitData.DefaultMoveSpeed;
            movableComponent.RotationSpeed = _playerInitData.DefaultRotationSpeed;
            movableComponent.Transform = playerActor.transform;
            movableComponent.Rigidbody = playerActor.Rigidbody;

            ref var animationsComponent = ref player.Get<AnimatedComponent>();
            animationsComponent.Animator = playerActor.Animator;

            return playerActor;
        }
        
        public SmallAlienEnemy CreateSmallAlienEnemy(PlayerActor target)
        {
            var smallEnemyAlienActor = _smallAlienEnemyPool.GetFreeElement();
            smallEnemyAlienActor.Construct(_experiencePoints, _textService);

            if (smallEnemyAlienActor.Health.TargetHealth <= MinValue)
            {
                smallEnemyAlienActor.Health.SetHealthValue();
            }
            
            var entity = _world.NewEntity();
            
            ref var enemyComponent = ref entity.Get<EnemyComponent>();
            enemyComponent.Health = smallEnemyAlienActor.Health;

            ref var enemyMovableComponent = ref entity.Get<EnemyMovableComponent>();
            enemyMovableComponent.Transform = smallEnemyAlienActor.transform;
            enemyMovableComponent.IsMoving = true;

            ref var enemyAnimationsComponent = ref entity.Get<AnimatedComponent>();
            enemyAnimationsComponent.Animator = smallEnemyAlienActor.Animator;

            ref var followComponent = ref entity.Get<FollowPlayerComponent>();
            followComponent.Target = target;
            followComponent.NavMeshAgent = smallEnemyAlienActor.NavMeshAgent;

            ref var attackComponent = ref entity.Get<EnemyMeleeAttackComponent>();
            attackComponent.Damage = _smallAlienEnemyInitData.DefaultDamage;

            return smallEnemyAlienActor;
        }

        public BigAlienEnemy CreateBigAlienEnemy(PlayerActor target)
        {
            var bigEnemyAlienActor = _bigAlienEnemyPool.GetFreeElement();
            bigEnemyAlienActor.Construct(_experiencePoints, _textService);
            
            if (bigEnemyAlienActor.Health.TargetHealth <= MinValue)
            {
                bigEnemyAlienActor.Health.SetHealthValue();
            }
            
            var entity = _world.NewEntity();
            
            ref var enemyComponent = ref entity.Get<EnemyComponent>();
            enemyComponent.Health = bigEnemyAlienActor.Health;

            ref var enemyMovableComponent = ref entity.Get<EnemyMovableComponent>();
            enemyMovableComponent.Transform = bigEnemyAlienActor.transform;
            enemyMovableComponent.IsMoving = true;

            ref var enemyAnimationsComponent = ref entity.Get<AnimatedComponent>();
            AnimatedStateMachine animatedStateMachine = new(bigEnemyAlienActor.Animator);
            enemyAnimationsComponent.AnimatedStateMachine = animatedStateMachine;
            enemyAnimationsComponent.Animator = bigEnemyAlienActor.Animator;

            ref var followComponent = ref entity.Get<FollowPlayerComponent>();
            followComponent.Target = target;
            followComponent.NavMeshAgent = bigEnemyAlienActor.NavMeshAgent;

            ref var attackComponent = ref entity.Get<EnemyBigAlienAttackComponent>();

            bigEnemyAlienActor.Weapon.SetData(target.transform, _bigAlienEnemyProjectilePool);

            return bigEnemyAlienActor;
        }

        public GunnerAlienEnemy CreateGunnerAlienEnemy(PlayerActor target)
        {
            var gunnerEnemyAlienActor = _gunnerAlienEnemyPool.GetFreeElement();
            gunnerEnemyAlienActor.Construct(_experiencePoints, _textService);
            
            if (gunnerEnemyAlienActor.Health.TargetHealth <= MinValue)
            {
                gunnerEnemyAlienActor.Health.SetHealthValue();
            }
            
            var entity = _world.NewEntity();
            
            ref var enemyComponent = ref entity.Get<EnemyComponent>();
            enemyComponent.Health = gunnerEnemyAlienActor.Health;

            ref var enemyMovableComponent = ref entity.Get<EnemyMovableComponent>();
            enemyMovableComponent.Transform = gunnerEnemyAlienActor.transform;
            enemyMovableComponent.IsMoving = true;

            ref var enemyAnimationsComponent = ref entity.Get<AnimatedComponent>();
            AnimatedStateMachine animatedStateMachine = new(gunnerEnemyAlienActor.Animator);
            enemyAnimationsComponent.AnimatedStateMachine = animatedStateMachine;
            enemyAnimationsComponent.Animator = gunnerEnemyAlienActor.Animator;

            ref var followComponent = ref entity.Get<FollowPlayerComponent>();
            followComponent.Target = target;
            followComponent.NavMeshAgent = gunnerEnemyAlienActor.NavMeshAgent;

            ref var attackComponent = ref entity.Get<EnemyGunnerAlienAttackComponent>();

            gunnerEnemyAlienActor.Weapon.SetData(target.transform, _gunnerAlienEnemyProjectilePool);

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
            goldCore.GetServices(_textService, _goldView);
            
            InitResource(goldCore);
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
            if(SmallEnemyAlienSpawnPoints.Count > 0)
                _smallAlienEnemyPool = new ObjectPool<SmallAlienEnemy>(_smallAlienEnemyInitData.SmallAlienEnemyPrefab,
                    SmallEnemyAlienSpawnPoints.Count, new GameObject(SmallEnemyAlienPool).transform)
                {
                    AutoExpand = IsAutoExpand
                };

            if (BigEnemyAlienSpawnPoints.Count > 0)
            {
                _bigAlienEnemyPool = new ObjectPool<BigAlienEnemy>(_bigAlienEnemyData.BigAlienEnemyPrefab,
                    BigEnemyAlienSpawnPoints.Count, new GameObject(BigEnemyAlienPool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
                
                _bigAlienEnemyProjectilePool = new ObjectPool<BigAlienEnemyProjectile>(_bigAlienEnemyData.ProjectilePrefab, 
                    CountAlienEnemyProjectile, new GameObject(BigAlienEnemyProjectilePool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
            }

            if (GunnerEnemyAlienSpawnPoints.Count > 0)
            {
                _gunnerAlienEnemyPool = new ObjectPool<GunnerAlienEnemy>(_gunnerAlienEnemyData.GunnerAlienEnemyPrefab, 
                    GunnerEnemyAlienSpawnPoints.Count, new GameObject(GunnerAlienEnemyPool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
                
                _gunnerAlienEnemyProjectilePool = new ObjectPool<GunnerAlienEnemyProjectile>(_gunnerAlienEnemyData.ProjectilePrefab, 
                    CountAlienEnemyProjectile, new GameObject(GunnerAlienEnemyProjectilePool).transform)
                {
                    AutoExpand = IsAutoExpand
                };
            }
        }
    }
}