using System;
using System.Collections.Generic;
using Build.Game.Scripts;
using Build.Game.Scripts.ECS.Components;
using Build.Game.Scripts.ECS.Data;
using Build.Game.Scripts.ECS.Data.SO;
using Build.Game.Scripts.ECS.EntityActors;
using Leopotam.Ecs;
using Project.Game.Scripts;
using Project.Scripts.Crystals;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Experience;
using Project.Scripts.MiningResources;
using Project.Scripts.Operations;
using Project.Scripts.Projectiles.Bullets;
using Project.Scripts.Score;
using Project.Scripts.UI;
using TreeEditor;
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
        private const int MinValue = 0;
        private const int QuantityGoldCoreSpawnPoints = 3;
        private const int QuantityHealingCoreSpawnPoints = 2;

        private readonly Vector3 _stoneRotation = new (0f, 90f, 0f);
        private readonly EcsWorld _world;

        private readonly FloatingDamageTextService _damageTextService;
        private readonly AudioSoundsService _audioSoundsService;
        private readonly ExperiencePoints _experiencePoints;
        private readonly CrystalSpawner _crystalSpawner;
        private readonly Timer _timer;
        
        private readonly PlayerInitData _playerInitData;
        private readonly SmallEnemyAlienInitData _smallEnemyAlienInitData;
        private readonly BigEnemyAlienInitData _bigEnemyAlienData; 
        private readonly StoneInitData _stoneInitData;
        private readonly CapsuleInitData _capsuleInitData;
        private readonly HealingCoreInitData _healingCoreInitData;
        private readonly GoldCoreInitData _goldCoreInitData;
        
        private readonly Level _level;
        
        private readonly List<Vector3> _smallEnemyAlienSpawnPoints;
        private readonly List<Vector3> _bigEnemyAlienSpawnPoints;
        private readonly List<Vector3> _stoneSpawnPoints;
        private readonly List<Vector3> _goldCoreSpawnPoints;
        private readonly List<Vector3> _healingCoreSpawnPoints;
        private readonly Vector3 _playerSpawnPoint;

        private Vector3 _capsuleSpawnPoint;

        private PlayerActor _player;
        private SmallAlienEnemy smallAlienEnemy;
        private CapsuleActor _capsule;

        private ObjectPool<BigAlienEnemyAlien> _bigEnemyAlienPool;
        private ObjectPool<SmallAlienEnemy> _smallEnemyAlienPool;

        public Health PlayerHealth { get; private set; }
        
        public Transform PlayerTransform { get; private set; }

        public event Action PlayerIsSpawned;

        public GameInitSystem(PlayerInitData playerData, SmallEnemyAlienInitData smallEnemyAlienData,
            BigEnemyAlienInitData bigEnemyAlienData, StoneInitData stoneInitData, CapsuleInitData capsuleData,
            LevelInitData levelData, HealingCoreInitData healingCoreData, GoldCoreInitData goldCoreData)
        {
            _playerInitData = playerData;
            _smallEnemyAlienInitData = smallEnemyAlienData;
            _bigEnemyAlienData = bigEnemyAlienData;
            _stoneInitData = stoneInitData;
            _healingCoreInitData = healingCoreData;
            _goldCoreInitData = goldCoreData;
            _capsuleInitData = capsuleData;
            
            _level = Object.Instantiate(levelData.LevelPrefab);
            _smallEnemyAlienSpawnPoints = levelData.SmallEnemyAlienSpawnPoints;
            _bigEnemyAlienSpawnPoints = levelData.BigEnemyAlienSpawnPoints;
            _playerSpawnPoint = levelData.PlayerSpawnPoint;
            _stoneSpawnPoints = levelData.StoneSpawnPoints;
            _goldCoreSpawnPoints = levelData.GoldCoreSpawnPoints;
            _healingCoreSpawnPoints = levelData.HealingCoreSpawnPoints;
        }

        public void Init()
        {
            _player = CreatePlayer();
            
            PlayerHealth = _player.Health;

            _player.gameObject.SetActive(false);

            _smallEnemyAlienPool = new ObjectPool<SmallAlienEnemy>(_smallEnemyAlienInitData.SmallAlienEnemyPrefab,
                _smallEnemyAlienSpawnPoints.Count, new GameObject(SmallEnemyAlienPool).transform)
            {
                AutoExpand = IsAutoExpand
            };

            _bigEnemyAlienPool = new ObjectPool<BigAlienEnemyAlien>(_bigEnemyAlienData.BigAlienEnemyPrefab,
                _bigEnemyAlienSpawnPoints.Count, new GameObject(BigEnemyAlienPool).transform)
            {
                AutoExpand = IsAutoExpand
            };
            
            SpawnResources();
            
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
                SmallAlienEnemy smallAlienEnemy = CreateSmallEnemyAlien(_player);

                var enemySpawnPosition = enemySpawnPoint + Vector3.one * Random.Range(-2f, 2f);
                enemySpawnPosition.y = 0f;

                smallAlienEnemy.transform.position = enemySpawnPosition;
            }
            
            foreach (var enemySpawnPoint in _bigEnemyAlienSpawnPoints)
            {
                BigAlienEnemyAlien bigEnemyAlien = CreateBigEnemyAlien(_player);

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
        
        private SmallAlienEnemy CreateSmallEnemyAlien(PlayerActor target)
        {
            var smallEnemyAlienActor = _smallEnemyAlienPool.GetFreeElement();
            smallEnemyAlienActor.Construct(_experiencePoints, _damageTextService);

            if (smallEnemyAlienActor.Health.TargetHealth <= MinValue)
            {
                smallEnemyAlienActor.Health.SetHealthValue();
            }

            InitEnemyAlien(smallAlienEnemy, target);

            return smallEnemyAlienActor;
        }

        private BigAlienEnemyAlien CreateBigEnemyAlien(PlayerActor target)
        {
            var bigEnemyAlienActor = _bigEnemyAlienPool.GetFreeElement();
            bigEnemyAlienActor.Construct(_experiencePoints, _damageTextService);
            
            if (bigEnemyAlienActor.Health.TargetHealth <= MinValue)
            {
                bigEnemyAlienActor.Health.SetHealthValue();
            }
            
            InitEnemyAlien(bigEnemyAlienActor, target, bigEnemyAlienActor.Projectile);

            return bigEnemyAlienActor;
        }

        private void CreateStone(Vector3 atPosition)
        {
            var stone = Object.Instantiate(_stoneInitData.StoneActorPrefab, atPosition, Quaternion.Euler(_stoneRotation));
            stone.GetExperiencePoints(_experiencePoints);

            InitResource(stone);
        }

        private void CreateHealingCore(Vector3 atPosition)
        {
            var healingCore = Object.Instantiate(_healingCoreInitData.HealingCorePrefab, atPosition, Quaternion.identity);
            healingCore.GetExperiencePoints(_experiencePoints);
            healingCore.GetCrystalSpawner(_crystalSpawner);
            
            InitResource(healingCore);
        }
        
        private void CreateGoldCore(Vector3 atPosition)
        {
            var goldCore = Object.Instantiate(_goldCoreInitData.GoldCorePrefab, atPosition, Quaternion.identity);
            goldCore.GetExperiencePoints(_experiencePoints);
            goldCore.GetCrystalSpawner(_crystalSpawner);
            
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

        private void InitEnemyAlien(EnemyAlienActor enemy, PlayerActor target, BigEnemyAlienProjectile projectile = null)
        {
            var entity = _world.NewEntity();
            
            ref var enemyComponent = ref entity.Get<EnemyComponent>();
            enemyComponent.Health = enemy.Health;

            ref var enemyMovableComponent = ref entity.Get<EnemyMovableComponent>();
            enemyMovableComponent.Transform = enemy.transform;

            ref var enemyAnimationsComponent = ref entity.Get<AnimatedComponent>();
            enemyAnimationsComponent.Animator = enemy.Animator;

            ref var followComponent = ref entity.Get<FollowPlayerComponent>();
            followComponent.Target = target;
            followComponent.NavMeshAgent = enemy.NavMeshAgent;

            ref var attackComponent = ref entity.Get<RangeAttackComponent>();
            attackComponent.Damage = _bigEnemyAlienData.DefaultDamage;
            attackComponent.Projectile = projectile;
            
            attackComponent.Projectile.SetCharacteristics(attackComponent.Damage);
        }

        private void SpawnResources()
        {
            List<Vector3> sortedSpawnPoints = new List<Vector3>();

            foreach (var stoneSpawnPoint in _stoneSpawnPoints)
            {
                var stoneSpawnPosition = stoneSpawnPoint + Vector3.one * Random.Range(-3f, 3f);
                stoneSpawnPosition.y = 0;
            
                CreateStone(stoneSpawnPosition);   
            }

            sortedSpawnPoints = GetSortedRandomSpawnPoints(_goldCoreSpawnPoints, QuantityGoldCoreSpawnPoints);

            foreach (var goldCoreSpawnPoint in sortedSpawnPoints)
            {
                var goldCoreSpawnPosition = goldCoreSpawnPoint + Vector3.one * Random.Range(-3f, 3f);
                goldCoreSpawnPosition.y = 0;
            
                CreateGoldCore(goldCoreSpawnPosition);   
            }
            
            sortedSpawnPoints = GetSortedRandomSpawnPoints(_healingCoreSpawnPoints, QuantityHealingCoreSpawnPoints);
            
            foreach (var healingCoreSpawnPoint in sortedSpawnPoints)
            {
                var healingCoreSpawnPosition = healingCoreSpawnPoint + Vector3.one * Random.Range(-3f, 3f);
                healingCoreSpawnPosition.y = 0;
            
                CreateHealingCore(healingCoreSpawnPosition);   
            }
        }

        private List<Vector3> GetSortedRandomSpawnPoints(List<Vector3> spawnPointsData, int quantityPoints)
        {
            List<Vector3> freeSpawnPoints = spawnPointsData;
            List<Vector3> sortedSpawnPoints = new List<Vector3>();

            int counterSpawnPoints = freeSpawnPoints.Count - 1;

            for (int i = 0; i < quantityPoints; i++)
            {
                Vector3 randomPoint = freeSpawnPoints[Random.Range(MinValue, counterSpawnPoints)];
                sortedSpawnPoints.Add(randomPoint);
                //freeSpawnPoints.Remove(randomPoint);
                counterSpawnPoints--;
            }

            return sortedSpawnPoints;
        }
    }
}