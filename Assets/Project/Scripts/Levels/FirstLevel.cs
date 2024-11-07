using Project.Scripts.Levels;
using Project.Scripts.Levels.TextForAdviser;
using UnityEngine;

namespace Project.Scripts.Operations
{
    public class FirstLevel : Level
    {
        private readonly FirstLevelAdviserTextsSetter _firstLevelAdviserTextsSetter = new ();
        
        [SerializeField] private EnemySpawnTrigger _enemySpawnTrigger;
        [SerializeField] private WelcomeMarsTextTrigger _welcomeMarsTextTrigger;

        private void Start()
        {
            _firstLevelAdviserTextsSetter.GetAdviserPanel(AdviserMessagePanel);
            
            if (IsLaunchedPlayerCapsule)
            {
                GameInitSystem.CreateCapsule();
            }
            
            _enemySpawnTrigger.IsTimerLaunched += Timer.Show;
            _enemySpawnTrigger.IsTimerLaunched += Timer.OnLaunchTimer;
            _enemySpawnTrigger.IsTimerLaunched += _firstLevelAdviserTextsSetter.SetAndShowEnemySpawnTriggerText;
            Timer.IsEndAttack += _firstLevelAdviserTextsSetter.SetAndShowEndAttackText;
            Timer.IsEndAttack += _enemySpawnTrigger.CompleteSpawn;
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
            _enemySpawnTrigger.IsTimerLaunched -= Timer.Show;
            _enemySpawnTrigger.IsTimerLaunched -= Timer.OnLaunchTimer;
            _enemySpawnTrigger.IsTimerLaunched -= _firstLevelAdviserTextsSetter.SetAndShowEnemySpawnTriggerText;
            Timer.IsEndAttack -= _firstLevelAdviserTextsSetter.SetAndShowEndAttackText;
            Timer.IsEndAttack -= _enemySpawnTrigger.CompleteSpawn;
        }
    }
}