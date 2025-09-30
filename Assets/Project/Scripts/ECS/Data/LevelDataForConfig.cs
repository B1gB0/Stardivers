using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    public class LevelDataForConfig : MonoBehaviour
    {
        [SerializeField] private LevelInitData levelInitData;

        [ContextMenu("Save Data")]
        public void SaveDataToConfigLevel()
        {
            GameObject[] firstWaveSmallEnemy = GameObject.FindGameObjectsWithTag("FirstWaveSmallEnemyAlienSpawnPoint");
            GameObject[] firstWaveBigEnemy = GameObject.FindGameObjectsWithTag("FirstWaveBigEnemyAlienSpawnPoint");
            GameObject[] firstWaveGunnerEnemy = GameObject.FindGameObjectsWithTag("FirstWaveGunnerEnemyAlienSpawnPoint");
            
            GameObject[] secondWaveSmallEnemy = GameObject.FindGameObjectsWithTag("SecondWaveSmallEnemyAlienSpawnPoint");
            GameObject[] secondWaveBigEnemy = GameObject.FindGameObjectsWithTag("SecondWaveBigEnemyAlienSpawnPoint");
            GameObject[] secondWaveGunnerEnemy = GameObject.FindGameObjectsWithTag("SecondWaveGunnerEnemyAlienSpawnPoint");
            
            GameObject[] stoneSpawnPoints = GameObject.FindGameObjectsWithTag("StoneSpawnPoint");
            GameObject[] healingCoreSpawnPoints = GameObject.FindGameObjectsWithTag("HealingCoreSpawnPoint");
            GameObject[] goldCoreSpawnPoints = GameObject.FindGameObjectsWithTag("GoldCoreSpawnPoint");
            GameObject playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        
            levelInitData.PlayerSpawnPosition = playerSpawnPoint.transform.position;
        
            foreach (var point in firstWaveSmallEnemy)
            {
                if(firstWaveSmallEnemy.Length != levelInitData.FirstWaveSmallEnemyAlienSpawnPositions.Count)
                    levelInitData.FirstWaveSmallEnemyAlienSpawnPositions.Add(point.transform.position);
            }
        
            foreach (var point in firstWaveBigEnemy)
            {
                if(firstWaveBigEnemy.Length != levelInitData.FirstWaveBigEnemyAlienSpawnPositions.Count)
                    levelInitData.FirstWaveBigEnemyAlienSpawnPositions.Add(point.transform.position);
            }
            
            foreach (var point in firstWaveGunnerEnemy)
            {
                if(firstWaveGunnerEnemy.Length != levelInitData.FirstWaveGunnerEnemyAlienSpawnPositions.Count)
                    levelInitData.FirstWaveGunnerEnemyAlienSpawnPositions.Add(point.transform.position);
            }
            
            foreach (var point in secondWaveSmallEnemy)
            {
                if(secondWaveSmallEnemy.Length != levelInitData.SecondWaveSmallEnemyAlienSpawnPositions.Count)
                    levelInitData.SecondWaveSmallEnemyAlienSpawnPositions.Add(point.transform.position);
            }
        
            foreach (var point in secondWaveBigEnemy)
            {
                if(secondWaveBigEnemy.Length != levelInitData.SecondWaveBigEnemyAlienSpawnPositions.Count)
                    levelInitData.SecondWaveBigEnemyAlienSpawnPositions.Add(point.transform.position);
            }
            
            foreach (var point in secondWaveGunnerEnemy)
            {
                if(secondWaveGunnerEnemy.Length != levelInitData.SecondWaveGunnerEnemyAlienSpawnPositions.Count)
                    levelInitData.SecondWaveGunnerEnemyAlienSpawnPositions.Add(point.transform.position);
            }

            foreach (var stone in stoneSpawnPoints)
            {
                if(stoneSpawnPoints.Length != levelInitData.StoneSpawnPositions.Count)
                    levelInitData.StoneSpawnPositions.Add(stone.transform.position);
            }
        
            foreach (var healingCore in healingCoreSpawnPoints)
            {
                if(healingCoreSpawnPoints.Length != levelInitData.HealingCoreSpawnPositions.Count)
                    levelInitData.HealingCoreSpawnPositions.Add(healingCore.transform.position);
            }
        
            foreach (var goldCore in goldCoreSpawnPoints)
            {
                if(goldCoreSpawnPoints.Length != levelInitData.GoldCoreSpawnPositions.Count)
                    levelInitData.GoldCoreSpawnPositions.Add(goldCore.transform.position);
            }
        }
    }
}
