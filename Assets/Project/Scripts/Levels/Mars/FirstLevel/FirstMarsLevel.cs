using Project.Scripts.Levels.TextForAdviser;
using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.FirstLevel
{
    public class FirstMarsLevel : Level
    {
        private readonly FirstLevelAdviserTextsSetter _firstLevelAdviserTextsSetter = new ();
        
        [SerializeField] private EnemySpawnTrigger _enemySpawnTrigger;
        [SerializeField] private WelcomeMarsTextTrigger _welcomeMarsTextTrigger;

        private void Start()
        {
            SpawnPlayer();
            
            _firstLevelAdviserTextsSetter.GetAdviserPanel(AdviserMessagePanel);

            _enemySpawnTrigger.EnemySpawned += Timer.Show;
            _enemySpawnTrigger.EnemySpawned += Timer.OnLaunchTimer;
            _enemySpawnTrigger.EnemySpawned += _firstLevelAdviserTextsSetter.SetAndShowEnemySpawnTriggerText;
            
            Timer.IsEndAttack += _firstLevelAdviserTextsSetter.SetAndShowEndAttackText;
            Timer.IsEndAttack += _enemySpawnTrigger.CompleteSpawn;
            Timer.IsEndAttack += EntranceTrigger.Activate;
            Timer.IsEndAttack += EndLevelTrigger.Activate;
        }

        private void OnEnable()
        {
            _welcomeMarsTextTrigger.IsWelcomeToMars += _firstLevelAdviserTextsSetter.SetAndShowWelcomeMarsText;
        }

        private void OnDisable()
        {
            _welcomeMarsTextTrigger.IsWelcomeToMars -= _firstLevelAdviserTextsSetter.SetAndShowWelcomeMarsText;
        }

        private void Update()
        {
            if (_enemySpawnTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy();
            }
        }

        private void OnDestroy()
        {
            _enemySpawnTrigger.EnemySpawned -= Timer.Show;
            _enemySpawnTrigger.EnemySpawned -= Timer.OnLaunchTimer;
            _enemySpawnTrigger.EnemySpawned -= _firstLevelAdviserTextsSetter.SetAndShowEnemySpawnTriggerText;
            
            Timer.IsEndAttack -= _firstLevelAdviserTextsSetter.SetAndShowEndAttackText;
            Timer.IsEndAttack -= _enemySpawnTrigger.CompleteSpawn;
            Timer.IsEndAttack -= EntranceTrigger.Activate;
            Timer.IsEndAttack -= EndLevelTrigger.Activate;
        }
    }
}