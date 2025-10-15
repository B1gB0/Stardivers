using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.ThirdLevel
{
    public class ThirdMysteryPlanetLevel : Level
    {
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;

        private void OnEnable()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            
            IsInitiatedSpawners += SpawnResources;
        }

        private void OnDisable()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            IsInitiatedSpawners -= SpawnResources;
        }

        private void FixedUpdate()
        {
            if (EnemySpawnFirstWaveTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
        }
    }
}