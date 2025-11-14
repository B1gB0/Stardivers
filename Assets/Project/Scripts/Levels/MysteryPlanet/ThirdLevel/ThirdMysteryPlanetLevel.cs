using Cysharp.Threading.Tasks;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.ThirdLevel
{
    public class ThirdMysteryPlanetLevel : Level
    {
        [SerializeField] private EnemySpawnTriggerWithoutEffect _enemySpawnFirstTriggerWithoutEffect;
        [SerializeField] private EnemySpawnTriggerWithoutEffect _enemySpawnSecondTriggerWithoutEffect;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;

        private AlienCocoonView _alienCocoonView;
        private ObjectiveTextView _objectiveTextView;

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
        }

        public override async UniTask OnStartLevel()
        {
            await base.OnStartLevel();
            
            _alienCocoonView = await ViewFactory.CreateAlienCocoonView();
            _objectiveTextView = await ViewFactory.CreateObjectiveText();

            OnAlienCocoonViewShow += _alienCocoonView.Show;

            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;

            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned += _objectiveTextView.Show;
            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned += CreateAllAlienEnemyTurrets;
            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned += DialogueSetter.OnEnemySpawnTriggerWithEffect;
            
            CurrencyService.OnAllAlienCocoonsCollected += DialogueSetter.OnEndAttack;
            CurrencyService.OnAllAlienCocoonsCollected += _enemySpawnFirstTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected += _enemySpawnSecondTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected += EntranceToNextLvlTrigger.Activate;
            CurrencyService.OnAllAlienCocoonsCollected += EndLevelTrigger.Activate;
        }

        private void FixedUpdate()
        {
            if (_enemySpawnFirstTriggerWithoutEffect.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }

            if (_enemySpawnSecondTriggerWithoutEffect.IsEnemySpawned)
            {
                CreateWaveOfEnemy(SecondWaveEnemy);
            }
        }

        private void OnDestroy()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            OnAlienCocoonViewShow -= _alienCocoonView.Show;
            
            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned -= _objectiveTextView.Show;
            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned -= CreateAllAlienEnemyTurrets;
            _enemySpawnFirstTriggerWithoutEffect.EnemySpawned -= DialogueSetter.OnEnemySpawnTriggerWithEffect;
            
            CurrencyService.OnAllAlienCocoonsCollected -= DialogueSetter.OnEndAttack;
            CurrencyService.OnAllAlienCocoonsCollected -= _enemySpawnFirstTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected -= _enemySpawnSecondTriggerWithoutEffect.CompleteSpawn;
            CurrencyService.OnAllAlienCocoonsCollected -= EntranceToNextLvlTrigger.Activate;
            CurrencyService.OnAllAlienCocoonsCollected -= EndLevelTrigger.Activate;
        }
    }
}