using Cysharp.Threading.Tasks;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.ThirdLevel
{
    public class ThirdMysteryPlanetLevel : Level
    {
        [SerializeField] private EnemySpawnTriggerWithoutEffect _enemySpawnTriggerWithoutEffect;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;

        private AlienCocoonView _alienCocoonView;
        private ObjectiveTextView _objectiveTextView;

        private void OnEnable()
        {
            IsInitiatedSpawners += SpawnResources;
            IsInitiatedSpawners += SpawnAlienCocoons;
        }

        private void OnDisable()
        {
            IsInitiatedSpawners -= SpawnResources;
            IsInitiatedSpawners -= SpawnAlienCocoons;
        }

        public override async UniTask OnStartLevel()
        {
            await base.OnStartLevel();
            
            _alienCocoonView = await ViewFactory.CreateAlienCocoonView();
            _objectiveTextView = await ViewFactory.CreateObjectiveText();

            OnAlienCocoonViewShow += _alienCocoonView.Show;

            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;

            _enemySpawnTriggerWithoutEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithoutEffect.EnemySpawned += _objectiveTextView.Show;
            _enemySpawnTriggerWithoutEffect.EnemySpawned += CreateAllAlienEnemyTurrets;
            _enemySpawnTriggerWithoutEffect.EnemySpawned += DialogueSetter.OnEnemySpawnTriggerWithEffect;
            
            CurrencyService.OnAllAlienCocoonsCollected += DialogueSetter.OnEndAttack;
        }

        private void FixedUpdate()
        {
            if (_enemySpawnTriggerWithoutEffect.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
        }

        private void OnDestroy()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            OnAlienCocoonViewShow -= _alienCocoonView.Show;
            
            _enemySpawnTriggerWithoutEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithoutEffect.EnemySpawned -= _objectiveTextView.Show;
            _enemySpawnTriggerWithoutEffect.EnemySpawned -= CreateAllAlienEnemyTurrets;
            _enemySpawnTriggerWithoutEffect.EnemySpawned -= DialogueSetter.OnEnemySpawnTriggerWithEffect;
            
            CurrencyService.OnAllAlienCocoonsCollected -= DialogueSetter.OnEndAttack;
        }
    }
}