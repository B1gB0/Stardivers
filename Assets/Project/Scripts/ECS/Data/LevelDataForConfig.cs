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
                if(smallEnemyAlienSpawnPoints.Length != levelInitData.SmallEnemyAlienSpawnPoints.Count)
                    levelInitData.SmallEnemyAlienSpawnPoints.Add(point.transform.position);
            }
        
            foreach (var point in bigEnemyAlienSpawnPoints)
            {
                if(bigEnemyAlienSpawnPoints.Length != levelInitData.BigEnemyAlienSpawnPoints.Count)
                    levelInitData.BigEnemyAlienSpawnPoints.Add(point.transform.position);
            }
            
            foreach (var point in gunnerEnemyAlienSpawnPoints)
            {
                if(gunnerEnemyAlienSpawnPoints.Length != levelInitData.GunnerEnemyAlienSpawnPoints.Count)
                    levelInitData.GunnerEnemyAlienSpawnPoints.Add(point.transform.position);
            }

            foreach (var stone in stoneSpawnPoints)
            {
                if(stoneSpawnPoints.Length != levelInitData.StoneSpawnPoints.Count)
                    levelInitData.StoneSpawnPoints.Add(stone.transform.position);
            }
        
            foreach (var healingCore in healingCoreSpawnPoints)
            {
                if(healingCoreSpawnPoints.Length != levelInitData.HealingCoreSpawnPoints.Count)
                    levelInitData.HealingCoreSpawnPoints.Add(healingCore.transform.position);
            }
        
            foreach (var goldCore in goldCoreSpawnPoints)
            {
                if(goldCoreSpawnPoints.Length != levelInitData.GoldCoreSpawnPoints.Count)
                    levelInitData.GoldCoreSpawnPoints.Add(goldCore.transform.position);
            }
        }
    }
}
