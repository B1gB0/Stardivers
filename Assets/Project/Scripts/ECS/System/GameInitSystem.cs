using System;
using System.Collections.Generic;
using Build.Game.Scripts;
using Build.Game.Scripts.ECS.Components;
using Build.Game.Scripts.ECS.Data;
using Build.Game.Scripts.ECS.Data.SO;
using Build.Game.Scripts.ECS.EntityActors;
using Leopotam.Ecs;
using Project.Game.Scripts;
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
        private const string EnemyPool = nameof(EnemyPool);
        private const bool IsAutoExpand = true;
        
        private const float CapsuleHeight = 20f;

        private readonly EcsWorld _world;

        private readonly FloatingDamageTextService _damageTextService;
        private readonly AudioSoundsService _audioSoundsService;
        private readonly ExperiencePoints _experiencePoints;
        private readonly Timer _timer;
        
        private readonly PlayerInitData _playerInitData;
        private readonly EnemyInitData _enemyInitData;
        private readonly StoneInitData _stoneInitData;
        private readonly CapsuleInitData _capsuleInitData;
        
        private readonly Level _level;
        
        private readonly List<Vector3> _enemySpawnPoints;
        private readonly List<Vector3> _stoneSpawnPoints;
        private readonly Vector3 _playerSpawnPoint;

        private Vector3 _capsuleSpawnPoint;

        private PlayerActor _player;
        private EnemyActor _enemy;
        private CapsuleActor _capsule;

        private ObjectPool<EnemyActor> _enemyPool;
        
        public Health PlayerHealth { get; private set; }
        
        public Transform PlayerTransform { get; private set; }

        public event Action PlayerIsSpawned;

        public GameInitSystem(PlayerInitData playerData, EnemyInitData enemyData, StoneInitData stoneInitData,
            CapsuleInitData capsuleData, LevelInitData levelData)
        {
            _playerInitData = playerData;
            _enemyInitData = enemyData;
            _stoneInitData = stoneInitData;
            _capsuleInitData = capsuleData;

            _level = Object.Instantiate(levelData.LevelPrefab);
            _enemySpawnPoints = levelData.EnemySpawnPoints;
            _playerSpawnPoint = levelData.PlayerSpawnPoint;
            _stoneSpawnPoints = levelData.StoneSpawnPoints;
        }

        public void Init()
        {
            _player = CreatePlayer();
            
            PlayerHealth = _player.Health;

            _player.gameObject.SetActive(false);

            _enemyPool = new ObjectPool<EnemyActor>(_enemyInitData.EnemyPrefab, _enemySpawnPoints.Count, 
                new GameObject(EnemyPool).transform)
            {
                AutoExpand = IsAutoExpand
            };

            foreach (var resourcesSpawnPoint in _stoneSpawnPoints)
            {
                var resourceSpawnPosition = resourcesSpawnPoint + Vector3.one * Random.Range(-2f, 2f);
                resourceSpawnPosition.y = 0;
            
                CreateStone(resourceSpawnPosition);   
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
            foreach (var enemySpawnPoint in _enemySpawnPoints)
            {
                EnemyActor enemy = CreateEnemy(_player);

                var enemySpawnPosition = enemySpawnPoint + Vector3.one * Random.Range(-2f, 2f);
                enemySpawnPosition.y = 0f;

                enemy.transform.position = enemySpawnPosition;
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
            playerComponent.weapons = playerActor.Weapons;
            playerComponent.health = playerActor.Health;

            ref var movableComponent = ref player.Get<MovableComponent>();
            movableComponent.moveSpeed = _playerInitData.DefaultMoveSpeed;
            movableComponent.rotationSpeed = _playerInitData.DefaultRotationSpeed;
            movableComponent.transform = playerActor.transform;
            movableComponent.rigidbody = playerActor.Rigidbody;

            ref var animationsComponent = ref player.Get<AnimatedComponent>();
            animationsComponent.animator = playerActor.Animator;
            
            ref var attackComponent = ref player.Get<AttackComponent>();

            return playerActor;
        }
        
        private EnemyActor CreateEnemy(PlayerActor target)
        {
            //var enemyActor = Object.Instantiate(_enemyInitData.EnemyPrefab);
            var enemyActor = _enemyPool.GetFreeElement();
            enemyActor.Construct(_experiencePoints, _damageTextService);

            var enemy = _world.NewEntity();
            
            ref var enemyComponent = ref enemy.Get<EnemyComponent>();
            enemyComponent.health = enemyActor.Health;

            ref var enemyMovableComponent = ref enemy.Get<MovableComponent>();
            enemyMovableComponent.moveSpeed = _enemyInitData.DefaultMoveSpeed;
            enemyMovableComponent.rotationSpeed = _enemyInitData.DefaultRotationSpeed;
            enemyMovableComponent.transform = enemyActor.transform;
            enemyMovableComponent.rigidbody = enemyActor.Rigidbody;

            ref var enemyAnimationsComponent = ref enemy.Get<AnimatedComponent>();
            enemyAnimationsComponent.animator = enemyActor.Animator;

            ref var followComponent = ref enemy.Get<FollowPlayerComponent>();
            followComponent.target = target;
            followComponent.navMeshAgent = enemyActor.NavMeshAgent;

            ref var attackComponent = ref enemy.Get<AttackComponent>();
            attackComponent.damage = _enemyInitData.DefaultDamage;

            return enemyActor;
        }

        private void CreateStone(Vector3 atPosition)
        {
            var stoneActor = Object.Instantiate(_stoneInitData.StonePrefab, atPosition, Quaternion.identity);
            stoneActor.Construct(_experiencePoints);

            var resource = _world.NewEntity();

            ref var resourceComponent = ref resource.Get<ResourceComponent>();
            resourceComponent.health = stoneActor.Health;

            ref var animatedComponent = ref resource.Get<AnimatedComponent>();
            animatedComponent.animator = stoneActor.Animator;
        }
    }
}