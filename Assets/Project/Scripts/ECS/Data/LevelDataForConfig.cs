using UnityEngine;

public class LevelDataForConfig : MonoBehaviour
{
    [SerializeField] private LevelInitData levelInitData;

    [ContextMenu("Save Data")]
    public void SaveDataToConfigLevel()
    {
        GameObject[] smallEnemyAlienSpawnPoints = GameObject.FindGameObjectsWithTag("SmallEnemyAlienSpawnPoint");
        GameObject[] bigEnemyAlienSpawnPoints = GameObject.FindGameObjectsWithTag("BigEnemyAlienSpawnPoint");
        GameObject[] stoneSpawnPoints = GameObject.FindGameObjectsWithTag("StoneSpawnPoint");
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

        foreach (var stone in stoneSpawnPoints)
        {
            levelInitData.StoneSpawnPoints.Add(stone.transform.position);
        }
    }
}
