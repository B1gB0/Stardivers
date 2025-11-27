using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.ThirdLevel
{
    public class ThirdMysteryPlanetLevel : Level
    {
        [SerializeField] private List<GameObject> _alienCocoonsPointers;
        [SerializeField] private List<GameObject> _enemyOutpostPointers;
        
        [SerializeField] private EnemySpawnTriggerWithoutEffect _enemySpawnFirstTriggerWithoutEffect;
        [SerializeField] private EnemySpawnTriggerWithoutEffect _enemySpawnSecondTriggerWithoutEffect;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;

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

            _alienCocoonView = await ViewFactory.CreateAlienCocoonView();
            _objectiveTextView = await ViewFactory.CreateObjectiveText();

            OnAlienCocoonViewShow += _alienCocoonView.Show;

            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            WelcomePlanetTextTrigger.IsWelcomeToPlanet += CreateAllAlienEnemyTurrets;
            WelcomePlanetTextTrigger.IsWelcomeToPlanet += _objectiveTextView.Show;

            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned += StartFirstWaveSpawning;

            _enemySpawnSecondTriggerWithoutEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnSecondTriggerWithoutEffect.EnemySpawned += StartSecondWaveSpawning;

            CurrencyService.OnAllAlienCocoonsCollected += DialogueSetter.OnEndAttack;
            CurrencyService.OnAllAlienCocoonsCollected += HideAliensCocoonsPointers;
            CurrencyService.OnAllAlienCocoonsCollected += ShowOutpostPointers;
            CurrencyService.OnAllAlienCocoonsCollected += _enemySpawnFirstTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected += _enemySpawnSecondTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected += EntranceToNextLvlTrigger.Activate;
            CurrencyService.OnAllAlienCocoonsCollected += EndLevelTrigger.Activate;
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

            OnAlienCocoonViewShow -= _alienCocoonView.Show;

            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned -= StartFirstWaveSpawning;

            _enemySpawnSecondTriggerWithoutEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnSecondTriggerWithoutEffect.EnemySpawned -= StartSecondWaveSpawning;

            CurrencyService.OnAllAlienCocoonsCollected -= DialogueSetter.OnEndAttack;
            CurrencyService.OnAllAlienCocoonsCollected -= HideAliensCocoonsPointers;
            CurrencyService.OnAllAlienCocoonsCollected -= ShowOutpostPointers;
            CurrencyService.OnAllAlienCocoonsCollected -= _enemySpawnFirstTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected -= _enemySpawnSecondTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected -= EntranceToNextLvlTrigger.Activate;
            CurrencyService.OnAllAlienCocoonsCollected -= EndLevelTrigger.Activate;

            _firstWaveCts?.Cancel();
            _secondWaveCts?.Cancel();
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