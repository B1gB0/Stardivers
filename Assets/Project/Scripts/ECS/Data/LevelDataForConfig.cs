using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    public class LevelDataForConfig : MonoBehaviour
    {
        [SerializeField] private LevelInitData levelInitData;

        [ContextMenu("Save Data")]
        public void SaveDataToConfigLevel()
        {
            GameObject[] smallEnemyAlienSpawnPoints = GameObject.FindGameObjectsWithTag("SmallEnemyAlienSpawnPoint");
            GameObject[] bigEnemyAlienSpawnPoints = GameObject.FindGameObjectsWithTag("BigEnemyAlienSpawnPoint");
            GameObject[] gunnerEnemyAlienSpawnPoints = GameObject.FindGameObjectsWithTag("GunnerEnemyAlienSpawnPoint");
            GameObject[] stoneSpawnPoints = GameObject.FindGameObjectsWithTag("StoneSpawnPoint");
            GameObject[] healingCoreSpawnPoints = GameObject.FindGameObjectsWithTag("HealingCoreSpawnPoint");
            GameObject[] goldCoreSpawnPoints = GameObject.FindGameObjectsWithTag("GoldCoreSpawnPoint");
            GameObject playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        
            levelInitData.PlayerSpawnPoint = playerSpawnPoint.transform.position;
        
            foreach (var point in smallEnemyAlienSpawnPoints)
            {
                levelInitData.SmallEnemyAlienSpawnPoints.Add(point.transform.position);
            }
        
            foreach (var point in bigEnemyAlienSpawnPoints)
            {
                levelInitData.BigEnemyAlienSpawnPoints.Add(point.transform.position);
            }
            
            foreach (var point in gunnerEnemyAlienSpawnPoints)
            {
                levelInitData.GunnerEnemyAlienSpawnPoints.Add(point.transform.position);
            }

            foreach (var stone in stoneSpawnPoints)
            {
                levelInitData.StoneSpawnPoints.Add(stone.transform.position);
            }
        
            foreach (var healingCore in healingCoreSpawnPoints)
            {
                levelInitData.HealingCoreSpawnPoints.Add(healingCore.transform.position);
            }
        
            foreach (var goldCore in goldCoreSpawnPoints)
            {
                levelInitData.GoldCoreSpawnPoints.Add(goldCore.transform.position);
            }
        }
    }
}
