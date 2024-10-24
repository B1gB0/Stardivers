using System;
using System.Collections.Generic;
using Project.Scripts.ECS.System;
using UnityEngine;

namespace Project.Scripts.Operations
{
    public class FirstLevel : Level
    {
        [SerializeField] private EnemySpawnTrigger _enemySpawnTrigger;

        private void Start()
        {
            if (IsLaunchedPlayerCapsule)
            {
                _gameInitSystem.CreateCapsule();
            }
            
            _enemySpawnTrigger.IsTimerLaunched += _timer.Show;
            _enemySpawnTrigger.IsTimerLaunched += _timer.OnLaunchTimer;
        }

        private void Update()
        {
            if (_enemySpawnTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy();
            }

            if (_timer._timeInSeconds <= MinValue)
            {
                _enemySpawnTrigger.CompleteSpawn();
                _timer.OffLaunchTimer();
                _timer.Hide();
            }
        }

        private void OnDestroy()
        {
            _enemySpawnTrigger.IsTimerLaunched -= _timer.Show;
            _enemySpawnTrigger.IsTimerLaunched -= _timer.OnLaunchTimer;
        }
    }
}