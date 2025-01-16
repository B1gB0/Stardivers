using System;
using Project.Scripts.ECS.System;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels
{
    public abstract class Level : MonoBehaviour
    {
        protected const float MinValue = 0f;
        
        [field: SerializeField] public bool IsLaunchedPlayerCapsule { get; private set; }
        
        [field: SerializeField] public EndLevelTrigger EndLevelTrigger { get; private set; }
        
        [field: SerializeField] public EntranceTrigger EntranceTrigger { get; private set; }
        
        [field: SerializeField] public Transform StartPoint { get; private set; }
        
        [SerializeField] protected float Delay = 10f;

        protected Timer Timer;
        protected AdviserMessagePanel AdviserMessagePanel;
        protected GameInitSystem GameInitSystem;
        protected float LastSpawnTime;

        public void GetServices(GameInitSystem gameInitSystem, Timer timer, AdviserMessagePanel adviserMessagePanel)
        {
            GameInitSystem = gameInitSystem;
            AdviserMessagePanel = adviserMessagePanel;
            Timer = timer;
        }

        protected virtual void CreateWaveOfEnemy()
        {
            if (LastSpawnTime <= MinValue)
            {
                GameInitSystem.SpawnSmallEnemyAlien();

                LastSpawnTime = Delay;
            }

            LastSpawnTime -= Time.deltaTime;
        }

        protected void SpawnPlayer()
        {
            if (IsLaunchedPlayerCapsule)
            {
                GameInitSystem.CreateCapsule();
            }
            else
            {
                GameInitSystem.SpawnPlayer();
            }
        }
    }
}