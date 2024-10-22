using System;
using Project.Scripts.ECS.System;
using UnityEngine;

namespace Project.Scripts.Operations
{
    public class Level : MonoBehaviour
    {
        protected const float MinValue = 0f;
        
        [field: SerializeField] public bool IsLaunchedPlayerCapsule { get; private set; }
        
        [SerializeField] private float Delay = 10f;
        
        protected Timer _timer;
        
        private GameInitSystem _gameInitSystem;
        private float _lastSpawnTime;
        
        private void Start()
        {
            if (IsLaunchedPlayerCapsule)
            {
                _gameInitSystem.CreateCapsule();
            }
        }
        
        public void GetServices(GameInitSystem gameInitSystem, Timer timer)
        {
            _gameInitSystem = gameInitSystem;
            _timer = timer;
        }

        protected void CreateWaveOfEnemy()
        {
            if (_lastSpawnTime <= MinValue)
            {
                _gameInitSystem.SpawnEnemy();

                _lastSpawnTime = Delay;
            }

            _lastSpawnTime -= Time.deltaTime;
        }
    }
}