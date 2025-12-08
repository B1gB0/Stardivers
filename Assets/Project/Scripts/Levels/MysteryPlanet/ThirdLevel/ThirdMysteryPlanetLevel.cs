using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.ThirdLevel
{
    public class ThirdMysteryPlanetLevel : Level
    {
        private const int LastIndexSecondAliensCocoons = 5;
        private const int LastIndexFirstAliensCocoons = 10;
        private const int MinSecondAliensCocoonsValue = 0;
        private const int MinCocoonsValue = 0;

        [SerializeField] private List<GameObject> _alienCocoonsPointers;
        [SerializeField] private List<GameObject> _enemyOutpostPointers;

        [SerializeField] private EnemySpawnTriggerWithoutEffect _enemySpawnFirstTriggerWithoutEffect;
        [SerializeField] private EnemySpawnTriggerWithoutEffect _enemySpawnSecondTriggerWithoutEffect;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;
        [SerializeField] private NestTriggerForNavigation _firstNestTrigger;
        [SerializeField] private NestTriggerForNavigation _secondNestTrigger;

        private List<AlienCocoon> _firstNestCocoons = new();
        private List<AlienCocoon> _secondNestCocoons = new();

        private AlienCocoonView _alienCocoonView;
        private ObjectiveTextView _objectiveTextView;

        private CancellationTokenSource _firstWaveCts;
        private CancellationTokenSource _secondWaveCts;

        private void OnEnable()
        {
            IsInitiatedSpawners += SpawnResources;
            IsInitiatedSpawners += SpawnAlienCocoons;
            IsInitiatedSpawners += SpawnIceCrystals;
        }

        private void OnDisable()
        {
            IsInitiatedSpawners -= SpawnResources;
            IsInitiatedSpawners -= SpawnAlienCocoons;
            IsInitiatedSpawners -= SpawnIceCrystals;

            _firstWaveCts?.Cancel();
            _secondWaveCts?.Cancel();
        }

        public override async UniTask OnStartLevel()
        {
            HideOutpostPointers();

            await base.OnStartLevel();

            for (int i = MinSecondAliensCocoonsValue; i < LastIndexSecondAliensCocoons; i++)
            {
                _secondNestCocoons.Add(ResourcesSpawner.AlienCocoons[i]);
            }

            for (int i = LastIndexSecondAliensCocoons; i < LastIndexFirstAliensCocoons; i++)
            {
                _firstNestCocoons.Add(ResourcesSpawner.AlienCocoons[i]);
            }

            _firstNestTrigger.GetData(Arrow);
            _secondNestTrigger.GetData(Arrow);

            _alienCocoonView = await ViewFactory.CreateAlienCocoonView();
            _alienCocoonView.Show();
            
            _objectiveTextView = await ViewFactory.CreateObjectiveText();

            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            WelcomePlanetTextTrigger.IsWelcomeToPlanet += CreateAllAlienEnemyTurrets;
            WelcomePlanetTextTrigger.IsWelcomeToPlanet += _objectiveTextView.Show;

            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned += StartFirstWaveSpawning;
            
            _enemySpawnSecondTriggerWithoutEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnSecondTriggerWithoutEffect.EnemySpawned += StartSecondWaveSpawning;

            CurrencyService.OnAllAlienCocoonsCollected += DialogueSetter.OnEndAttack;
            CurrencyService.OnAllAlienCocoonsCollected += _objectiveTextView.Hide;
            CurrencyService.OnAllAlienCocoonsCollected += LookArrowAtOutpost;
            CurrencyService.OnAllAlienCocoonsCollected += HideAliensCocoonsPointers;
            CurrencyService.OnAllAlienCocoonsCollected += ShowOutpostPointers;
            CurrencyService.OnAllAlienCocoonsCollected += _enemySpawnFirstTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected += _enemySpawnSecondTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected += EntranceToNextLvlTrigger.Activate;
            CurrencyService.OnAllAlienCocoonsCollected += EndLevelTrigger.Activate;

            foreach (var cocoon in _firstNestCocoons)
            {
                cocoon.OnDied += CheckFirstAliensCocoons;
            }
            
            foreach (var cocoon in _secondNestCocoons)
            {
                cocoon.OnDied += CheckSecondAliensCocoons;
            }
        }

        protected override void CreateWaveOfEnemy(int numberWaveEnemy)
        {
            CreateWaveOfSmallEnemies(numberWaveEnemy);
            CreateWaveOfBigEnemies(numberWaveEnemy);
            CreateWaveOfGunnerEnemies(numberWaveEnemy);
        }

        private void StartFirstWaveSpawning()
        {
            _firstWaveCts = new CancellationTokenSource();
            SpawnWaveContinuously(FirstWaveEnemy, _firstWaveCts.Token).Forget();
        }

        private void StartSecondWaveSpawning()
        {
            _secondWaveCts = new CancellationTokenSource();
            SpawnWaveContinuously(SecondWaveEnemy, _secondWaveCts.Token).Forget();
        }

        private async UniTaskVoid SpawnWaveContinuously(int waveIndex, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!_enemySpawnFirstTriggerWithoutEffect.IsEnemySpawned)
                    _firstWaveCts?.Cancel();

                if (!_enemySpawnSecondTriggerWithoutEffect.IsEnemySpawned)
                    _secondWaveCts?.Cancel();

                CreateWaveOfEnemy(waveIndex);
                await UniTask.Delay(TimeSpan.FromSeconds(SpawnWaveOfEnemyDelay), cancellationToken: cancellationToken);
            }
        }

        private void OnDestroy()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= CreateAllAlienEnemyTurrets;
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= _objectiveTextView.Show;

            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned -= StartFirstWaveSpawning;

            _enemySpawnSecondTriggerWithoutEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnSecondTriggerWithoutEffect.EnemySpawned -= StartSecondWaveSpawning;

            CurrencyService.OnAllAlienCocoonsCollected -= DialogueSetter.OnEndAttack;
            CurrencyService.OnAllAlienCocoonsCollected -= _objectiveTextView.Hide;
            CurrencyService.OnAllAlienCocoonsCollected -= LookArrowAtOutpost;
            CurrencyService.OnAllAlienCocoonsCollected -= HideAliensCocoonsPointers;
            CurrencyService.OnAllAlienCocoonsCollected -= ShowOutpostPointers;
            CurrencyService.OnAllAlienCocoonsCollected -= _enemySpawnFirstTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected -= _enemySpawnSecondTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected -= EntranceToNextLvlTrigger.Activate;
            CurrencyService.OnAllAlienCocoonsCollected -= EndLevelTrigger.Activate;
            
            foreach (var cocoon in _firstNestCocoons)
            {
                cocoon.OnDied -= CheckFirstAliensCocoons;
            }
            
            foreach (var cocoon in _secondNestCocoons)
            {
                cocoon.OnDied -= CheckSecondAliensCocoons;
            }

            _firstWaveCts?.Cancel();
            _secondWaveCts?.Cancel();
        }
        
        private void CheckFirstAliensCocoons(AlienCocoon alienCocoon)
        {
            alienCocoon.OnDied -= CheckFirstAliensCocoons;
            _firstNestCocoons.Remove(alienCocoon);

            if (_firstNestCocoons.Count == MinCocoonsValue && _secondNestCocoons.Count != MinCocoonsValue)
            {
                Arrow.OnLookAtTarget(_secondNestTrigger.TargetNestPoint);
                _firstNestTrigger.Deactivate();
            }
        }
        
        private void CheckSecondAliensCocoons(AlienCocoon alienCocoon)
        {
            alienCocoon.OnDied -= CheckFirstAliensCocoons;
            _secondNestCocoons.Remove(alienCocoon);

            if (_secondNestCocoons.Count == MinCocoonsValue && _firstNestCocoons.Count != MinCocoonsValue)
            {
                Arrow.OnLookAtTarget(_firstNestTrigger.TargetNestPoint);
                _secondNestTrigger.Deactivate();
            }
        }

        private void LookArrowAtOutpost()
        {
            Arrow.OnLookAtTarget(EndLevelTrigger.transform);
        }

        private void ShowOutpostPointers()
        {
            foreach (var pointer in _enemyOutpostPointers)
            {
                pointer.gameObject.SetActive(true);
            }
        }

        private void HideOutpostPointers()
        {
            foreach (var pointer in _enemyOutpostPointers)
            {
                pointer.gameObject.SetActive(false);
            }
        }

        private void HideAliensCocoonsPointers()
        {
            foreach (var pointer in _alienCocoonsPointers)
            {
                pointer.gameObject.SetActive(false);
            }
        }
    }
}