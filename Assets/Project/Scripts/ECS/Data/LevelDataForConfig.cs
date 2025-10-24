#if UNITY_EDITOR
using UnityEngine;

namespace Project.Scripts.ECS.Data
{

    public class LevelDataForConfig : MonoBehaviour
    {
        [SerializeField] private LevelInitData levelInitData;

        [ContextMenu("Save Data")]
        public void SaveDataToConfigLevel()
        {
            GameObject[] enemyPatrolPoints = GameObject.FindGameObjectsWithTag("EnemyPatrolPoints");
            
            GameObject[] firstWaveSmallEnemy = GameObject.FindGameObjectsWithTag("FirstWaveSmallEnemyAlienSpawnPoint");
            GameObject[] firstWaveBigEnemy = GameObject.FindGameObjectsWithTag("FirstWaveBigEnemyAlienSpawnPoint");
            GameObject[] firstWaveGunnerEnemy = 
                GameObject.FindGameObjectsWithTag("FirstWaveGunnerEnemyAlienSpawnPoint");
            
            GameObject[] secondWaveSmallEnemy = 
                GameObject.FindGameObjectsWithTag("SecondWaveSmallEnemyAlienSpawnPoint");
            GameObject[] secondWaveBigEnemy = GameObject.FindGameObjectsWithTag("SecondWaveBigEnemyAlienSpawnPoint");
            GameObject[] secondWaveGunnerEnemy = 
                GameObject.FindGameObjectsWithTag("SecondWaveGunnerEnemyAlienSpawnPoint");
            
            GameObject[] enemyTurretSpawnPoints = 
                GameObject.FindGameObjectsWithTag("EnemyTurretSpawnPoints");
            
            GameObject[] alienCocoonSpawnPoints = GameObject.FindGameObjectsWithTag("AlienCocoonSpawnPoints");
            GameObject[] stoneSpawnPoints = GameObject.FindGameObjectsWithTag("StoneSpawnPoint");
            GameObject[] healingCoreSpawnPoints = GameObject.FindGameObjectsWithTag("HealingCoreSpawnPoint");
            GameObject[] goldCoreSpawnPoints = GameObject.FindGameObjectsWithTag("GoldCoreSpawnPoint");
            GameObject playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
            
            levelInitData.EnemyPatrolPositions.Clear();
            levelInitData.FirstWaveSmallEnemyAlienSpawnPositions.Clear();
            levelInitData.FirstWaveBigEnemyAlienSpawnPositions.Clear();
            levelInitData.FirstWaveGunnerEnemyAlienSpawnPositions.Clear();
            levelInitData.SecondWaveSmallEnemyAlienSpawnPositions.Clear();
            levelInitData.SecondWaveBigEnemyAlienSpawnPositions.Clear();
            levelInitData.SecondWaveGunnerEnemyAlienSpawnPositions.Clear();
            levelInitData.StoneSpawnPositions.Clear();
            levelInitData.HealingCoreSpawnPositions.Clear();
            levelInitData.GoldCoreSpawnPositions.Clear();
            levelInitData.AlienCocoonSpawnPoints.Clear();
            levelInitData.EnemyTurretsSpawnPoints.Clear();
        
            levelInitData.PlayerSpawnPosition = playerSpawnPoint.transform.position;
            
            foreach (var point in enemyPatrolPoints)
            {
                levelInitData.EnemyPatrolPositions.Add(point.transform.position);
            }
        
            foreach (var point in firstWaveSmallEnemy)
            {
                levelInitData.FirstWaveSmallEnemyAlienSpawnPositions.Add(point.transform.position);
            }
        
            foreach (var point in firstWaveBigEnemy)
            {
                levelInitData.FirstWaveBigEnemyAlienSpawnPositions.Add(point.transform.position);
            }
            
            foreach (var point in firstWaveGunnerEnemy)
            {
                levelInitData.FirstWaveGunnerEnemyAlienSpawnPositions.Add(point.transform.position);
            }
            
            foreach (var point in secondWaveSmallEnemy)
            {
                levelInitData.SecondWaveSmallEnemyAlienSpawnPositions.Add(point.transform.position);
            }
        
            foreach (var point in secondWaveBigEnemy)
            {
                levelInitData.SecondWaveBigEnemyAlienSpawnPositions.Add(point.transform.position);
            }
            
            foreach (var point in secondWaveGunnerEnemy)
            {
                levelInitData.SecondWaveGunnerEnemyAlienSpawnPositions.Add(point.transform.position);
            }
            
            foreach (var point in enemyTurretSpawnPoints)
            {
                levelInitData.EnemyTurretsSpawnPoints.Add(point.transform.position);
            }
            
            foreach (var point in alienCocoonSpawnPoints)
            {
                levelInitData.AlienCocoonSpawnPoints.Add(point.transform.position);
            }

            foreach (var stone in stoneSpawnPoints)
            {
                levelInitData.StoneSpawnPositions.Add(stone.transform.position);
            }
        
            foreach (var healingCore in healingCoreSpawnPoints)
            {
                levelInitData.HealingCoreSpawnPositions.Add(healingCore.transform.position);
            }
        
            foreach (var goldCore in goldCoreSpawnPoints)
            {
                levelInitData.GoldCoreSpawnPositions.Add(goldCore.transform.position);
            }
            
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(levelInitData);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif
