using System;
using System.Collections.Generic;
using Build.Game.Scripts;
using Build.Game.Scripts.ECS.Components;
using Build.Game.Scripts.ECS.Data;
using Build.Game.Scripts.ECS.Data.SO;
using Build.Game.Scripts.ECS.EntityActors;
using Leopotam.Ecs;
using Project.Game.Scripts;
using Project.Scripts.ECS.Data;
using Project.Scripts.Operations;
using Project.Scripts.Score;
using Project.Scripts.UI;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Project.Scripts.ECS.System
{
    public class GameInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const string SmallEnemyAlienPool = nameof(SmallEnemyAlienPool);
        private const string BigEnemyAlienPool = nameof(BigEnemyAlienPool);
        
        private const bool IsAutoExpand = true;
        
        private const float CapsuleHeight = 20f;
        private const float MinValue = 0f;

        private readonly Vector3 _stoneRotation = new (0f, 90f, 0f);
        private readonly EcsWorld _world;

        private readonly FloatingDamageTextService _damageTextService;
        private readonly AudioSoundsService _audioSoundsService;
        private readonly ExperiencePoints _experiencePoints;
        private readonly Timer _timer;
        
        private readonly PlayerInitData _playerInitData;
        private readonly SmallEnemyAlienInitData _smallEnemyAlienInitData;
        private readonly BigEnemyAlienInitData bigEnemyAlienData; 
        private readonly StoneInitData _stoneInitData;
        private readonly CapsuleInitData _capsuleInitData;
        
        private readonly Level _level;
        
        private readonly List<Vector3> _smallEnemyAlienSpawnPoints;
        private readonly List<Vector3> _bigEnemyAlienSpawnPoints;
        private readonly List<Vector3> _stoneSpawnPoints;
        private readonly Vector3 _playerSpawnPoint;

        private Vector3 _capsuleSpawnPoint;

        private PlayerActor _player;
        private SmallAlienEnemyActor smallAlienEnemy;
        private CapsuleActor _capsule;

        private ObjectPool<BigAlienEnemyAlienActor> _bigEnemyAlienPool;
        private ObjectPool<SmallAlienEnemyActor> _smallEnemyAlienPool;
        
        public Health PlayerHealth { get; private set; }
        
        public Transform PlayerTransform { get; private set; }

        public event Action PlayerIsSpawned;

        public GameInitSystem(PlayerInitData playerData, SmallEnemyAlienInitData smallEnemyAlienData,
            BigEnemyAlienInitData bigEnemyAlienData, StoneInitData stoneInitData, CapsuleInitData capsuleData,
            LevelInitData levelData)
        {
            _playerInitData = playerData;
            _smallEnemyAlienInitData = smallEnemyAlienData;
            this.bigEnemyAlienData = bigEnemyAlienData;
            _stoneInitData = stoneInitData;
            _capsuleInitData = capsuleData;

            _level = Object.Instantiate(levelData.LevelPrefab);
            _smallEnemyAlienSpawnPoints = levelData.SmallEnemyAlienSpawnPoints;
            _bigEnemyAlienSpawnPoints = levelData.BigEnemyAlienSpawnPoints;
            _playerSpawnPoint = levelData.PlayerSpawnPoint;
            _stoneSpawnPoints = levelData.StoneSpawnPoints;
        }

        public void Init()
        {
            _player = CreatePlayer();
            
            PlayerHealth = _player.Health;

            _player.gameObject.SetActive(false);

            _smallEnemyAlienPool = new ObjectPool<SmallAlienEnemyActor>(_smallEnemyAlienInitData.SmallAlienEnemyPrefab,
                _smallEnemyAlienSpawnPoints.Count, new GameObject(SmallEnemyAlienPool).transform)
            {
                AutoExpand = IsAutoExpand
            };

            _bigEnemyAlienPool = new ObjectPool<BigAlienEnemyAlienActor>(bigEnemyAlienData.BigAlienEnemyPrefab,
                _bigEnemyAlienSpawnPoints.Count, new GameObject(BigEnemyAlienPool).transform)
            {
                AutoExpand = IsAutoExpand
            };

            foreach (var stoneSpawnPoint in _stoneSpawnPoints)
            {
                var stoneSpawnPosition = stoneSpawnPoint + Vector3.one * Random.Range(-3f, 3f);
                stoneSpawnPosition.y = 0;
            
                CreateStone(stoneSpawnPosition);   
            }
            
            _level.GetServices(this, _timer);

            if (!_level.IsLaunchedPlayerCapsule)
            {
                SpawnPlayer();
            }
        }

        public void Run()
        {
            if (_capsule != null)
            {
                LaunchPlayerCapsule();
            }
        }
        
        public void SpawnEnemy()
        {
            foreach (var enemySpawnPoint in _smallEnemyAlienSpawnPoints)
            {
                SmallAlienEnemyActor smallAlienEnemy = CreateSmallEnemyAlien(_player);

                var enemySpawnPosition = enemySpawnPoint + Vector3.one * Random.Range(-2f, 2f);
                enemySpawnPosition.y = 0f;

                smallAlienEnemy.transform.position = enemySpawnPosition;
            }
            
            foreach (var enemySpawnPoint in _bigEnemyAlienSpawnPoints)
            {
                BigAlienEnemyAlienActor bigEnemyAlien = CreateBigEnemyAlien(_player);

                var enemySpawnPosition = enemySpawnPoint + Vector3.one * Random.Range(-2f, 2f);
                enemySpawnPosition.y = 0f;

                bigEnemyAlien.transform.position = enemySpawnPosition;
            }
        }
        
        public void CreateCapsule()
        {
            _audioSoundsService.PlaySound(Sounds.CapsuleFlight);
            
            _capsuleSpawnPoint = _playerSpawnPoint;
            _capsuleSpawnPoint.y += CapsuleHeight;
            
            _capsule = Object.Instantiate(_capsuleInitData.Prefab, _capsuleSpawnPoint, Quaternion.identity);
        }

        private void SpawnPlayer()
        {
            _player.gameObject.SetActive(true);
        }

        private void LaunchPlayerCapsule()
        {
            _capsule.transform.position = Vector3.MoveTowards(_capsule.transform.position, _playerSpawnPoint,
                _capsuleInitData.DefaultMoveSpeed * Time.deltaTime);

            if (_capsule.transform.position == _playerSpawnPoint)
            {
                PlayerIsSpawned?.Invoke();
                SpawnPlayer();
                _capsule.Destroy();
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
            playerComponent.Weapons = playerActor.Weapons;
            playerComponent.Health = playerActor.Health;

            ref var movableComponent = ref player.Get<PlayerMovableComponent>();
            movableComponent.MoveSpeed = _playerInitData.DefaultMoveSpeed;
            movableComponent.RotationSpeed = _playerInitData.DefaultRotationSpeed;
            movableComponent.Transform = playerActor.transform;
            movableComponent.Rigidbody = playerActor.Rigidbody;

            ref var animationsComponent = ref player.Get<AnimatedComponent>();
            animationsComponent.Animator = playerActor.Animator;

            return playerActor;
        }
        
        private SmallAlienEnemyActor CreateSmallEnemyAlien(PlayerActor target)
        {
            var smallEnemyAlienActor = _smallEnemyAlienPool.GetFreeElement();
            smallEnemyAlienActor.Construct(_experiencePoints, _damageTextService);

            if (smallEnemyAlienActor.Health.TargetHealth <= MinValue)
            {
                smallEnemyAlienActor.Health.SetHealthValue();
            }

            var enemy = _world.NewEntity();
            
            ref var enemyComponent = ref enemy.Get<EnemyComponent>();
            enemyComponent.Health = smallEnemyAlienActor.Health;

            ref var enemyMovableComponent = ref enemy.Get<EnemyMovableComponent>();
            enemyMovableComponent.Transform = smallEnemyAlienActor.transform;

            ref var enemyAnimationsComponent = ref enemy.Get<AnimatedComponent>();
            enemyAnimationsComponent.Animator = smallEnemyAlienActor.Animator;

            ref var followComponent = ref enemy.Get<FollowPlayerComponent>();
            followComponent.Target = target;
            followComponent.NavMeshAgent = smallEnemyAlienActor.NavMeshAgent;

            ref var attackComponent = ref enemy.Get<MeleeAttackComponent>();
            attackComponent.Damage = _smallEnemyAlienInitData.DefaultDamage;

            return smallEnemyAlienActor;
        }

        private BigAlienEnemyAlienActor CreateBigEnemyAlien(PlayerActor target)
        {
            var bigEnemyAlienActor = _bigEnemyAlienPool.GetFreeElement();
            bigEnemyAlienActor.Construct(_experiencePoints, _damageTextService);
            
            if (bigEnemyAlienActor.Health.TargetHealth <= MinValue)
            {
                bigEnemyAlienActor.Health.SetHealthValue();
            }
            
            var enemy = _world.NewEntity();
            
            ref var enemyComponent = ref enemy.Get<EnemyComponent>();
            enemyComponent.Health = bigEnemyAlienActor.Health;

            ref var enemyMovableComponent = ref enemy.Get<EnemyMovableComponent>();
            enemyMovableComponent.Transform = bigEnemyAlienActor.transform;

            ref var enemyAnimationsComponent = ref enemy.Get<AnimatedComponent>();
            enemyAnimationsComponent.Animator = bigEnemyAlienActor.Animator;

            ref var followComponent = ref enemy.Get<FollowPlayerComponent>();
            followComponent.Target = target;
            followComponent.NavMeshAgent = bigEnemyAlienActor.NavMeshAgent;

            ref var attackComponent = ref enemy.Get<RangeAttackComponent>();
            attackComponent.Damage = bigEnemyAlienData.DefaultDamage;
            attackComponent.ProjectileSpeed = bigEnemyAlienData.ProjectileSpeed;
            attackComponent.Projectile = bigEnemyAlienData.BigAlienEnemyAlienProjectilePrefab;

            return bigEnemyAlienActor;
        }

        private void CreateStone(Vector3 atPosition)
        {
            var stoneActor = Object.Instantiate(_stoneInitData.StonePrefab, atPosition, Quaternion.Euler(_stoneRotation));
            stoneActor.Construct(_experiencePoints);

            var resource = _world.NewEntity();

            ref var resourceComponent = ref resource.Get<ResourceComponent>();
            resourceComponent.Health = stoneActor.Health;

            ref var animatedComponent = ref resource.Get<AnimatedComponent>();
            animatedComponent.Animator = stoneActor.Animator;
        }
    }
}